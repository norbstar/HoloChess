using UnityEngine;

using FX;

namespace Tests
{
    [RequireComponent(typeof(ScaleFXManager))]
    public class ScaleFXTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] float scaleFactor = 1.1f;
        [Range(0, 1)]
        [SerializeField] float tweenPoint = 0.5f;
        [SerializeField] bool enableGizmos = true;

        private ScaleFXManager scaleFXManager;
        private PointProjector projector;
        private Vector3 fromScale, toScale;

        void Awake()
        {
            ResolveDependencies();
            fromScale = transform.localScale;
            toScale = transform.localScale * scaleFactor;
        }

        private void ResolveDependencies() => scaleFXManager = GetComponent<ScaleFXManager>() as ScaleFXManager;

        // Start is called before the first frame update
        void Start()
        {
            PointProjectorDatabase.PlotPoint($"From", $"From", PointProjector.Type.Red, fromScale, Quaternion.identity, fromScale);
            PointProjectorDatabase.PlotPoint($"To", $"To", PointProjector.Type.Green, toScale, Quaternion.identity, toScale);
            
            Vector3 tweenScale = GetTweenScale(fromScale, toScale, tweenPoint);
            projector = PointProjectorDatabase.PlotPoint($"Tween Point", $"Tween Point", PointProjector.Type.Blue, tweenScale, Quaternion.identity, tweenScale);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 tweenScale = GetTweenScale(fromScale, toScale, tweenPoint);
            projector.Point.position = tweenScale;
            projector.Point.scale = tweenScale;
            Debug.Log($"Tween Scale : {tweenScale.ToString()}");
        }

        private Vector3 GetTweenScale(Vector3 fromScale, Vector3 toScale, float normalizedPoint) => Vector3.Lerp(fromScale, toScale, normalizedPoint);

        void OnDrawGizmos()
        {
            if (!enableGizmos)  return;
            Gizmos.DrawLine(fromScale, toScale);
        }
    }
}