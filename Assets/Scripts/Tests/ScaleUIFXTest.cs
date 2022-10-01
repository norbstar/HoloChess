using System.Collections.Generic;

using UnityEngine;

using FX.UI;

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

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}