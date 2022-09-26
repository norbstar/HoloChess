using System;
using System.Collections;

using UnityEngine;

namespace FX
{
    public class ScaleFX : AsyncTrigger
    {
        [Header("Config")]
        [SerializeField] float timeline = 1f;
        public float Timeline { get { return timeline; } }
        [SerializeField] bool synchronize = true;
        public bool Synchronize { get { return synchronize; } }

        [Serializable]
        public class Config
        {
            public Vector3 fromScale;
            public Vector3 toScale;
        }

        private Config config;

        protected override IEnumerator Co_Routine(object obj)
        {
            config = (Config) obj;
            
            float startTime = Time.time;
            float speed = Vector3.Distance(config.fromScale, config.toScale) / timeline;
            float elapsedTime = 0f;
            float fractionComplete = 0f;

            while (fractionComplete < 1f)
            {
                elapsedTime += Time.deltaTime * speed;
                fractionComplete = elapsedTime / Vector3.Distance(config.fromScale, config.toScale);

                if (fractionComplete <= 1f)
                {
                    transform.localScale = Vector3.Lerp(config.fromScale, config.toScale, fractionComplete);
                }

                yield return null;
            }

            transform.localScale = config.toScale;
        }
    }
}