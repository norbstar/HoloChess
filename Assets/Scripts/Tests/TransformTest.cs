using UnityEngine;

namespace Tests
{
    [AddComponentMenu("Tests/Transform Test")]
    public class TransformTest : MonoBehaviour
    {
        [SerializeField] Transform origin;
        [SerializeField] Transform rotatingObject;
        [SerializeField] Transform reference;

        // Update is called once per frame
        void Update()
        {
            reference.eulerAngles = new Vector3(0, rotatingObject.eulerAngles.y, 0);
            Vector3 direction = rotatingObject.position - origin.position;
            float angle = Vector3.Angle(reference.forward, direction);
            Debug.Log(angle);
        }
    }
}