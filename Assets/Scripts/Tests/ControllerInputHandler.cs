using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Tests
{
    public class ControllerInputHandler : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] UnityEvent @event;

        private XRIInputActions inputActions;
        private InputAction menu;

        void Awake() => inputActions = new XRIInputActions();

        void OnEnable()
        {
            menu = inputActions.XRIOther.Menu;
            menu.Enable();
            menu.performed += Callback_OnMenuPerformed;
        }

        void OnDisable()
        {
            menu.Disable();
            menu.performed -= Callback_OnMenuPerformed;
        }

#if false
        // Update is called once per frame
        void Update()
        {
            if (menu == null) return;

            if (menu.triggered)
            {
                OnActionTriggered();
            }
        }
#endif

        private void Callback_OnMenuPerformed(InputAction.CallbackContext context)
        {
            // Debug.Log("Callback_OnMenuPerformed");
            PostEvents();
        }

        private void OnMenuTriggered()
        {
            // Debug.Log("OnMenuTriggered");
            PostEvents();
        }

        private void PostEvents() => @event.Invoke();
    }
}