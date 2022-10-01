using System.Collections;

using UnityEngine;

namespace UI
{
    [AddComponentMenu("UI/Canvas Group Fader")]
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] float timeline = 1f;
        public float Timeline { get { return timeline; } }

        private static float MinValue = 0f;
        private static float MaxValue = 1f;

        private CanvasGroup canvasGroup;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;

        public void FadeIn() => StartCoroutine(Co_Fade(MaxValue, MinValue));

        public void FadeOut() => StartCoroutine(Co_Fade(MinValue, MaxValue));

        private IEnumerator Co_Fade(float fromAlpha, float toAlpha)
        {      
            float startTime = Time.time;
            float speed = Mathf.Abs(MaxValue - MinValue) / timeline;
            float elapsedTime = 0f;
            float fractionComplete = 0f;

            while (fractionComplete < 1f)
            {
                elapsedTime += Time.deltaTime * speed;
                fractionComplete = elapsedTime / Mathf.Abs(MaxValue - MaxValue);

                if (fractionComplete <= 1f)
                {
                    canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, fractionComplete);
                }
                
                yield return null;
            }

            canvasGroup.alpha = toAlpha;
        }
    }
}