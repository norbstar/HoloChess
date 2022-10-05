using System.Collections;

using UnityEngine;

using Flags = FX.UI.ScaleUIFXComponent.Flags;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX")]
    [RequireComponent(typeof(RectTransform))]
    public class ScaleUIFX : AsyncTrigger
    {
        public enum ScaleType
        {
            Unified,
            XConstrained,
            XEqual,
            YConstrained,
            YEqual,
            Custom
        }

        public class Config
        {
            public Vector2 size;
            public Vector2 scale;
            public float speed;
            public Flags flags;
        }

        public Vector2 OriginalSize { get { return originalSize; } }
        public Vector2 OriginalScale { get { return originalScale; } }

        private RectTransform rectTransform;
        private Vector2 originalSize, originalScale;
        private Config config;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies()
        {
            rectTransform = GetComponent<RectTransform>() as RectTransform;
            originalSize = rectTransform.rect.size;
            originalScale = transform.localScale;
        }

        protected override IEnumerator Co_Routine(int id, object obj)
        {
            config = (Config) obj;

            float startTime = Time.time;
            
            Vector2 startSize = rectTransform.sizeDelta;
            Vector2 endSize = config.size;
            float sizeDelta = Vector2.Distance(startSize, endSize);
            
            Vector2 startScale = transform.localScale;
            Vector2 endScale = config.scale;
            float scaleDelta = Vector2.Distance(startScale, endScale);

            if ((sizeDelta == 0) && (scaleDelta == 0))
            {
                MarkEndCoroutine();
                yield return null;
                yield break;
            }

            float unitSpeed = 1f / config.speed;
            float travelTime = unitSpeed;
            Debug.Log($"UnitSpeed : {unitSpeed} TravelTime : {travelTime}");
            float fractionComplete = 0f;

            while (fractionComplete != 1f)
            {
                float elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / travelTime);
                rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, fractionComplete);
                transform.localScale = Vector2.Lerp(startScale, endScale, fractionComplete);
                yield return null;
            }

            MarkEndCoroutine();
        }
    }
}