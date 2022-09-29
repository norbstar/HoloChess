using UnityEngine;

namespace FX
{
    [RequireComponent(typeof(ScaleFX))]
    public class ScaleFXComponent : MonoBehaviour
    {
        public enum ScaleType
        {
            Proportional,
            RelativeToX,
            RelativeToY,
            RelativeToZ
        }

        [Header("Config")]
        [SerializeField] ScaleType scaleType = ScaleType.Proportional;
        [SerializeField] float scaleFactor = 1.1f;

        private ScaleFX scaleFX;
        private RectTransform rectTransform;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies()
        {
            scaleFX = GetComponent<ScaleFX>() as ScaleFX;
            rectTransform = GetComponent<RectTransform>() as RectTransform;
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