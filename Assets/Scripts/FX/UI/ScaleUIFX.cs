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

#if false
        public string DebugWorkings(Config config)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            float startTime = Time.time;
            builder.Append($"StartTime : {startTime}");
            float distance = Vector2.Distance(rectTransform.rect.size, config.size);
            builder.Append($" Distance : {distance}");
            float unitSpeed = 1f / config.speed;
            builder.Append($" UnitSpeed : {unitSpeed}");
            float travelTime = distance / unitSpeed;
            builder.Append($" TravelTime : {travelTime}");
            Vector2 startSize = rectTransform.sizeDelta;
            builder.Append($" StartSize : {startSize}");
            Vector2 endSize = config.size;
            builder.Append($" EndSize : {endSize}");

            return builder.ToString();
        }
#endif

        protected override IEnumerator Co_Routine(object obj)
        {
            config = (Config) obj;

            Debug.Log($"{gameObject.name} Co_Routine Speed : {config.speed} EndSize : {config.size.ToPrecisionString()} Flags : {config.flags}");

            float startTime = Time.time;
            float distance = Vector2.Distance(rectTransform.rect.size, config.size);
            float unitSpeed = 1f / config.speed;
            float travelTime = distance / unitSpeed;
            Vector2 startSize = rectTransform.sizeDelta;
            float fractionComplete = 0f;

            while (fractionComplete != 1f)
            {
                float elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / travelTime);
                rectTransform.sizeDelta = Vector2.Lerp(startSize, config.size, fractionComplete);
                yield return null;
            }
        }
    }
}