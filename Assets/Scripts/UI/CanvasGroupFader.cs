using System.Collections;

using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] float durationSec = 0.5f;

        public delegate void Event(CanvasGroupFader manager, float fractionComplete);
        public event Event EventReceived;

        // public enum Direction
        // {
        //     ToMax,
        //     ToMin
        // }

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

            coroutine = StartCoroutine(Co_FadeIn());
            // coroutine = StartCoroutine(Co_Fade(Direction.ToMax));
        }

        // private IEnumerator Co_Fade(Direction direction)
        // {
        //     float startTime = Time.time;
        //     float startAlpha = canvasGroup.alpha;
        //     float fractionComplete = 0f;

        //     while (fractionComplete < 1f)
        //     {
        //         switch(direction)
        //         {
        //             case Direction.ToMax:
        //                 fractionComplete = ((Time.time - startTime) / durationSec) + startAlpha;
        //                 canvasGroup.alpha = Mathf.Lerp(0f, 1f, fractionComplete);
        //                 break;

        //             case Direction.ToMin:
        //                 fractionComplete = ((Time.time - startTime) / durationSec) + (1f - startAlpha);
        //                 canvasGroup.alpha = Mathf.Lerp(1f, 0f, fractionComplete);
        //                 break;
        //         }
                
        //         EventReceived?.Invoke(this, fractionComplete);
        //         yield return null;
        //     }
        // }

        private IEnumerator Co_FadeIn()
        {
            float startTime = Time.time;
            float fractionComplete = 0f;

            while (fractionComplete < 1f)
            {
                fractionComplete = ((Time.time - startTime) / durationSec) + canvasGroup.alpha;               
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, fractionComplete);
                EventReceived?.Invoke(this, fractionComplete);
                yield return null;
            }
        }

        public void FadeOut()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(Go_FadeOut());
            // coroutine = StartCoroutine(Co_Fade(Direction.ToMin));
        }

        private IEnumerator Go_FadeOut()
        {
            float startTime = Time.time;
            float fractionComplete = 0f;

            while (fractionComplete < 1f)
            {
                fractionComplete = ((Time.time - startTime) / durationSec) + (1f - canvasGroup.alpha);                
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, fractionComplete);
                EventReceived?.Invoke(this, fractionComplete);
                yield return null;
            }
        }
    }
}