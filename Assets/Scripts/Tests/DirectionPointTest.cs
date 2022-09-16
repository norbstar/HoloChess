using UnityEngine;

namespace Tests
{
    public class DirectionPointTest : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] GameObject pointA;
        [SerializeField] GameObject pointB;
        [SerializeField] GameObject projectedPoint;

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"Point A : {pointA.transform.position}");
            Debug.Log($"Point B : {pointB.transform.position}");
            Debug.Log($"Projected Point : {projectedPoint.transform.position}");
        }

        // Start is called before the first frame update
        void Start()
        {
            Vector3 direction = (pointB.transform.position - pointA.transform.position).normalized;
            float distance = Vector3.Distance(pointA.transform.position, pointB.transform.position);
            var point = pointA.transform.position + direction * distance;
            projectedPoint.transform.position = point;
        }
    }
}