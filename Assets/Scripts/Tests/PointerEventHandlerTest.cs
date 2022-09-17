using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.InputSystem;

using UI;

namespace Tests
{
    [RequireComponent(typeof(PointerEventHandler))]
    public class PointerEventHandlerTest : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] GameObject pointer;
        
        [Header("Haptics")]
        [SerializeField] bool enableHaptics = false;
        public bool EnableHaptics { get { return enableHaptics; } }
        [SerializeField] float hapticsAmplitude = 0.15f;
        public float HapticsAmplitude { get { return hapticsAmplitude; } }
        [SerializeField] float hapticsDuration = 0.1f;
        public float HapticsDuration { get { return hapticsDuration; } }

        private SpriteRenderer pointerRenderer;
        private XRIInputActions inputActions;
        private InputAction point;
        private bool isPointerEnter, isPointerDown;
        // private PointerEventData eventData;

        void Awake()
        {
            ResolveDependencies();
            inputActions = new XRIInputActions();
        }

        private void ResolveDependencies() => pointerRenderer = pointer.GetComponent<SpriteRenderer>() as SpriteRenderer;

        void OnEnable()
        {
            var eventHandler = GetComponent<PointerEventHandler>() as PointerEventHandler;
            eventHandler.EventReceived += OnPointerEvent;

            point = inputActions.XRIUI.Point;
            point.Enable();
            point.performed += Callback_OnPointPerformed;
        }

        void OnDisable()
        {
            var eventHandler = GetComponent<PointerEventHandler>() as PointerEventHandler;
            eventHandler.EventReceived -= OnPointerEvent;

            point.Disable();
            point.performed -= Callback_OnPointPerformed;
        }

        private XRUIInputModule GetXRInputModule() => EventSystem.current.currentInputModule as XRUIInputModule;

        private bool TryGetXRRayInteractor(int pointerID, out XRRayInteractor rayInteractor)
        {
            var inputModule = GetXRInputModule();

            if (inputModule == null)
            {
                rayInteractor = null;
                return false;
            }
    
            rayInteractor = inputModule.GetInteractor(pointerID) as XRRayInteractor;
            return (rayInteractor != null);
        }

        private void OnPointerEvent(GameObject gameObject, PointerEventHandler.Event @event, PointerEventData eventData)
        {
            Debug.Log($"OnPointerEvent : {@event}");

            switch (@event)
            {
                case PointerEventHandler.Event.Enter:
                    OnPointerEnter(gameObject, eventData);
                    break;

                case PointerEventHandler.Event.Down:
                    OnPointerDown(gameObject, eventData);
                    break;

                case PointerEventHandler.Event.Up:
                    OnPointerUp(gameObject, eventData);
                    break;

                case PointerEventHandler.Event.Exit:
                    OnPointerExit(gameObject, eventData);
                    break;
            }
        }

        private void OnPointerEnter(GameObject gameObject, PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            isPointerEnter = true;
            pointerRenderer.enabled = true;
            // this.eventData = eventData;

            OnPointerEnter(eventData, eventData.pointerEnter, rayInteractor);
        }

        protected virtual void OnPointerEnter(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            StartCoroutine(OnPointerEnterCoroutine(eventData, eventData.pointerEnter, rayInteractor));
        }

        private IEnumerator OnPointerEnterCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            if (enableHaptics)
            {
                rayInteractor?.SendHapticImpulse(hapticsAmplitude, hapticsDuration);
            }

            yield return null;
        }

        private void OnPointerDown(GameObject gameObject, PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            isPointerDown = true;

            OnPointerDown(eventData, eventData.pointerEnter, rayInteractor);
        }

        protected virtual void OnPointerDown(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) { }

        private void OnPointerUp(GameObject gameObject, PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            isPointerDown = false;

            OnPointerUp(eventData, eventData.pointerEnter, rayInteractor);
        }

        protected virtual void OnPointerUp(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) { }

        private void OnPointerExit(GameObject gameObject, PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            isPointerEnter = false;
            pointerRenderer.enabled = false;
            // this.eventData = null;

            OnPointerExit(eventData, eventData.pointerEnter, rayInteractor);
        }

        protected virtual void OnPointerExit(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            StartCoroutine(OnPointerExitCoroutine(eventData, eventData.pointerEnter, rayInteractor));
        }

        private IEnumerator OnPointerExitCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            yield return null;
        }

        private void Callback_OnPointPerformed(InputAction.CallbackContext context)
        {
            Vector2 position = context.ReadValue<Vector2>();
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(position);
            Debug.Log($"Callback_OnPointPerformed Position : {position} World Point : {worldPoint}");

            if (!isPointerEnter) return;

            Ray ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                pointer.transform.position = hit.point;
            }
        }
    }
}