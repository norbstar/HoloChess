using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

using UnityButton = UnityEngine.UI.Button;

namespace UI
{
    [RequireComponent(typeof(UnityButton))]
    [RequireComponent(typeof(PointerEventHandler))]
    public class ButtonUIManager : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] protected AudioClip onHoverClip;
        [SerializeField] protected AudioClip onSelectClip;
        
        [Header("Config")]
        [SerializeField] bool enableHaptics = false;
        public bool EnableHaptics { get { return enableHaptics; } }
        [SerializeField] protected bool deselectOnSelect = true;
        [SerializeField] protected float deselectionDelay = 0.25f;
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

        public delegate void OnButtonEvent(GameObject gameObject, Event evt);
        public event OnButtonEvent EventReceived;

        private UnityButton button;
        private Coroutine postAnnotationCoroutine;
        private Vector3 originalScale;

        public virtual void Awake() => ResolveDependencies();

        private void ResolveDependencies() => button = GetComponent<UnityButton>() as UnityButton;

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

        private void OnPointerEvent(GameObject gameObject, PointerEventHandler.Event evt, PointerEventData eventData)
        {
            switch (evt)
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
            var manager = gameObject.GetComponent<ButtonUI>() as ButtonUI;

            if (manager.Header != null)
            {
                manager.HeaderColor = manager.HeaderHighlightColor;
            }

            if (enableHaptics)
            {
                rayInteractor?.SendHapticImpulse(0.25f, 0.1f);
            }

            var scaleFXManager = manager.ScaleFXManager;
            scaleFXManager.ScaleUp(originalScale, originalScale* 1.1f);
            
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
            var manager = gameObject.GetComponent<ButtonUI>() as ButtonUI;

            if (manager.Header != null)
            {
                manager.HeaderColor = manager.DefaultHeaderColor;
            }

            var scaleFXManager = manager.ScaleFXManager;
            scaleFXManager.ScaleDown(originalScale * 1.1f, originalScale);

            yield return null;
        }

        private IEnumerator PostAnnotationCoroutine(GameObject gameObject)
        {
            yield return new WaitForSeconds(postAnnotationDelay);

            var manager = gameObject.GetComponent<ButtonUI>() as ButtonUI;

            if (manager.Annotation != null)
            {
                NotifyReceivers(manager.Annotation.Text);
            }
        }

        protected void PostEvent(Event evt) => EventReceived?.Invoke(gameObject, evt);

        protected void NotifyReceivers(string text)
        {
            foreach (TextReceiver receiver in textReceivers)
            {
                receiver.OnText(text);
            }
        }

        public virtual void OnClickButton(UnityButton button)
        {
            if (onSelectClip != null)
            {
                AudioSource.PlayClipAtPoint(onSelectClip, Vector3.zero, 1.0f);
            }

            NotifyReceivers(string.Empty);
            PostEvent(Event.OnClick);

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