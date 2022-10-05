using System;
using System.Collections;

using UnityEngine;

namespace FX.UI
{
    [AddComponentMenu("FX/Rect Transform FX")]
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformFX : AsyncTrigger
    {
        [Header("Config")]
        [SerializeField] float timeline = 1f;
        public float Timeline { get { return timeline; } }
        [SerializeField] bool synchronize = true;
        public bool Synchronize { get { return synchronize; } }

        [Serializable]
        public class Config
        {
            public Vector2 fromSize;
            public Vector2 toSize;
            public Vector2 startSize;
            public Vector2 endSize;
        }

        private RectTransform rectTransform;
        private Vector2 originalSize;
        public Vector2 OriginalSize { get { return originalSize; } }
        public Vector2 TweenSize { get { return rectTransform.sizeDelta; } }
        private Config config;

        void Awake()
        {
            ResolveDependencies();
            originalSize = rectTransform.sizeDelta;
        }

        private void ResolveDependencies() => rectTransform = GetComponent<RectTransform>() as RectTransform;

        protected override IEnumerator Co_Routine(int id, object obj)
        {
            config = (Config) obj;
            
            // Debug.Log($"{gameObject.name} Co_Routine Id : {id} FromSize : {config.fromSize.ToPrecisionString()} ToSize : {config.toSize.ToPrecisionString()} StartSize : {config.startSize.ToPrecisionString()} EndSize : {config.endSize.ToPrecisionString()}");

            float scaleDelta = Vector2.Distance(config.startSize, config.endSize);

            if (scaleDelta == 0)
            {
                yield break;
            }

            float startTime = Time.time;
            float range = Vector2.Distance(config.fromSize, config.toSize);
            float interRange = scaleDelta;
            float fractionalTimeline = timeline * (interRange / range);
            float fractionComplete = 0f;

            while (fractionComplete != 1f)
            {
                float elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / fractionalTimeline);
                rectTransform.sizeDelta = Vector2.Lerp(config.startSize, config.endSize, fractionComplete);
                yield return null;
            }

            // Debug.Log($"{gameObject.name} Co_Routine End Id : {id}"); 
            DecrementCount();
        }
    }
}