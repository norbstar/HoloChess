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
        public string ShowWorkings(Config config)
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

        protected override IEnumerator Co_Routine(int id, object obj)
        {
            config = (Config) obj;

            // Debug.Log($"Co_Routine Start Id : {id} Co_Routine Speed : {config.speed} Size : {config.size.ToPrecisionString()} Scale : {config.scale.ToPrecisionString()} Flags : {config.flags}");

            float startTime = Time.time;
            
            Vector2 startSize = rectTransform.sizeDelta;
            Vector2 endSize = config.size;
            float sizeDelta = Vector2.Distance(startSize, endSize);
            Debug.Log($"Co_Routine StartSize : {startSize.ToPrecisionString()} EndSize : {endSize.ToPrecisionString()} SizeDelta : {sizeDelta}");
            
            Vector2 startScale = transform.localScale;
            Vector2 endScale = config.scale;
            float scaleDelta = Vector2.Distance(startScale, endScale);
            Debug.Log($"Co_Routine StartScale : {startScale.ToPrecisionString()} EndScale : {endScale.ToPrecisionString()} ScaleDelta : {scaleDelta}");

#if false
            float distance = Vector2.Distance(startSize, endSize);
            
            if (distance == 0)
            {
                distance = Vector2.Distance(startScale, endScale);
            }

            if (distance == 0)
            {
                // Debug.Log($"Co_Routine End Id : {id}"); 
                MarkEndCoroutine();
                yield return null;
                yield break;
            }
#endif

            if ((sizeDelta == 0) && (scaleDelta == 0))
            {
                MarkEndCoroutine();
                yield return null;
                yield break;
            }

            float unitSpeed = 1f / config.speed;
            // float travelTime = distance / unitSpeed;
            // Debug.Log($"Distance : {distance} UnitSpeed : {unitSpeed} TravelTime : {travelTime}");
            float travelTime = unitSpeed;
            Debug.Log($"UnitSpeed : {unitSpeed} TravelTime : {travelTime}");
            float fractionComplete = 0f;

            // Debug.Log($"[1] FC : {fractionComplete} Size : {rectTransform.sizeDelta} Scale : {transform.localScale}");

            while (fractionComplete != 1f)
            {
                float elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / travelTime);
                rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, fractionComplete);
                transform.localScale = Vector2.Lerp(startScale, endScale, fractionComplete);
                // Debug.Log($"[2] FC : {fractionComplete} Size : {rectTransform.sizeDelta} Scale : {transform.localScale}");
                yield return null;
            }

            // Debug.Log($"[3] FC : {fractionComplete} Size : {rectTransform.sizeDelta} Scale : {transform.localScale}");

            // Debug.Log($"Co_Routine End Id : {id}"); 
            MarkEndCoroutine();
        }
    }
}