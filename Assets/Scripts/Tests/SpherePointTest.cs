using UnityEngine;

namespace Tests
{
    public class SpherePointTest : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] GameObject currentPoint;
        [SerializeField] GameObject referencePoint;
        [SerializeField] GameObject interimPoint;
        [SerializeField] GameObject target;
        [SerializeField] GameObject targetPoint;

        // Update is called once per frame
        void Update()
        {
            // Debug.DrawRay(transform.position, (currentPoint.transform.position - transform.position).normalized, Color.blue);
            // Debug.DrawRay(transform.position, (referencePoint.transform.position - transform.position).normalized, Color.red);
            // Debug.DrawRay(transform.position, (targetPoint.transform.position - transform.position).normalized, Color.green);
            // Debug.DrawRay(transform.position, (interimPoint.transform.position - transform.position).normalized, Color.yellow);
            // Debug.DrawRay(targetPoint.transform.position, (referencePoint.transform.position - targetPoint.transform.position).normalized, Color.white);
            // Debug.DrawRay(targetPoint.transform.position, (currentPoint.transform.position - targetPoint.transform.position).normalized, Color.white);
            // Debug.DrawRay(targetPoint.transform.position, (interimPoint.transform.position - targetPoint.transform.position).normalized, Color.white);

            // Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
            // Debug.DrawRay(transform.position, forward, Color.green);

            // Debug.Log($"Current Point : {FormatPosition(currentPoint.transform.position)} Distance : {Vector3.Distance(transform.position, currentPoint.transform.position)}");
            // Debug.Log($"Reference Point : {FormatPosition(referencePoint.transform.position)} Distance : {Vector3.Distance(transform.position, referencePoint.transform.position)}");
            // Debug.Log($"Intertim Point : {FormatPosition(interimPoint.transform.position)} Distance : {Vector3.Distance(transform.position, interimPoint.transform.position)}");
            // Debug.Log($"Target Point : {FormatPosition(targetPoint.transform.position)} Distance : {Vector3.Distance(transform.position, targetPoint.transform.position)}");
            
            // Vector3 cross = Vector3.Cross(transform.forward, targetPoint.transform.forward);
            // Debug.Log($"Cross : {cross}");
            // int sign = cross.y < 0 ? -1 : 1;
            // Debug.Log($"Sign : {sign}");
            // float angle = Vector3.Angle(transform.forward, targetPoint.transform.forward);
            // Debug.Log($"Angle : {angle}");
            // angle = Vector3.SignedAngle(transform.forward, targetPoint.transform.forward, transform.right);
            // Debug.Log($"Signed Angle : {angle}");
        }

        private string FormatPosition(Vector3 position) => $"[<color=red>x</color>:{position.x}, <color=green>y</color>:{position.y}, <color=blue>z</color>:{position.z}]";

        public void OnExecute()
        {
            Vector3 direction = (referencePoint.transform.position - transform.position).normalized;
            var projectedPoint = transform.position + direction * (transform.localScale.z * 0.5f);
            interimPoint.transform.position = projectedPoint;

            target.transform.LookAt(interimPoint.transform.position);
            // targetPoint.transform.position = interimPoint.transform.position;

            // float angle = DifferentialAngleDegrees(transform.position, currentPoint.transform.position);
            Vector3 cross = Vector3.Cross(transform.forward, currentPoint.transform.forward);
            Debug.Log($"Cross : {cross}");
            int sign = cross.y < 0 ? -1 : 1;
            Debug.Log($"Sign : {sign}");
            float angle = Vector3.SignedAngle(transform.forward, currentPoint.transform.forward, transform.right);
            Debug.Log($"Angle : {angle}");
            // float angle = Vector3.SignedAngle(transform.forward, currentPoint.transform.forward, transform.forward);
            // Debug.Log($"Angle : {sign * angle}");
            // target.transform.Rotate(new Vector3(angle, target.transform.rotation.x, target.transform.rotation.y));
            Vector3 tmp = target.transform.localEulerAngles;
            tmp.x = angle;
            target.transform.localEulerAngles = tmp;

#if false
            float lockY = currentPoint.transform.position.y;
            var lockedPoint = new Vector3(referencePoint.transform.position.x, lockY, referencePoint.transform.position.z);
            interimPoint.transform.position = lockedPoint;
            Vector3 relativeDirection = (lockedPoint - transform.position).normalized;
            var point = transform.position + relativeDirection * (transform.localScale.z * 0.5f);
            targetPoint.transform.position = point;

            Debug.Log($"Target Point : {targetPoint.transform.localPosition}");
#endif

#if false
            float lockY = Round(currentPoint.transform.position.y);
            var lockedPoint = new Vector3(Round(referencePoint.transform.position.x), lockY, Round(referencePoint.transform.position.z));
            interimPoint.transform.position = lockedPoint;
            Vector3 relativeDirection = (lockedPoint - Round(transform.position)).normalized;
            var point = Round(transform.position) + relativeDirection * (Round(transform.localScale.z) * 0.5f);
            targetPoint.transform.position = point;

            Debug.Log($"Target Point : {Round(targetPoint.transform.localPosition)}");
#endif
        }

        // private float Round(float value) => (float) System.Math.Round(value, 2);

        // private Vector3 Round(Vector3 value) => new Vector3((float) System.Math.Round(value.x, 2), (float) System.Math.Round(value.y, 2), (float) System.Math.Round(value.z, 2));

        // private float DifferentialAngleDegrees(Vector3 origin, Vector3 target) {
        //     origin.Normalize();
        //     target.Normalize();
        //     float ADotB = Vector3.Dot(origin, target);
        //     float radians = Mathf.Acos(ADotB);
        //     return radians * Mathf.Rad2Deg;
        // }
    }
}