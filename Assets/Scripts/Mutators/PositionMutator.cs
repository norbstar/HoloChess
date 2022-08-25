using System.Collections;

using UnityEngine;

namespace Mutator
{
    public class PositionMutator : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject target;
        public GameObject Target { get { return target; } }
        [SerializeField] float speed = 1f;
        public float Speed { get { return speed; } }

        public delegate void Event(PositionMutator mutator, float fractionComplete);
        public event Event EventReceived;

        public Vector3 Position { get { return target.transform.position; } }

        private Coroutine coroutine;

        public void MoveTo(Vector3 position)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(Co_MoveTo(position));
        }

        private IEnumerator Co_MoveTo(Vector3 position)
        {
            float startTime = Time.time;
            float fractionComplete = 0f;
            Vector3 originalPosition = target.transform.position;
            Debug.Log($"Original Position : {originalPosition}");

            while (fractionComplete < 1f)
            {
                float distance = (Time.time - startTime) * speed;
                // Debug.Log($"Distance : {distance}");
                fractionComplete = distance / Vector3.Distance(originalPosition, position);
                // Debug.Log($"FC : {fractionComplete}");
                var vector = (position - target.transform.position).normalized;
                // Debug.Log($"Vector : {vector}");
                target.transform.position += vector * speed * Time.deltaTime;
                Debug.Log($"Position : {target.transform.position}");
                EventReceived?.Invoke(this, fractionComplete);
                yield return null;
            }

            target.transform.position = position;
        }
    }
}