using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UI
{
    public abstract class BaseButtonGroupPanelUIManager : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] protected AudioClip onHoverClip;
        [SerializeField] protected AudioClip onSelectClip;
        
        [Header("Config")]
        [SerializeField] bool enableHaptics = false;
        public bool EnableHaptics { get { return enableHaptics; } }
        [SerializeField] bool deselectOnSelect = true;
        [SerializeField] float deselectionDelay = 0.25f;
        public float DeselectionDelay { get { return deselectionDelay; } }
        [SerializeField] float postAnnotationDelay = 0.5f;
        public float PostAnnotationDelay { get { return postAnnotationDelay; } }

        [Header("Notifications")]
        [SerializeField] List<TextReceiver> textReceivers;

        public enum Event
        {
            OnPointerEnter,
            OnPointerDown,
            OnPointerUp,
            OnPointerExit,
            OnClick
        }

        public delegate void OnButtonEvent(GameObject gameObject, Event @event);
        public event OnButtonEvent EventReceived;

        public class ButtonAccessor
        {
            public ButtonUIManager manager;
            public Vector3 originalScale;

            public ButtonAccessor(ButtonUIManager manager)
            {
                this.manager = manager;
                originalScale = manager.Button.transform.localScale;
            }
        }

        protected List<ButtonAccessor> accessors;
        private Coroutine postAnnotationCoroutine;

        void Awake()
        {
            accessors = ResolveAccessors();

            foreach (ButtonAccessor container in accessors)
            {
                container.manager.Button.onClick.AddListener(delegate {
                    OnClickButton(container);
                });
            }
        }

        protected abstract List<ButtonAccessor> ResolveAccessors();

        void OnEnable()
        {
            foreach (ButtonAccessor container in accessors)
            {
                var eventHandler = container.manager.Button.GetComponent<PointerEventHandler>() as PointerEventHandler;
                eventHandler.EventReceived += OnPointerEvent;
            }
        }

        void OnDisable()
        {
            foreach (ButtonAccessor container in accessors)
            {
                var eventHandler = container.manager.Button.GetComponent<PointerEventHandler>() as PointerEventHandler;
                eventHandler.EventReceived -= OnPointerEvent;
            }
        }

        public void ResetButtons()
        {
            foreach (ButtonAccessor container in accessors)
            {
                var transform = container.manager.Button.transform;
                transform.localScale = container.originalScale;

                var manager = transform.gameObject.GetComponent<ButtonUI>() as ButtonUI;

                if (manager.Header != null)
                {
                    manager.HeaderColor = manager.DefaultHeaderColor;
                }
            }
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

        protected virtual void OnPointerEnter(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            StartCoroutine(OnPointerEnterCoroutine(eventData, eventData.pointerEnter, rayInteractor));
            postAnnotationCoroutine = StartCoroutine(PostAnnotationCoroutine(eventData.pointerEnter));
        }

        private IEnumerator OnPointerEnterCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            var buttonUI = gameObject.GetComponent<ButtonUI>() as ButtonUI;

            if (buttonUI.Header != null)
            {
                buttonUI.HeaderColor = buttonUI.HeaderHighlightColor;
            }

            if (enableHaptics)
            {
                rayInteractor?.SendHapticImpulse(0.25f, 0.1f);
            }

            var container = accessors.First(bc => GameObject.ReferenceEquals(bc.manager.Button.gameObject, gameObject));

            var scaleFXManager = buttonUI.ScaleFXManager;
            scaleFXManager.ScaleUp(container.originalScale, container.originalScale* 1.1f);
            
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

            OnPointerDown(eventData, eventData.pointerEnter, rayInteractor);
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

        protected virtual void OnPointerExit(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            StartCoroutine(OnPointerExitCoroutine(eventData, eventData.pointerEnter, rayInteractor));
                
            if (postAnnotationCoroutine != null)
            {
                StopCoroutine(postAnnotationCoroutine);
            }

            NotifyReceivers(string.Empty);
        }

        private IEnumerator OnPointerExitCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            var buttonUI = gameObject.GetComponent<ButtonUI>() as ButtonUI;

            if (buttonUI.Header != null)
            {
                buttonUI.HeaderColor = buttonUI.DefaultHeaderColor;
            }

            var container = accessors.First(bc => GameObject.ReferenceEquals(bc.manager.Button.gameObject, gameObject));

            var scaleFXManager = buttonUI.ScaleFXManager;
            scaleFXManager.ScaleDown(container.originalScale * 1.1f, container.originalScale);

            yield return null;
        }

        private IEnumerator PostAnnotationCoroutine(GameObject gameObject)
        {
            yield return new WaitForSeconds(postAnnotationDelay);

            var manager = gameObject.GetComponent<ButtonUI>() as ButtonUI;
            NotifyReceivers(manager.Annotation.Text);
        }

        protected void PostEvent(Event @event) => EventReceived?.Invoke(gameObject, @event);

        protected void NotifyReceivers(string text)
        {
            foreach (TextReceiver receiver in textReceivers)
            {
                receiver.OnText(text);
            }
        }

        public virtual void OnClickButton(ButtonAccessor container)
        {
            if (onSelectClip != null)
            {
                AudioSource.PlayClipAtPoint(onSelectClip, Vector3.zero, 1.0f);
            }

            if (deselectOnSelect)
            {
                StartCoroutine(DeselectCoroutine(deselectionDelay));
            }

            NotifyReceivers(string.Empty);
        }

        private IEnumerator DeselectCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}