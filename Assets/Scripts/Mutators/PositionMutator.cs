using System.Collections;

using UnityEngine;

using NaughtyAttributes;

namespace Mutator
{
    public class PositionMutator : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject target;
        [Label("Speed (XPS)")]
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
            Vector3 vector = (position - originalPosition).normalized;
            float distance = 0f;

            while (fractionComplete < 1f)
            {
                distance += Time.deltaTime * speed;
                fractionComplete = distance / Vector3.Distance(originalPosition, position);

                if (fractionComplete <= 1f)
                {
                    target.transform.position = originalPosition + (vector * distance);
                    EventReceived?.Invoke(this, fractionComplete);
                }

                yield return null;
            }

            target.transform.position = position;
        }
    }
}