using UnityEngine;

namespace Tests
{
    public class ScaleTest : MonoBehaviour
    {
        [SerializeField] new SphereCollider collider;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log($"Radius : {collider.radius}");
        }
    }
}