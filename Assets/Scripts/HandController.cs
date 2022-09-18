using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

using Enum;

[RequireComponent(typeof(RaycastNotifier))]
[RequireComponent(typeof(ActionBasedController))]
public class HandController : GizmoManager
{
    [Serializable]
    public class PointerConfig
    {
        public GameObject prefab;
        public List<string> compositeMask;
    }

    [Serializable]
    public class Pointers
    {
        public GameObject defaultPrefab;
        public List<string> defaultCompositeMask;
        public List<PointerConfig> configs;
    }

    [Header("Config")]
    [SerializeField] Pointers pointers;
    [SerializeField] float pointerOffset = 0.2f;

    public static InputDeviceCharacteristics RightHandCharacteristics = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right);
    public static InputDeviceCharacteristics LeftHandCharacteristics = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left);

    public delegate void ActuationEvent(ControllerEnums.Actuation actuation, InputDeviceCharacteristics characteristics);
    public event ActuationEvent AcutationEventReceived;

    public enum Hand
    {
        Left,
        Right
    }

    [Header("Config")]
    [SerializeField] Hand hand;
    [SerializeField] bool enablePointer = false;

    private ActionBasedController controller;
    private LayerMask layerMask;
    private RaycastNotifier notifier;
    private InputDeviceCharacteristics characteristics;
    private GameObject pointer;

    public RaycastNotifier Notifier
    {
        get
        {
            if (notifier == null)
            {
                notifier = GetComponent<RaycastNotifier>() as RaycastNotifier;
            }

            return notifier;
        }
    }

    public InputDeviceCharacteristics Characteristics { get { return characteristics; } }
    public Hand WhichHand{ get { return hand; } }

    private bool hasHit;
    private RaycastHit hit;

    void Awake()
    {
        ResolveDependencies();

        switch (hand)
        {
            case Hand.Left:
                characteristics = LeftHandCharacteristics;
                break;

            case Hand.Right:
                characteristics = RightHandCharacteristics;
                break;
        }

        layerMask = LayerMaskExtensions.CreateLayerMask(pointers.defaultCompositeMask);
    }

    private void ResolveDependencies()
    {
        controller = GetComponent<ActionBasedController>() as ActionBasedController;
        notifier = Notifier;
    }

    void OnEnable() => notifier.EventReceived += OnRaycastEvent;

    void OnDisable() => notifier.EventReceived -= OnRaycastEvent;

    // Update is called once per frame
    void Update()
    {
        if ((!enablePointer) || (pointer == null) || (!pointer.gameObject.activeSelf)) return;
        pointer.transform.LookAt(hit.point + -hit.normal);
    }

    private void OnRaycastEvent(GameObject source, List<RaycastNotifier.HitInfo> hits)
    {
        if (!enablePointer) return;

        float closestDistance = float.MaxValue;
        RaycastHit? closestHit = null;

        System.Text.StringBuilder logBuilder = new System.Text.StringBuilder();
        logBuilder.Append($"{gameObject.name} OnRaycastEvent Hit Count: {hits.Count}");

        foreach (RaycastNotifier.HitInfo hitInfo in hits)
        {
            var target = hitInfo.hit.transform.gameObject;
            logBuilder.Append($"\n [1] Hit Target : {target.name}");
            
            if (!LayerMaskExtensions.HasLayer(layerMask, target.layer)) continue;

            bool handleHit = true;

            if (target.TryGetComponent<RootResolver>(out RootResolver rootResolver))
            {
                target =  rootResolver.Root;
            }

            if (target.TryGetComponent<FocusResoiver>(out FocusResoiver focusResolver))
            {
                logBuilder.Append($"\n [2] Resolved FocusResolver");
                handleHit = focusResolver.ShouldReceivePointer();
                logBuilder.Append($"\n [3] FocusResolver Handle Hit : {handleHit}");
            }

            logBuilder.Append($"\n [4] Handle Hit : {handleHit}");
            
            if (!handleHit) continue;

            var distance = Vector3.Distance(hitInfo.hit.point, transform.position);
            logBuilder.Append($"\n [5] Hit Distance : {distance}");

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hitInfo.hit;
            }
        }

        logBuilder.Append($"\n [6] Closest Hit : {closestHit?.collider.gameObject.name}");
        Debug.Log(logBuilder.ToString());

        if (closestHit.HasValue)
        {
            this.hit = closestHit.Value;
            var target = hit.transform.gameObject;
            Vector3 point = hit.point;
            hasHit = true;

            if (pointer == null)
            {
                pointer = Instantiate(pointers.defaultPrefab, point + (hit.normal * pointerOffset), Quaternion.identity);
                pointer.transform.parent = transform;
            }
            else
            {
                pointer.transform.position = point + (hit.normal * pointerOffset);
            }
        }
    }

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        pointer?.gameObject.SetActive(hasHit);
        hasHit = false;
    }
}