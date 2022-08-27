using System;
using System.Collections;

using UnityEngine;

namespace FX
{
    public class ScaleFX : AsyncTrigger
    {
        [SerializeField] float timeline = 1f;
        public float Timeline { get { return timeline; } }

        [Serializable]
        public class Config
        {
            public GameObject target;
            public Vector3 fromScale;
            public Vector3 toScale;
        }

        private Config config;

        protected override IEnumerator Co_Routine(object obj)
        {
            config = (Config) obj;

            float startTime = Time.time;
            float fractionComplete = 0f;
            Vector3 originalScale = transform.localScale;
            
            while (fractionComplete < 1f)
            {
                fractionComplete =  Mathf.Clamp((Time.time - startTime) / timeline, 0f, 1f);
                
                transform.localScale = new Vector3
                {
                    x = Mathf.Lerp(originalScale.x, config.toScale.x, (float) fractionComplete),
                    y = Mathf.Lerp(originalScale.y, config.toScale.y, (float) fractionComplete),
                    z = Mathf.Lerp(originalScale.z, config.toScale.z, (float) fractionComplete)
                };

                yield return null;
            }
        }
    }
}