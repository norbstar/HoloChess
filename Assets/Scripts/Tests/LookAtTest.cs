using UnityEngine;

namespace Tests
{
    [AddComponentMenu("Tests/Look At Test")]
    public class LookAtTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] Transform target;
        [SerializeField] Vector3 worldUp = Vector3.up;

        // Frame-rate independent call for physics calculations
        void FixedUpdate()
        {
            transform.LookAt(target, worldUp);
        }
    }
}