using UnityEngine;
using UnityEngine.InputSystem;

public class SceneNavigator : CachedObject<SceneNavigator>
{
    [SerializeField] InputAction inputAction;

    void OnEnable()
    {
        inputAction.Enable();
        inputAction.performed += OnEscape;
    }

    void OnDisable()
    {
        inputAction.Disable();
        inputAction.performed -= OnEscape;
    }

    private void OnEscape(InputAction.CallbackContext context) => Debug.Log("Escape");
}