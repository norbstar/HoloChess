using UnityEngine;

namespace Tests
{
    public class ScaleTest : MonoBehaviour
    {
        [SerializeField] GameObject target;

        private new SphereCollider collider;
        private new Renderer renderer;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies()
        {
            renderer = target.GetComponent<Renderer>() as Renderer;
            collider = target.GetComponent<SphereCollider>() as SphereCollider;
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log($"World Scale : {target.transform.lossyScale.x} {target.transform.lossyScale.y} {target.transform.lossyScale.z}");

            Debug.Log($"Radius [1] : {collider.radius}");
            Debug.Log($"Radius [2] : {renderer.bounds.extents.magnitude}");

            float actualRadius = collider.radius * Mathf.Max(target.transform.lossyScale.x, target.transform.lossyScale.y, target.transform.lossyScale.z);
            Debug.Log($"Radius [3] : {actualRadius}");
        }
    }
}