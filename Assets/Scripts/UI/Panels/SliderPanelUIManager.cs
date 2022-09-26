using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

using FX;

namespace UI.Panels
{
    public class SliderPanelUIManager : MonoBehaviour, IAudioComponent
    {
        [Header("Components")]
        [SerializeField] protected Slider slider;
        public float Value { get { return slider.value; } set { slider.value = value; } }
        [SerializeField] PointerEventHandler eventHandler;

        [Header("Audio")]
        [SerializeField] protected AudioClip onHoverClip;

        [Header("Config")]
        [SerializeField] bool enableHaptics = false;
        public bool EnableHaptics { get { return enableHaptics; } }

        public delegate void OnValueEvent(SliderPanelUIManager manager, float value);
        public event OnValueEvent EventReceived;

        private Vector3 originalScale;
        private bool isSynchronizing;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            originalScale = slider.handleRect.transform.localScale;
            
            slider.onValueChanged.AddListener(delegate {
                OnValueChanged(slider.value);
            });
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
                    OnPointerEnter(eventData);
                    break;

                case PointerEventHandler.Event.Exit:
                    OnPointerExit(eventData);
                    break;
            }
        }

        private void OnPointerEnter(PointerEventData eventData)
        {
            XRRayInteractor rayInteractor = null;
            
            if (TryGetXRRayInteractor(eventData.pointerId, out var interactor))
            {
                rayInteractor = interactor;
            }

            StartCoroutine(OnPointerEnterCoroutine(eventData, rayInteractor));
        }

        private IEnumerator OnPointerEnterCoroutine(PointerEventData eventData, XRRayInteractor rayInteractor)
        {
            var manager = slider.handleRect.gameObject.GetComponent<ScaleFX2DManager>() as ScaleFX2DManager;

            if (enableHaptics)
            {
                rayInteractor?.SendHapticImpulse(0.25f, 0.1f);
            }

            manager.ScaleFromTo(originalScale, new Vector3(originalScale.x * 1.1f, originalScale.y * 1.1f, originalScale.z));

            if (onHoverClip != null)
            {
                AudioSource.PlayClipAtPoint(onHoverClip, Vector3.zero, 1.0f);
            }

            yield return null;
        }

        private void OnPointerExit(PointerEventData eventData) => StartCoroutine(OnPointerExitCoroutine(eventData));

        private IEnumerator OnPointerExitCoroutine(PointerEventData eventData)
        {
            var manager = slider.handleRect.gameObject.GetComponent<ScaleFX2DManager>() as ScaleFX2DManager;
            manager.ScaleFromTo(new Vector3(originalScale.x * 1.1f, originalScale.y * 1.1f, originalScale.z), originalScale);

            yield return null;
        }

        protected virtual void OnValueChanged(float value)
        {
            if (!isSynchronizing)
            {
                EventReceived?.Invoke(this, slider.value);
            }
            
            isSynchronizing = false;
        }

        public void SyncVolume(float volume)
        {
            isSynchronizing = true;
            slider.value = volume;
        }
    }
}