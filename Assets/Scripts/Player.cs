using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInputActions inputActions;
    [SerializeField] float moveSpeed = 10f;

    private new Rigidbody rigidbody;
    private InputAction move;
    private InputAction fire;
    private InputAction keyboard;

    private Vector2 moveDirection;

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
        fire.performed += OnFire;

        keyboard = inputActions.Player.Keyboard;
        keyboard.Enable();
        keyboard.performed += OnEscape;
    }

    void OnDisable()
    {
        move.Disable();

        fire.Disable();
        fire.performed -= OnFire;
        
        keyboard.Disable();
        keyboard.performed -= OnEscape;
    }

    // Update is called once per frame
    void Update() => moveDirection = move.ReadValue<Vector2>();

    void FixedUpdate() => rigidbody.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.y * moveSpeed);

    private void OnFire(InputAction.CallbackContext context) => Debug.Log("Fire");

    private void OnEscape(InputAction.CallbackContext context) => Debug.Log("Escape");
}