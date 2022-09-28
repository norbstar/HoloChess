using UnityEngine;

using Utilities.Points;

namespace Tests
{
    public class DistanceToSurfaceExampleATest : MonoBehaviour
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
            float distance = radius - Mathf.Abs(transform.localPosition.y);
            point = transform.position + transform.right * distance;
            PointProjectorDatabase.PlotPoint($"Point", $"Point", PointProjector.Type.Red, point);
            Debug.Log($"Distance : {distance}");
        }

        void OnDrawGizmos()
        {
            if (!enableGizmos)  return;
            Gizmos.DrawLine(layer.transform.position, point);
        }
    }
}