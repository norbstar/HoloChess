using System;
using System.Collections;

using UnityEngine;

namespace FX
{
    public class ScaleFX : AsyncTrigger
    {
        [Header("Custom Config")]
        [SerializeField] float timeline = 1f;
        public float Timeline { get { return timeline; } }
        [SerializeField] bool synchronize = true;
        public bool Synchronize { get { return synchronize; } }

        [Serializable]
        public class Config
        {
            public Vector3 fromScale;
            public Vector3 startScale;
            public Vector3 toScale;
            public Vector3 endScale;
        }

        private Config config;

        protected override IEnumerator Co_Routine(object obj)
        {
            config = (Config) obj;
            
            float scaleDelta = Vector3.Distance(config.startScale, config.endScale);

            if (scaleDelta == 0)
            {
                yield break;
            }

            float startTime = Time.time;
            float range = Vector3.Distance(config.fromScale, config.toScale);
            float interRange = scaleDelta;
            float fractionalTimeline = timeline * (interRange / range);
            float fractionComplete = 0f;

            while (fractionComplete != 1f)
            {
                float elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / fractionalTimeline);
                transform.localScale = Vector3.Lerp(config.startScale, config.endScale, fractionComplete);
                yield return null;
            }
        }
    }
}