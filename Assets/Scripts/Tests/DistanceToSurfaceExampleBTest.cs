using UnityEngine;

namespace Tests
{
    public class DistanceToSurfaceExampleBTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject layer;
        [SerializeField] bool enableGizmos = true;
        
        private float radius;
        private Vector3 point, point2;

        void Awake() => radius = layer.transform.localScale.z * 0.5f;

        // Update is called once per frame
        void Update()
        {
            point = transform.position + transform.right * radius;
            PointProjectorDatabase.PlotPoint($"Point A", $"Point A", PointProjector.Type.Red, point);

            Vector3 direction = (point - layer.transform.position).normalized;
            point2 = layer.transform.position + direction * radius;
            PointProjectorDatabase.PlotPoint($"Point B", $"Point B", PointProjector.Type.Green, point2);
        }

        void OnDrawGizmos()
        {
            if (!enableGizmos)  return;
            Gizmos.DrawLine(layer.transform.position, point);
        }
    }
}