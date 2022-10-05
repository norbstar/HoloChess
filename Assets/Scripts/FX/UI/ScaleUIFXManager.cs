using System.Collections.Generic;

using UnityEngine;

using Config = FX.UI.ScaleUIFXComponent.Config;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX Manager")]
    public class ScaleUIFXManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] bool scaleContent = false;
        [field : SerializeField] public List<ScaleUIFXComponent> Components { get; private set; }

        public void Scale(Config config)
        {
            if (config == null) return;

            foreach (ScaleUIFXComponent component in Components)
            {
                component.Scale(new ScaleUIFXComponent.Config
                {
                    flow = config.flow,
                    flags = config.flags,
                    speed = config.speed
                });
            }
        }
    }
}