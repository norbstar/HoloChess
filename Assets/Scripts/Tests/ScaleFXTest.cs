using System.Collections;

using UnityEngine;

using FX;
using Utilities.Points;

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
        private Vector3 heading, direction, fromScale, toScale, tweenScale;
        private float range;
        private Coroutine coroutine;

        void Awake()
        {
            ResolveDependencies();
            fromScale = transform.localScale;
            toScale = transform.localScale * scaleFactor;
            range = Vector3.Distance(fromScale, toScale);
            heading = toScale - fromScale;
            direction = heading.normalized;
        }

        private void ResolveDependencies() => scaleFXManager = GetComponent<ScaleFXManager>() as ScaleFXManager;

        // Start is called before the first frame update
        void Start()
        {
            PointProjectorDatabase.PlotPoint($"From", $"From", PointProjector.Type.Red, fromScale, Quaternion.identity, fromScale);
            PointProjectorDatabase.PlotPoint($"To", $"To", PointProjector.Type.Green, toScale, Quaternion.identity, toScale);
        }

        // Update is called once per frame
        void Update()
        {
            tweenScale = GetTweenScale(tweenPoint);

            if (projector == null)
            {
                projector = PointProjectorDatabase.PlotPoint($"Tween Point", $"Tween Point", PointProjector.Type.Blue, tweenScale, Quaternion.identity, tweenScale);
            }

            projector.Point.position = tweenScale;
            projector.Point.scale = tweenScale;
        }

        private Vector3 GetTweenScale(float normalizedPoint) => Vector3.Lerp(fromScale, toScale, normalizedPoint);

        private float CalculateTweenPoint(Vector3 tweenScale) => Vector3.Distance(fromScale, tweenScale) / range;

        public void OnScaleUp() => coroutine = StartCoroutine(Co_Scale(tweenScale, toScale));

        public void OnScaleDown() => coroutine = StartCoroutine(Co_Scale(tweenScale, fromScale));

        public void OnStop() => StopCoroutine(coroutine);

        private IEnumerator Co_Scale(Vector3 startScale, Vector3 endScale)
        {
            float startTime = Time.time;
            float fractionalTimeline = timeline * Vector3.Distance(startScale, endScale) / range;
            float fractionComplete = 0f;
  
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            while (fractionComplete != 1f)
            {
                float elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / fractionalTimeline);
                Vector3 tweenScale = Vector3.Lerp(startScale, endScale, fractionComplete);
                tweenPoint = CalculateTweenPoint(tweenScale);
                yield return null;
            }

            // Debug.Log($"Elapsed Time : {Time.time - startTime}");
        }

        void OnDrawGizmos()
        {
            if (!enableGizmos)  return;
            Gizmos.DrawLine(fromScale, toScale);
        }
    }
}