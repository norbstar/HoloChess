using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

using Enum;

[RequireComponent(typeof(RaycastNotifier))]
[RequireComponent(typeof(ActionBasedController))]
[RequireComponent(typeof(PrefabToLayerMap))]
public class HandController : GizmoManager
{
    [Header("Config")]
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

    private RaycastNotifier notifier;
    private ActionBasedController controller;
    private PrefabToLayerMap pointerMap;
    private InputDeviceCharacteristics characteristics;
    private LayerMask layerMask;
    private GameObject pointerPrefab, pointer;

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
    }

    private void ResolveDependencies()
    {
        controller = GetComponent<ActionBasedController>() as ActionBasedController;
        pointerMap = GetComponent<PrefabToLayerMap>() as PrefabToLayerMap;
        notifier = Notifier;
    }

    // Start is called before the first frame update
    void Start() => layerMask = pointerMap.LayerMask;

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

        foreach (RaycastNotifier.HitInfo hitInfo in hits)
        {
            var target = hitInfo.hit.transform.gameObject;
            bool inLayerMask = LayerMaskExtensions.HasLayer(layerMask, target.layer);

            if (!inLayerMask) continue;

            bool handleHit = true;

            if (target.TryGetComponent<RootResolver>(out RootResolver rootResolver))
            {
                target =  rootResolver.Root;
            }

            if (target.TryGetComponent<FocusResoiver>(out FocusResoiver focusResolver))
            {
                handleHit = focusResolver.ShouldReceivePointer();
            }

            if (!handleHit) continue;

            var distance = Vector3.Distance(hitInfo.hit.point, transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hitInfo.hit;
            }
        }

        if (closestHit.HasValue)
        {
            this.hit = closestHit.Value;
            var target = hit.transform.gameObject;
            Vector3 point = hit.point;
            hasHit = true;

            if (pointerMap.TryGetMapItem(target.layer, out GameObject prefab))
            {
                if (pointer != null && !pointer.name.Equals(prefab.name))
                {
                    pointer.gameObject.SetActive(false);
                    bool hasPrefab = false;

                    foreach (Transform child in transform)
                    {
                        if (child.name.Equals(prefab.name))
                        {
                            pointer = child.gameObject;
                            pointer.SetActive(true);
                            hasPrefab = true;
                        }
                    }

                    if (!hasPrefab) pointer = null;
                }
                
                pointerPrefab = prefab;

                if (pointer == null)
                {
                    pointer = Instantiate(pointerPrefab, point + (hit.normal * pointerOffset), Quaternion.identity);
                    pointer.transform.parent = transform;
                    pointer.name = pointerPrefab.name;
                }
                else
                {
                    pointer.transform.position = point + (hit.normal * pointerOffset);
                }
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