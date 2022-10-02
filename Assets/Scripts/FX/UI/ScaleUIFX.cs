using System.Collections;

using UnityEngine;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX")]
    [RequireComponent(typeof(RectTransform))]
    public class ScaleUIFX : AsyncTrigger
    {
        public enum ScaleType
        {
            Proportional,
            XConstrained,
            XEqual,
            YConstrained,
            YEqual,
            Custom
        }

        public class Config
        {
            public float timeline;
            public Vector2 fromSize;
            public Vector2 toSize;
            public Vector2 startSize;
            public Vector2 endSize;
            public bool scaleContent;
        }

        public Vector2 TweenSize { get { return rectTransform.sizeDelta; } }
        public Vector2 TweenScale { get { return rectTransform.localScale; } }

        private RectTransform rectTransform;
        private Config config;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => rectTransform = GetComponent<RectTransform>() as RectTransform;

        protected override IEnumerator Co_Routine(object obj)
        {
            config = (Config) obj;

            Debug.Log($"{gameObject.name} Co_Routine Timelime : {config.timeline} FromSize : {config.fromSize.ToPrecisionString()} ToSize : {config.toSize.ToPrecisionString()} StartSize : {config.startSize.ToPrecisionString()} EndSize : {config.endSize.ToPrecisionString()} Scale Content : {config.scaleContent}");

            float scaleDelta = Vector2.Distance(config.startSize, config.endSize);

            if (scaleDelta == 0)
            {
                yield break;
            }

            // if (config.scaleContent)
            // {
            //     // TODO
            // }

            float startTime = Time.time;
            float range = Vector2.Distance(config.fromSize, config.toSize);
            float interRange = scaleDelta;
            float fractionalTimeline = config.timeline * (interRange / range);
            float fractionComplete = 0f;

            while (fractionComplete != 1f)
            {
                float elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / fractionalTimeline);
                rectTransform.sizeDelta = Vector2.Lerp(config.startSize, config.endSize, fractionComplete);
                yield return null;
            }

            yield return null;
        }
    }
}