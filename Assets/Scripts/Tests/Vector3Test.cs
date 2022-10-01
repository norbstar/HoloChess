using UnityEngine;

namespace Tests
{
    [AddComponentMenu("Tests/Vector3 Test")]
    public class Vector3Test : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] Transform p0;
        [SerializeField] Transform p1;
        [SerializeField] Transform p2;
        [SerializeField] Transform p3;

#if false
        // Start is called before the first frame update
        void Start()
        {
            PointProjectorDatabase.PlotPoint($"P0", $"P0 {p0.position}", PointProjector.Type.Red, p0.position);
            PointProjectorDatabase.PlotPoint($"P1", $"P1 {p1.position}", PointProjector.Type.Green, p1.position);
            
            p2.position = p0.transform.position + new Vector3(p1.transform.position.x, 0f, 0f);
            PointProjectorDatabase.PlotPoint($"P2", $"P2 {p2.position}", PointProjector.Type.Blue, p2.position);

            Vector3[] positions = new Vector3[4];
            positions[0] = p0.position;
            positions[1] = p2.position;
            positions[2] = p1.position;
            positions[3] = p0.position;
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

            // Magnitude is the length of the vector calculated as the square root of (x*x+y*y+z*z)
            Debug.Log($"P0 Position : {p0.position} Magnitude : {p0.position.magnitude}");
            Debug.Log($"P1 Position : {p1.position} Magnitude : {p1.position.magnitude}");

            // Use the Pythagorean theorem to calculate the hypotenuse from right triangle sides.
            // Take a square root of sum of squares: c = √(a² + b²)
            float lenX =  p1.position.x - p0.position.x;
            float lenY =  p1.position.y - p0.position.y;
            float hypotenuse = Mathf.Sqrt((lenX * lenX) + (lenY * lenY));
            Debug.Log($"Hypotenuse of [{lenX}, {lenY}] : {hypotenuse}");

            Vector3 cross = Vector3.Cross(-p2.up, -p2.right).normalized;
            p3.position = cross;
            PointProjectorDatabase.PlotPoint($"P3", $"P3 {p3.position}", PointProjector.Type.White, p3.position);
        }
#endif

#if true
        // Start is called before the first frame update
        void Start()
        {
            // PointProjectorDatabase.PlotPoint($"{gameObject.name} P2", $"P2 {p2.position}", PointProjector.Type.Red, p2.position);
            // PointProjectorDatabase.PlotPoint($"{gameObject.name} P3", $"P3 {p3.position}", PointProjector.Type.Green, p3.position);

            // float angle = Vector3.SignedAngle(p2.forward, p3.forward, Vector3.right);
            // Debug.Log($"{gameObject.name} Angle : {angle}");

            // Vector3 tmp = p3.transform.localEulerAngles;
            // tmp.x += angle;
            // p3.transform.localEulerAngles = tmp;
        }

        // Update is called once per frame
        void Update()
        {
            float angle;

            // angle = Vector3.SignedAngle(p2.forward, p3.forward, Vector3.up);
            // Debug.Log($"{gameObject.name} Up Angle [1] : {angle}");

            // angle = Vector3.SignedAngle(p2.forward, p3.forward, Vector3.down);
            // Debug.Log($"{gameObject.name} Down Angle : {angle}");

            // angle = Vector3.SignedAngle(p2.forward, p3.forward, Vector3.left);
            // Debug.Log($"{gameObject.name} Left Angle : {angle}");

            // angle = Vector3.SignedAngle(p2.forward, p3.forward, Vector3.right);
            // Debug.Log($"{gameObject.name} Right Angle : {angle}");

            // angle = Vector3.SignedAngle(p2.forward, p3.forward, Vector3.forward);
            // Debug.Log($"{gameObject.name} Forward Angle : {angle}");

            // angle = Vector3.SignedAngle(p2.forward, p3.forward, Vector3.back);
            // Debug.Log($"{gameObject.name} Back Angle : {angle}");

            angle = Vector3.SignedAngle(p2.up, p3.up, Vector3.up);
            Debug.Log($"{gameObject.name} Up Angle : {angle}");

            angle = Vector3.SignedAngle(p2.up, p3.up, Vector3.right);
            Debug.Log($"{gameObject.name} Right Angle : {angle}");

            // angle = Vector3.SignedAngle(p2.up, p3.up, Vector3.right);
            // Debug.Log($"{gameObject.name} Right Angle : {angle}");

            Debug.DrawRay(p2.position, p2.forward * 2f, Color.red);
            Debug.DrawRay(p3.position, p3.forward * 2f, Color.green);
        }
#endif
    }
}