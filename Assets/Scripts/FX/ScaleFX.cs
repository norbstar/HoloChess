using System;
using System.Collections;

using UnityEngine;

namespace FX
{
    public class ScaleFX : AsyncTrigger
    {
        [SerializeField] float timespan = 1.0f;

        [Serializable]
        public class Config
        {
            public float fromScale = 1.0f;
            public float toScale = 1.0f;
        }

        protected Vector3 originalScale;

        private Config config;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => originalScale = transform.localScale;

        protected override IEnumerator Co_Routine(object obj)
        {
            config = (Config) obj;

            var tmp = Mathf.Abs(config.fromScale - config.toScale);
            var step = tmp / timespan;
            var journey = Mathf.Abs(originalScale.x - config.toScale);

            float startTransformTime = Time.time;
            bool complete = false;

            if (journey > 0f)
            {
                var adjustedDuration = step * journey;

                while (!complete)
                {
                    float fractionComplete =  Mathf.Clamp((Time.time - startTransformTime) / adjustedDuration, 0f, 1f);
                    SetScale(Mathf.Lerp(originalScale.x, config.toScale, (float) fractionComplete));

                    if (fractionComplete >= 1f)
                    {
                        complete = true;
                    }

                    yield return null;
                }
            }
        }

        // private void SetScale(float scale) => transform.localScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z * scale);
        private void SetScale(float scale) => transform.localScale = new Vector3(scale, scale, scale);
    }
}