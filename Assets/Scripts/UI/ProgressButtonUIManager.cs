using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

using UnityButton = UnityEngine.UI.Button;

namespace UI
{
    [AddComponentMenu("UI/Progress Button UI Manager")]
    [RequireComponent(typeof(ColorGradient))]
    public class ProgressButtonUIManager : ButtonUIManager
    {
        [Header("Progress Components")]
        [SerializeField] Image progress;

        [Header("Progress Config")]
        [SerializeField] float progressTimeline = 1f;
        public float ProgressTimeline { get { return progressTimeline; } }

        private Coroutine coroutine;
        private ColorGradient colorGradient;
        private float fractionComplete;

        public override void Awake()
        {
            base.Awake();

            ResolveDependencies();
            progress.fillAmount = 0f;
        }

        private void ResolveDependencies() => colorGradient = GetComponent<ColorGradient>() as ColorGradient;

        protected override void OnPointerDown(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            base.OnPointerDown(eventData, gameObject, rayInteractor);
            coroutine = StartCoroutine(ManageProgressCoroutine());
        }

        protected override void OnPointerUp(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            base.OnPointerUp(eventData, gameObject, rayInteractor);
            Cancel();
        }

        protected override void OnPointerExit(PointerEventData eventData, GameObject gameObject, XRRayInteractor rayInteractor)
        {
            base.OnPointerExit(eventData, gameObject, rayInteractor);
            Cancel();
        }

        private void Cancel()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            if (deselectOnSelect)
            {
                StartCoroutine(DeselectCoroutine(deselectionDelay));
            }

            progress.fillAmount = 0f;
        }

        public override void OnClickButton(UnityButton button) { }

        private IEnumerator ManageProgressCoroutine()
        {
            bool complete = false;           
            float startTime = Time.time;            
            float endTime = startTime + progressTimeline;

            progress.fillAmount = 0f;

            while (!complete)
            {
                fractionComplete = (Time.time - startTime) / progressTimeline;
                complete = (fractionComplete >= 1f);
                
                progress.fillAmount = fractionComplete;
                progress.color = colorGradient.Value(fractionComplete);

                yield return null;
            }

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
    }
}