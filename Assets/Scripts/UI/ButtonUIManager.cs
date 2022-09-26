using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

using UnityButton = UnityEngine.UI.Button;

using FX;

namespace UI
{
    [RequireComponent(typeof(UnityButton))]
    [RequireComponent(typeof(PointerEventHandler))]
    [RequireComponent(typeof(ScaleFX2DManager))]
    public class ButtonUIManager : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] protected AudioClip onHoverClip;
        [SerializeField] protected AudioClip onSelectClip;
        
        [Header("Haptics")]
        [SerializeField] bool enableHaptics = false;
        public bool EnableHaptics { get { return enableHaptics; } }
        [SerializeField] float hapticsAmplitude = 0.15f;
        public float HapticsAmplitude { get { return hapticsAmplitude; } }
        [SerializeField] float hapticsDuration = 0.1f;
        public float HapticsDuration { get { return hapticsDuration; } }
        
        [Header("Config")]
        [SerializeField] protected bool deselectOnSelect = true;
        [SerializeField] protected float deselectionDelay = 0.25f;
        public float DeselectionDelay { get { return deselectionDelay; } }
       
        public enum Event
        {
            OnPointerEnter,
            OnPointerDown,
            OnPointerUp,
            OnPointerExit,
            OnSelect
        }

        public delegate void OnButtonEvent(ButtonUIManager manager, Event @event);
        public event OnButtonEvent EventReceived;

        public UnityButton Button { get { return button; } }

        private UnityButton button;
        private ScaleFX2DManager scaleFXManager;
        private Vector3 originalScale;

        public virtual void Awake()
        {
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            button = GetComponent<UnityButton>() as UnityButton;
            scaleFXManager = GetComponent<ScaleFX2DManager>() as ScaleFX2DManager;
        }

        // Start is called before the first frame update
        void Start()
        {
            originalScale = transform.localScale;

            button.onClick.AddListener(delegate {
                OnClickButton(button);
            });
        }

        void OnEnable()
        {
            var eventHandler = button.GetComponent<PointerEventHandler>() as PointerEventHandler;
            eventHandler.EventReceived += OnPointerEvent;
        }

        void OnDisable()
        {
            var eventHandler = button.GetComponent<PointerEventHandler>() as PointerEventHandler;
            eventHandler.EventReceived -= OnPointerEvent;
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

            OnPointerEnter(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerEnter);
        }

        protected virtual void OnPointerEnter(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) => StartCoroutine(OnPointerEnterCoroutine(eventData, eventData.pointerEnter, rayInteractor));

        private IEnumerator OnPointerEnterCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            if (enableHaptics)
            {
                rayInteractor?.SendHapticImpulse(hapticsAmplitude, hapticsDuration);
            }
            
            scaleFXManager.ScaleTween(originalScale, new Vector3(originalScale.x * 1.1f, originalScale.y * 1.1f, originalScale.z));
            
            if (onHoverClip != null)
            {
                AudioSource.PlayClipAtPoint(onHoverClip, Vector3.zero, 1.0f);
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

            OnPointerDown(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerDown);
        }

        protected virtual void OnPointerDown(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) { }
        
        private void OnPointerUp(GameObject gameObject, PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            OnPointerUp(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerUp);
        }

        protected virtual void OnPointerUp(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) { }

        private void OnPointerExit(GameObject gameObject, PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            OnPointerExit(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerExit);
        }

        protected virtual void OnPointerExit(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) => StartCoroutine(OnPointerExitCoroutine(eventData, eventData.pointerEnter, rayInteractor));

        private IEnumerator OnPointerExitCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            scaleFXManager.ScaleTween(new Vector3(originalScale.x * 1.1f, originalScale.y * 1.1f, originalScale.z), originalScale);
            yield return null;
        }

        protected void PostEvent(Event @event) => EventReceived?.Invoke(this, @event);

        public virtual void OnClickButton(UnityButton button)
        {
            if (onSelectClip != null)
            {
                AudioSource.PlayClipAtPoint(onSelectClip, Vector3.zero, 1.0f);
            }

            PostEvent(Event.OnSelect);

            if (deselectOnSelect)
            {
                StartCoroutine(DeselectCoroutine(deselectionDelay));
            }
        }

        protected IEnumerator DeselectCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}