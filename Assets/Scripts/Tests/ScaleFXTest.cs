using System.Collections;

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
        [SerializeField] float timeline = 1f;
        [SerializeField] bool enableGizmos = true;

        private ScaleFXManager scaleFXManager;
        private PointProjector projector;
        private Vector3 fromScale, toScale, tweenPointScale;
        private Coroutine coroutine;
        private bool isLerping;

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
            // projector = PointProjectorDatabase.PlotPoint($"Tween Point", $"Tween Point", PointProjector.Type.Blue, new PointProjector.PointProperties
            // {
            //     position = tweenScale,
            //     rotation = Quaternion.identity,
            //     scale = tweenScale,
            //     enableDebug = true
            // });
        }

        // Update is called once per frame
        void Update()
        {
            if (isLerping) return;

            tweenPointScale = GetTweenScale(fromScale, toScale, tweenPoint);
            projector.Point.position = tweenPointScale;
            projector.Point.scale = tweenPointScale;

            // Debug.Log($"Tween Point Scale : {tweenPointScale.ToString()}");
        }

        private Vector3 GetTweenScale(Vector3 fromScale, Vector3 toScale, float normalizedPoint) => Vector3.Lerp(fromScale, toScale, normalizedPoint);

        public void OnScaleUp()
        {
            float range = Vector3.Distance(fromScale, toScale);
            float point = Vector3.Distance(tweenPointScale, toScale);
            float normalizedPoint = point / range;

            coroutine = StartCoroutine(Co_Scale(tweenPointScale, toScale, normalizedPoint));
            // Debug.Log($"OnScaleUp Normalized Point : {normalizedPoint}");
        }

        public void OnScaleDown()
        {
            float range = Vector3.Distance(fromScale, toScale);
            float point = Vector3.Distance(fromScale, tweenPointScale);
            float normalizedPoint = point / range;

            coroutine = StartCoroutine(Co_Scale(tweenPointScale, fromScale, normalizedPoint));
            // Debug.Log($"OnScaleDown Normalized Point : {normalizedPoint}");
        }

        private IEnumerator Co_Scale(Vector3 fromScale, Vector3 toScale, float normalizedPoint)
        {
            float startTime = Time.time;
            float speed = normalizedPoint / timeline;
            float elapsedTime = 0f;
            float fractionComplete = 0f;

            isLerping = true;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            // Debug.Log($"Co_Scale Start Position : {projector.Point.position} Start Scale : {projector.Point.scale}");

            while (fractionComplete < 1f)
            {
                elapsedTime += Time.deltaTime * speed;
                fractionComplete = elapsedTime / normalizedPoint;

                if (fractionComplete <= 1f)
                {
                    projector.Point.position = projector.Point.scale = Vector3.Lerp(fromScale, toScale, fractionComplete);
                    // Debug.Log($"Co_Scale Position : {projector.Point.position} Scale : {projector.Point.scale}");
                }

                yield return null;
            }

            projector.Point.position = projector.Point.scale = toScale;
            // Debug.Log($"Co_Scale End Position : {projector.Point.position} End Scale : {projector.Point.scale}");
            isLerping = false;
        }

        void OnDrawGizmos()
        {
            if (!enableGizmos)  return;
            Gizmos.DrawLine(fromScale, toScale);
        }
    }
}