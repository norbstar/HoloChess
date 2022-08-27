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
            // public GameObject target;
            public Vector3 fromScale;
            public Vector3 toScale;
        }

        private Config config;

        protected override IEnumerator Co_Routine(object obj)
        {
            config = (Config) obj;

            float startTime = Time.time;
            Vector3 originalScale = transform.localScale;

#if false
            float fractionComplete = 0f;
            
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
#endif

#if false
            Vector3 speed = (synchronize) ? speed = SplitSpeed(config) : Vector3.one * Vector3.Distance(config.fromScale, config.toScale) / timeline;

            float x = 0f, y = 0f, z = 0f;
            Vector3 fractionComplete;
            bool complete = false;

            while (!complete)
            {
                x += Time.deltaTime * speed.x;
                y += Time.deltaTime * speed.y;
                z += Time.deltaTime * speed.z;

                fractionComplete = new Vector3
                {
                    x = x / config.toScale.x,
                    y = y / config.toScale.y,
                    z = z / config.toScale.z
                };

                complete = (fractionComplete.x <= 1f || fractionComplete.y <= 1f || fractionComplete.z<= 1f);

                if (!complete)
                {
                    transform.localScale = new Vector3
                    {
                        x = Mathf.Lerp(originalScale.x, config.toScale.x, (float) fractionComplete.x),
                        y = Mathf.Lerp(originalScale.y, config.toScale.y, (float) fractionComplete.y),
                        z = Mathf.Lerp(originalScale.z, config.toScale.z, (float) fractionComplete.z)
                    };
                }
                
                yield return null;
            }
#endif

            float speed = Vector3.Distance(config.fromScale, config.toScale) / timeline;
            float distance = 0f;
            float fractionComplete = 0f;

            while (fractionComplete < 1f)
            {
                distance += Time.deltaTime * speed;
                fractionComplete = distance / Vector3.Distance(config.fromScale, config.toScale);

                if (fractionComplete <= 1f)
                {
                    transform.localScale = Vector3.Lerp(config.fromScale, config.toScale, fractionComplete);
                }

                yield return null;
            }

            transform.localScale = config.toScale;
        }

        // private Vector3 SplitSpeed(Config config)
        // {
        //     return new Vector3
        //     {
        //         x = Mathf.Abs(config.toScale.x - config.fromScale.x) / timeline,
        //         y = Mathf.Abs(config.toScale.y - config.fromScale.y) / timeline,
        //         z = Mathf.Abs(config.toScale.z - config.fromScale.z) / timeline
        //     };
        // }
    }
}