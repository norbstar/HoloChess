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
    private RaycastNotifier raycastNotifier;
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
        raycastNotifier = GetComponent<RaycastNotifier>() as RaycastNotifier;
    }

    public InputDeviceCharacteristics Characteristics { get { return characteristics; } }
    public Hand WhichHand{ get { return hand; } }

    void OnEnable() => raycastNotifier.EventReceived += OnRaycastEvent;

    void OnDisable() => raycastNotifier.EventReceived -= OnRaycastEvent;

    private void OnRaycastEvent(GameObject source, Vector3 point)
    {
        Debug.Log($"Hand Controller Source : {source.name} Point : {point}");
        RaycastEventReceived?.Invoke(this, source, point);
    }
}