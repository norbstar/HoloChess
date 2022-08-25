using System.Collections;

using UnityEngine;

using NaughtyAttributes;

namespace Mutator
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupAlphaMutator : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject target;
        public GameObject Target { get { return target; } }
        [Label("Timespan (Seconds)")]
        [SerializeField] float timespan = 1f;
        public float Timespan { get { return timespan; } }
        [Label("Speed (XPS)")]
        [SerializeField] float speed = 1f;
        public float Speed { get { return speed; } }
        [SerializeField] bool useSpeed = false;

        public delegate void Event(CanvasGroupAlphaMutator mutator, float fractionComplete);
        public event Event EventReceived;

        public static float MinValue = 0f;
        public static float MaxValue = 1f;

        public float Alpha { get { return canvasGroup.alpha; } }
        private CanvasGroup canvasGroup;
        private Coroutine coroutine;

        void Awake()
        {
            ResolveDependencies();

            if (!useSpeed)
            {
                speed = MaxValue / timespan;
            }
        }

        private void ResolveDependencies() => canvasGroup = target.GetComponent<CanvasGroup>() as CanvasGroup;

        public void SyncTo(float fractionComplete) => canvasGroup.alpha = Mathf.Lerp(0f, 1f, fractionComplete);

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
            float fractionComplete = 0f;
            float alpha = canvasGroup.alpha;

            while (fractionComplete < 1f)
            {
                alpha += Time.deltaTime * speed;
                fractionComplete = alpha / MaxValue;
                canvasGroup.alpha = Mathf.Lerp(MinValue, MaxValue, fractionComplete);
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
            float fractionComplete = 0f;
            float alpha = canvasGroup.alpha;

            while (fractionComplete < 1f)
            {
                alpha -= Time.deltaTime * speed;
                fractionComplete = alpha / MaxValue;
                canvasGroup.alpha = Mathf.Lerp(MinValue, MaxValue, fractionComplete);
                EventReceived?.Invoke(this, fractionComplete);
                yield return null;
            }
        }
    }
}