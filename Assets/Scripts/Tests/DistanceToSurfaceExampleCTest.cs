using UnityEngine;

using Utilities.Points;

namespace Tests
{
    // See https://www.redcrab-software.com/en/Calculator/Spherical-Cap
    public class DistanceToSurfaceExampleCTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject layer;
        [SerializeField] bool enableGizmos = true;
        
        private float diameter, radius;
        private Vector3 point, point2;

        void Awake()
        {
            diameter = layer.transform.localScale.z;
            radius = diameter * 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            float height = Mathf.Abs((layer.transform.localPosition.y - radius) - transform.localPosition.y);
            Debug.Log($"Height : {height}");

            float capRadius = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(radius - height, 2));
            
            // if (float.IsNaN(capRadius))
            // {
            //     capRadius = 0f;
            // }

            point = transform.position + transform.right * capRadius;
            PointProjectorDatabase.PlotPoint($"Point", $"Point", PointProjector.Type.Red, point);

            Debug.Log($"Cap Radius : {capRadius}");
        }
    }
}