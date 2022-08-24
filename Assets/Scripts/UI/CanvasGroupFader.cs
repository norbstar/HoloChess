using System.Collections;

using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] float durationSec = 0.5f;

        public float Alpha { get { return canvasGroup.alpha; } }

        private CanvasGroup canvasGroup;
        private Coroutine coroutine;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;

        public void FadeIn()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(FadeInCanvasGroupCoroutine());
        }

        private IEnumerator FadeInCanvasGroupCoroutine()
        {
            bool complete = false;           
            float startTime = Time.time;     
            float startAlpha = canvasGroup.alpha;

            while (!complete)
            {
                float fractionComplete = ((Time.time - startTime) / durationSec) + startAlpha;
                complete = (fractionComplete >= 1f);
                
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, fractionComplete);
                yield return null;
            }
        }

        public void FadeOut()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(FadeOutCanvasGroupCoroutine());
        }

        private IEnumerator FadeOutCanvasGroupCoroutine()
        {
            bool complete = false;           
            float startTime = Time.time;            
            float startAlpha = canvasGroup.alpha;
            
            while (!complete)
            {
                float fractionComplete = ((Time.time - startTime) / durationSec) + (1f - startAlpha);
                complete = (fractionComplete >= 1f);
                
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, 1f - fractionComplete);
                yield return null;
            }
        }
    }
}