using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] float moveSpeed = 10f;

    private new Rigidbody rigidbody;
    private InputAction move;
    private InputAction fire;
    private InputAction keyboard;
    private InputAction click;
    private Vector2 moveDirection;
    private PlayerInputActions inputActions;

    void Awake() => inputActions = new PlayerInputActions();

    // Start is called before the first frame update
    void Start() => ResolveDependencies();

    private void ResolveDependencies() => rigidbody = GetComponent<Rigidbody>() as Rigidbody;

    void OnEnable()
    {
        move = inputActions.Player.Move;
        move.Enable();

        fire = inputActions.Player.Fire;
        fire.Enable();
        fire.performed += Callback_OnFirePerformed;

        keyboard = inputActions.Player.Custom;
        keyboard.Enable();
        keyboard.performed += Callback_OnEscapeKeyPerformed;

        click = inputActions.UI.Click;
        click.Enable();
        click.performed += Callback_OnLeftMouseButtonPerformed;
    }

    void OnDisable()
    {
        move.Disable();

        fire.Disable();
        fire.performed -= Callback_OnFirePerformed;
        
        keyboard.Disable();
        keyboard.performed -= Callback_OnEscapeKeyPerformed;

        click.Enable();
        click.performed -= Callback_OnLeftMouseButtonPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        // A Unity Event is triggered every time an action is started, performed, and canceled.
        // While an InputAction is actuated, it will trigger a performed event each time the underlying value changes.
        // An action such as a digital button will typically fire a single started, performed, and canceled event.
        // Holding a button for 5 seconds, for example, would merely delay the canceled event.
        // On the other hand, a more variable value, such as a gamepad stick Vector2 value or a Mouse delta Vector2,
        // would trigger a started, typically followed by many performed events, and finally, a canceled event.
        if (inputActions.Player.Custom.triggered)
        {
            OnEscapeKeyTriggered();
        }

        if (inputActions.UI.Click.triggered)
        {
            OnLeftMouseButtonTriggered();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnEscapeKeyPressedThisFrame();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnLeftMouseButtonPressedThisFrame();
        }
    }

    // Frame-rate independent call for physics calculations
    void FixedUpdate() => rigidbody.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.y * moveSpeed);

    private void Callback_OnFirePerformed(InputAction.CallbackContext context) => Debug.Log("Callback_OnFirePerformed");

    private void Callback_OnEscapeKeyPerformed(InputAction.CallbackContext context) => Debug.Log("Callback_OnEscapePerformed");
    
    private void Callback_OnLeftMouseButtonPerformed(InputAction.CallbackContext context) => Debug.Log("Callback_OnLeftMouseButtonPerformed");

    private void OnEscapeKeyTriggered() => Debug.Log("OnEscapeTriggered");

    private void OnLeftMouseButtonTriggered() => Debug.Log("OnLeftMouseButtonTriggered");

    private void OnEscapeKeyPressedThisFrame() => Debug.Log("OnEscapeKeyPressedThisFrame");

    private void OnLeftMouseButtonPressedThisFrame() => Debug.Log("OnLeftMouseButtonPressedThisFrame");
}