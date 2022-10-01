using UnityEngine;

namespace Tests
{
    [AddComponentMenu("Tests/Vectors Test")]
    public class VectorsTest : MonoBehaviour
    {
#if false
        [Header("Components")]
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] GameObject start;
        [SerializeField] GameObject end;

        // Start is called before the first frame update
        void Start()
        {
            float angle = Vector3.SignedAngle(start.transform.forward, end.transform.forward, Vector3.up);
            // float angle = Vector3.SignedAngle(start.transform.up, end.transform.up, Vector3.right);
            Debug.Log($"Angle : {angle}");

            Vector3[] positions = new Vector3[2];
            positions[0] = start.transform.position;
            positions[1] = end.transform.position;
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 direction = (end.transform.position - start.transform.position).normalized;
            Debug.DrawRay(start.transform.position, direction * 10f, Color.red);
        }
#endif

#if true
        [Header("Components")]
        [SerializeField] Transform enemy;
        [SerializeField] Transform player;

        // Update is called once per frame
        void Update()
        {
            Debug.DrawRay(player.position, player.forward * 2f, Color.white);

            Vector3 playerDirection = player.position - enemy.position;
            Debug.Log($"Direction : {playerDirection}");

            // Calculates the angle between vectors from and to.
            // The angle returned will always be between 0 and 180 degrees, because the method returns the smallest angle between the vectors.
            float angle = Vector3.Angle(playerDirection, enemy.transform.forward);
            Debug.Log($"Angle : {angle}");

            // Calculates the signed angle between vectors from and to in relation to axis.
            float signedAngle = Vector3.SignedAngle(playerDirection, enemy.transform.forward, Vector3.up);
            Debug.Log($"Signed Angle : {signedAngle}");

            if (angle < 5.0f)
            {
                Debug.DrawRay(enemy.position, enemy.forward * 2f, Color.red);
                Debug.Log($"Status : <color=red>Detected</color>");
            }
            else
            {
                Debug.DrawRay(enemy.position, enemy.forward * 2f, Color.green);
                Debug.Log($"Status : <color=green>Not detected</color>");
            }
        }
#endif
    }
}