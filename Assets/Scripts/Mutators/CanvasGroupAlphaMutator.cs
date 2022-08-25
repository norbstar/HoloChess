using System.Collections;

using UnityEngine;

namespace Mutator
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupAlphaMutator : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject target;
        public GameObject Target { get { return target; } }
        [SerializeField] float timespanSec = 1f;
        public float Timespan { get { return timespanSec; } }

        public delegate void Event(CanvasGroupAlphaMutator mutator, float fractionComplete);
        public event Event EventReceived;

        public static float MinValue = 0f;
        public static float MaxValue = 1f;

        public float Alpha { get { return canvasGroup.alpha; } }
        private CanvasGroup canvasGroup;
        private Coroutine coroutine;
        private float speed;

        void Awake()
        {
            ResolveDependencies();
            speed = MaxValue / timespanSec;
            // StartCoroutine(Co_Update());
        }

        private void ResolveDependencies() => canvasGroup = target.GetComponent<CanvasGroup>() as CanvasGroup;

        public void SetFractionComplete(float fractionComplete) => canvasGroup.alpha = Mathf.Lerp(0f, 1f, fractionComplete);

        // public void SetFadeInFraction(float fractionComplete) => canvasGroup.alpha = Mathf.Lerp(0f, 1f, fractionComplete);

        // public void SetFadeOutFraction(float fractionComplete) => canvasGroup.alpha = Mathf.Lerp(1f, 0f, fractionComplete);

        // private IEnumerator Co_Update()
        // {
        //     float startTime = Time.time;
        //     float value = 0f;

        //     while (true)
        //     {
        //         var fc = (Time.time - startTime) / timespanSec;
        //         Debug.Log($"ValueA {fc}");
        //         value += Time.deltaTime * speed;
        //         Debug.Log($"ValueB {value}");
        //         yield return null;
        //     }
        // }

        public void FadeIn()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(Co_FadeIn());
        }

        private IEnumerator Co_FadeIn()
        {
            float startTime = Time.time;
            float fractionComplete = 0f;
            float alpha = canvasGroup.alpha;

            while (fractionComplete < 1f)
            {
                alpha += Time.deltaTime * speed;

                // float ratio = canvasGroup.alpha / MaxValue;
                // float timeRemaining = ((Time.time - startTime) / (timespanSec * ratio));
                // fractionComplete = ((Time.time - startTime) / timespanSec) + canvasGroup.alpha;
                fractionComplete = alpha / MaxValue;
                canvasGroup.alpha = Mathf.Lerp(MinValue, MaxValue, fractionComplete);
                Debug.Log($"FadeIn Alpha : {canvasGroup.alpha}");
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

            coroutine = StartCoroutine(Co_FadeOut());
        }

        private IEnumerator Co_FadeOut()
        {
            float startTime = Time.time;
            float fractionComplete = 0f;
            float alpha = canvasGroup.alpha;

            while (fractionComplete < 1f)
            {
                alpha -= Time.deltaTime * speed;

                // fractionComplete = ((Time.time - startTime) / timespanSec) + (MaxValue - canvasGroup.alpha);
                // canvasGroup.alpha = Mathf.Lerp(MaxValue, MinValue, fractionComplete);
                fractionComplete = alpha / MaxValue;
                canvasGroup.alpha = Mathf.Lerp(MinValue, MaxValue, fractionComplete);
                Debug.Log($"FadeOut Alpha : {canvasGroup.alpha}");
                EventReceived?.Invoke(this, fractionComplete);
                yield return null;
            }
        }
    }
}