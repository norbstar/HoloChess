using UnityEngine;

namespace Tests
{
    [AddComponentMenu("Tests/Differential Angle Test")]
    public class DifferentialAngleTest : MonoBehaviour
    {
        [SerializeField] GameObject player;
        [SerializeField] Transform enemy;
        [SerializeField] float enemyRange = 25f;
        [SerializeField] Light enemyLight;

        private PlayerController playerController;

        void Awake()
        {
            // float differentialY = DifferentialAngleDegrees(origin.transform.position, target.transform.position);
            // Debug.Log($"Differential : {differentialY}");

            ResolveDependencies();

            enemyLight.spotAngle = enemyRange;
            enemyLight.innerSpotAngle = enemyRange;
        }

        private void ResolveDependencies() => playerController = player.GetComponent<PlayerController>() as PlayerController;

        // private float DifferentialAngleDegrees(Vector3 origin, Vector3 target) {
        //     origin.Normalize();
        //     target.Normalize();
        //     float ADotB = Vector3.Dot(origin, target);
        //     float radians = Mathf.Acos(ADotB);
        //     return radians * Mathf.Rad2Deg;
        // }

         // Update is called once per frame
        void Update()
        {
            // float angleDegrees = Vector3.SignedAngle(target.position, origin.position, Vector3.forward);
            // Debug.Log($"Differential : {angleDegrees}");

            // Debug.DrawRay(origin.position, (target.transform.position - origin.position).normalized, Color.blue);

            Debug.DrawRay(enemy.position, enemy.forward * 5f, Color.red);
            Debug.DrawRay(player.transform.position, player.transform.forward * 5f, Color.blue);

            Vector3 direction = (player.transform.position - enemy.transform.position).normalized;
            Debug.DrawRay(enemy.transform.position, direction * 5f, Color.white);
            // Debug.Log($"Player Direction: {direction}");

            Vector3 cross = Vector3.Cross(enemy.forward, player.transform.forward);
            Debug.Log($"Cross : {cross}");
            int sign = cross.y < 0 ? -1 : 1;
            Debug.Log($"Sign : {sign}");
            float angle = Vector3.Angle(enemy.forward, direction);
            Debug.Log($"Angle : {angle}");

            playerController.SetDetected((angle < enemyRange));

            // Debug.Log($"Angle : {sign * angle}");
            // Debug.Log($"Origin Angle : {enemy.eulerAngles.y}");
            // Debug.Log($"Target Angle : {player.eulerAngles.y}");
            // float singedAngle = Vector3.SignedAngle(origin.forward, target.forward, Vector3.forward);
            // Debug.Log($"Signed Angle : {angle}");
        }
    }
}