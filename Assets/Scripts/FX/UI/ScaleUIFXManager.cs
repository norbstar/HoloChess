using System;
using System.Collections.Generic;

using UnityEngine;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX Manager")]
    public class ScaleUIFXManager : MonoBehaviour
    {
        [field : SerializeField] public List<ScaleUIFXComponent> Components { get; private set; }

        public enum ScaleType
        {
            Uniform,
            Custom
        }

        [Flags]
        public enum Flags
        {
            // https://docs.unity3d.com/ScriptReference/InspectorNameAttribute.html
            [InspectorName("Foo Flag")]
            Foo = 0
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}