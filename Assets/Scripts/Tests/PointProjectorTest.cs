using UnityEngine;

using Utilities.Points;

namespace Tests
{
    public class PointProjectorTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] Vector3 scale;

        private PointProjectorManager manager;
        private PointProjector pointA, pointB, pointC;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => manager = FindObjectOfType<PointProjectorManager>() as PointProjectorManager;

        void OnEnable()
        {
            pointA = manager.Add(PointProjector.Type.Red, "Point A", "A");
            pointA.Point.scale = scale;

            pointB = manager.Add(PointProjector.Type.Green, "Point B", "B");
            pointB.Point.scale = scale;
            
            pointC = manager.Add(PointProjector.Type.Blue, "Point C", "C");
            pointC.Point.scale = scale;
        }

        void OnDisable()
        {
            manager.Remove("Point A");
            manager.Remove("Point B");
            manager.Remove("Point C");
        }

        // Update is called once per frame
        void Update()
        {
            pointA.Point = new PointProjector.PointProperties
            {
                position = transform.position
            };

            pointB.Point = new PointProjector.PointProperties
            {
                position = transform.position + (transform.localScale * 0.5f)
            };

            pointC.Point = new PointProjector.PointProperties
            {
                position = transform.position - (transform.localScale * 0.5f)
            };
        }
    }
}