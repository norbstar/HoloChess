using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SimplePlayer : MonoBehaviour
{
    [SerializeField] InputAction inputAction;
    [SerializeField] float moveSpeed = 10f;

    private new Rigidbody rigidbody;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start() => ResolveDependencies();

    private void ResolveDependencies() => rigidbody = GetComponent<Rigidbody>() as Rigidbody;

    void OnEnable() => inputAction.Enable();

    void OnDisable() => inputAction.Disable();

    // Update is called once per frame
    void Update() => moveDirection = inputAction.ReadValue<Vector2>();

    // Frame-rate independent call for physics calculations
    void FixedUpdate() => rigidbody.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.y * moveSpeed);
}