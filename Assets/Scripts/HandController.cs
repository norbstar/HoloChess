using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

using Enum;

[RequireComponent(typeof(RaycastNotifier))]
[RequireComponent(typeof(ActionBasedController))]
public class HandController : GizmoManager
{
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

    private ActionBasedController controller;
    private RaycastNotifier notifier;
    private InputDeviceCharacteristics characteristics;

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
        notifier = Notifier;
    }

    public InputDeviceCharacteristics Characteristics { get { return characteristics; } }
    public Hand WhichHand{ get { return hand; } }
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

    void OnEnable() => notifier.EventReceived += OnRaycastEvent;

    void OnDisable() => notifier.EventReceived -= OnRaycastEvent;

    private void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
    {
        // GameObject source = hit.transform.gameObject;
        Vector3 point = hit.point;
        
        // Debug.Log($"Hand Controller Origin : {origin.name} Source : {source.name} Point : {point}");
        RaycastEventReceived?.Invoke(this, source, point);
    }
}