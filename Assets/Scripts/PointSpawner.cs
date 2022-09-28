using UnityEngine;
using UnityEngine.InputSystem;

using Utilities.Points;

public class PointSpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] InputActionReference leftHandPositionFromReference;
    [SerializeField] InputActionReference rightHandPositionFromReference;
    // [SerializeField] InputActionReference headPositionFromReference;
    [SerializeField] InputActionReference leftHandActivateFromReference;
    [SerializeField] InputActionReference rightHandActivateFromReference;

    public class Config
    {
        public Vector3 point;
        public PointProjector.Type type = PointProjector.Type.White;
        public string name = "Point";
    }

    // private XRIInputActions inputActions;
    // private InputAction leftHandPosition, headPosition, leftHandActivate;


    // void Awake() => inputActions = new XRIInputActions();

    void OnEnable()
    {
        leftHandActivateFromReference.action.performed += Callback_OnLeftHandActivateFromReferencePerformed;
        rightHandActivateFromReference.action.performed += Callback_OnRightHandActivateFromReferencePerformed;

        // leftHandActivate = inputActions.XRIRightHandInteraction.Activate;
        // leftHandActivate.Enable();
        // leftHandActivate.performed += Callback_OnLeftHandActionPerformed;

        // leftHandPosition = inputActions.XRILeftHand.Position;
        // leftHandPosition.Enable();

        // headPosition = inputActions.XRIHead.Position;
        // headPosition.Enable();
    }

    void OnDisable()
    {
        leftHandActivateFromReference.action.performed -= Callback_OnLeftHandActivateFromReferencePerformed;
        rightHandActivateFromReference.action.performed -= Callback_OnRightHandActivateFromReferencePerformed;

        // leftHandActivate.Disable();
        // leftHandActivate.performed -= Callback_OnLeftHandActionPerformed;

        // leftHandPosition.Disable();
        // headPosition.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position;

        // position = leftHandPosition.ReadValue<Vector3>();
        // Debug.Log($"Left Hand Position : {position}");

        // position = headPosition.ReadValue<Vector3>();
        // Debug.Log($"Head Position : {position}");

        position = leftHandPositionFromReference.action.ReadValue<Vector3>();
        Debug.Log($"<color=red>Left</color> Hand Position From Reference : {position}");

        position = rightHandPositionFromReference.action.ReadValue<Vector3>();
        Debug.Log($"<color=blue>Right</color> Hand Position From Reference : {position}");

        // position = leftHandPositionFromReference.ToInputAction().ReadValue<Vector3>();
        // Debug.Log($"Left Hand Position From Reference [2] : {position}");

        // position = headPositionFromReference.action.ReadValue<Vector3>();
        // Debug.Log($"Head Position From Reference : {position}");

        // position = headPositionFromReference.ToInputAction().ReadValue<Vector3>();
        // Debug.Log($"Head Position From Reference [2] : {position}");
    }

    private void Callback_OnLeftHandActivateFromReferencePerformed(InputAction.CallbackContext context)
    {
        UnityEngine.InputSystem.InputDevice device = context.control.device;
        Debug.Log($"<color=red>Left</color> Hand Action From Reference : {device.name}");

        // var usages = InputSystem.LoadLayout("XRController").commonUsages;
        
        foreach (var usage in device.usages)
        {
            Debug.Log($"<color=red>Left</color> Hand Action From Reference Usage : {usage}");
        }

        var position = leftHandPositionFromReference.action.ReadValue<Vector3>();
        Spawn(position, new Config
        {
            type = PointProjector.Type.Red,
            name = "<color=red>Left</color> Point"
        });
    }

    private void Callback_OnRightHandActivateFromReferencePerformed(InputAction.CallbackContext context)
    {
        UnityEngine.InputSystem.InputDevice device = context.control.device;
        Debug.Log($"<color=blue>Right</color> Hand Action From Reference : {device.name}");

        // var usages = InputSystem.LoadLayout("XRController").commonUsages;
        
        foreach (var usage in device.usages)
        {
            Debug.Log($"<color=blue>Right</color> Hand Action From Reference Usage : {usage}");
        }

        var position = rightHandPositionFromReference.action.ReadValue<Vector3>();
        Spawn(position, new Config
        {
            type = PointProjector.Type.Blue,
            name = "<color=blue>Right</color> Point"
        });
    }

    // private void Callback_OnLeftHandActionPerformed(InputAction.CallbackContext context)
    // {
    //     UnityEngine.InputSystem.InputDevice device = context.control.device;
    //     Debug.Log($"Left Hand Action : {device.name}");

    //     foreach (var usage in device.usages)
    //     {
    //         Debug.Log($"Left Hand Action Usage : {usage}");
    //     }

    //     var position = leftHandPosition.ReadValue<Vector3>();
    //     Spawn(position);
    // }

    public PointProjector Spawn(Vector3 point, Config config = null)
    {
        if (config == null)
        {
            config = new Config();
        }

        return PointProjectorDatabase.PlotPoint($"{config.name}", $"{config.name}", config.type, point);
    }
}