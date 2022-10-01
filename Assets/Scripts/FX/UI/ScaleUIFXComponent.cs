using UnityEngine;

using ScaleType = FX.UI.ScaleUIFXManager.ScaleType;

namespace FX.UI
{
    [AddComponentMenu("FX/UI/Scale UI FX Component")]
    [RequireComponent(typeof(ScaleUIFX))]
    public class ScaleUIFXComponent : MonoBehaviour
    {
        public ScaleType scaleType = ScaleType.Uniform;
        [field : SerializeField] public bool ScaleContent { get; private set; }

        private ScaleUIFX scaleUIFX;
        public ScaleUIFX ScaleUIFX { get { return scaleUIFX; } }

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => scaleUIFX = GetComponent<ScaleUIFX>() as ScaleUIFX;

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