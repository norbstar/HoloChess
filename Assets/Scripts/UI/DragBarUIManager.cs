using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

using UI.Panels;
using FX;

namespace UI
{
    [RequireComponent(typeof(PointerEventHandler))]
    [RequireComponent(typeof(RootResolver))]
    [RequireComponent(typeof(ScaleFXManager))]
    public class DragBarUIManager : MonoBehaviour
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

        [Header("Color")]
        [SerializeField] Color hoverColor = Color.white;
        [SerializeField] Color selectColor = Color.white;

        public enum Event
        {
            OnPointerEnter,
            OnPointerDown,
            OnPointerUp,
            OnPointerExit,
        }

        public delegate void OnDragBarEvent(DragBarUIManager manager, Event @event, GameObject source);
        public event OnDragBarEvent EventReceived;

        private PointerEventHandler eventHandler;
        private RootResolver rootResolver;
        private ScaleFXManager scaleFXManager;
        private GameObject root;
        private bool isPointerEntered, isPointerDown;
        public bool IsPointerDown { get { return isPointerDown; } }
        private GameObject interactor;
        private Image image;
        private Vector3 originalScale;
        private Color originalColor;

        void Awake()
        {
            ResolveDependencies();

            originalScale = transform.localScale;
            originalColor = image.color;
            root = rootResolver.Root;
        }

        private void ResolveDependencies()
        {
            image = GetComponent<Image>() as Image;
            eventHandler = GetComponent<PointerEventHandler>() as PointerEventHandler;
            rootResolver = GetComponent<RootResolver>() as RootResolver;
            scaleFXManager = GetComponent<ScaleFXManager>() as ScaleFXManager;
        }

        void OnEnable() => eventHandler.EventReceived += OnPointerEvent;

        void OnDisable() => eventHandler.EventReceived -= OnPointerEvent;

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

            image.color = hoverColor;
            isPointerEntered = true;

            OnPointerEnter(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerEnter, interactor.gameObject ?? null);
        }

        protected virtual void OnPointerEnter(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) => StartCoroutine(OnPointerEnterCoroutine(eventData, eventData.pointerEnter, rayInteractor));

        private IEnumerator OnPointerEnterCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            if (!isPointerDown)
            {
                if (enableHaptics)
                {
                    rayInteractor?.SendHapticImpulse(hapticsAmplitude, hapticsDuration);
                }

                scaleFXManager.ScaleUp(originalScale, originalScale* 1.1f);

                if (onHoverClip != null)
                {
                    AudioSource.PlayClipAtPoint(onHoverClip, Vector3.zero, 1.0f);
                }
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

            if (rayInteractor != null)
            {
                this.interactor = rayInteractor.gameObject;
            }

            image.color = selectColor;
            
            if (root.TryGetComponent<HomePanelUIManager>(out HomePanelUIManager homeManager))
            {
                homeManager.ButtonGroupManager.Disable();
            }

            if (root.TryGetComponent<TerminalPanelUIManager>(out TerminalPanelUIManager terminalManager))
            {
                terminalManager.ButtonGroupManager.Disable();
            }

            isPointerDown = true;

            OnPointerDown(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerDown, interactor.gameObject ?? null);
        }

        protected virtual void OnPointerDown(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) { }
        
        private void OnPointerUp(GameObject gameObject, PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            image.color = hoverColor;
            
            if (root.TryGetComponent<HomePanelUIManager>(out HomePanelUIManager homeManager))
            {
                homeManager.ButtonGroupManager.Enable();
            }

            if (root.TryGetComponent<TerminalPanelUIManager>(out TerminalPanelUIManager terminalManager))
            {
                terminalManager.ButtonGroupManager.Enable();
            }

            this.interactor = null;
            isPointerDown = false;

            OnPointerUp(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerUp, interactor.gameObject ?? null);
        }

        protected virtual void OnPointerUp(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) { }

        private void OnPointerExit(GameObject gameObject, PointerEventData eventData)
        {
            if (isPointerDown) return;

            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            image.color = originalColor;
            isPointerEntered = false;

            OnPointerExit(eventData, eventData.pointerEnter, rayInteractor);
            PostEvent(Event.OnPointerExit, interactor.gameObject ?? null);
        }

        protected virtual void OnPointerExit(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor) => StartCoroutine(OnPointerExitCoroutine(eventData, eventData.pointerEnter, rayInteractor));

        private IEnumerator OnPointerExitCoroutine(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            scaleFXManager.ScaleDown(originalScale * 1.1f, originalScale);
            yield return null;
        }

        protected void PostEvent(Event @event, GameObject source) => EventReceived?.Invoke(this, @event, source);
    }
}