using System.Collections.Generic;

using UnityEngine;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX Manager")]
    public class ScaleUIFXManager : MonoBehaviour
    {
        [field : SerializeField] public List<ScaleUIFXComponent> Components { get; private set; }

        public void Scale(List<ScaleUIFXComponent> components)
        {
            foreach (ScaleUIFXComponent component in components)
            {
                // TODO
            }
        }
    }
}