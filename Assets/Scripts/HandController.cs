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
    [Header("Config")]
    [SerializeField] GameObject pointerPrefab;
    [SerializeField] List<string> compositeMask;

    public static InputDeviceCharacteristics RightHandCharacteristics = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right);
    public static InputDeviceCharacteristics LeftHandCharacteristics = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left);

    public delegate void ActuationEvent(ControllerEnums.Actuation actuation, InputDeviceCharacteristics characteristics);
    public event ActuationEvent AcutationEventReceived;

    public delegate void RaycastEvent(HandController controller, GameObject gameObject, Vector3 point);
    public event RaycastEvent RaycastEventReceived;

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

        layerMask = LayerMaskExtensions.CreateLayerMask(compositeMask);
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

    private void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            var target = hit.transform.gameObject;
            
            if (LayerMaskExtensions.HasLayer(layerMask, target.layer))
            {
                Debug.Log($"{gameObject.name} OnRaycastEvent");

                hasHit = true;
                this.hit = hit;

                Vector3 point = hit.point;

                if (enablePointer)
                {
                    if (pointer == null)
                    {
                        pointer = Instantiate(pointerPrefab, point, Quaternion.identity);
                    }
                    else
                    {
                        pointer.transform.position = point;
                    }
                }

                RaycastEventReceived?.Invoke(this, source, point);
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