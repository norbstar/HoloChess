using UnityEngine;

namespace Tests
{
    public class TransformPointTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Vector3 requiredScale = transform.parent.InverseTransformPoint(Vector3.one);

            transform.localScale = requiredScale;
            Debug.Log($"{requiredScale}");
        }
    }
}