using System.Collections.Generic;

using UnityEngine;

using FX.UI;

using ScaleType = FX.UI.ScaleUIFX.ScaleType;
using Flow = FX.UI.ScaleUIFXComponent.Flow;
using Flags = FX.UI.ScaleUIFXComponent.Flags;

namespace Tests
{
    [AddComponentMenu("Tests/Scale UI FX Test")]
    [RequireComponent(typeof(ScaleUIFXManager))]
    public class ScaleUIFXTest : MonoBehaviour
    {
        private ScaleUIFXManager scaleUIFXManager;
        private List<ScaleUIFXComponent> components;

        void Awake()
        {
            ResolveDependencies();
            components = scaleUIFXManager.Components;
        }

        private void ResolveDependencies() => scaleUIFXManager = GetComponent<ScaleUIFXManager>() as ScaleUIFXManager;

        // Start is called before the first frame update
        void Start()
        {
            foreach (ScaleUIFXComponent component in components)
            {
                component.Scale(new ScaleUIFXComponent.Config
                {
                    // type = ScaleType.Proportional,
                    flow = Flow.ToEnd,
                    flags = Flags.ScaleContent
                });
            }
        }
    }
}