using System;
using System.Collections;

using UnityEngine;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX")]
    [RequireComponent(typeof(RectTransform))]
    public class ScaleUIFX : AsyncTrigger
    {
        [field : SerializeField] public float Timeline { get; private set; } = 1f;

        [Serializable]
        public class Properties { }

        private RectTransform rectTransform;
        private Properties properties;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => rectTransform = GetComponent<RectTransform>() as RectTransform;

        protected override IEnumerator Co_Routine(object obj)
        {
            properties = (Properties) obj;
            
            // TODO

            yield return null;
        }
    }
}