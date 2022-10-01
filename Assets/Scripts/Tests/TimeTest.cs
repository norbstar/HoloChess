using System.Collections;

using UnityEngine;

namespace Tests
{
    [AddComponentMenu("Tests/Time Test")]
    public class TimeTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] Transform origin;
        [SerializeField] Transform target;
        [SerializeField] float timeline = 2f;

        float distance, speed;

        // Start is called before the first frame update
        void Start()
        {
            distance = Vector3.Distance(origin.position, target.position);
            Debug.Log($"Distance : {distance}");

            speed = distance / timeline;
            Debug.Log($"Speed : {speed}");

            StartCoroutine(Co_Scale(origin.position, target.position));
        }

        private IEnumerator Co_Scale(Vector3 fromScale, Vector3 toScale)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;
            float fractionComplete = 0f;

            while (fractionComplete != 1f)
            {
                elapsedTime = Time.time - startTime;
                fractionComplete = Mathf.Clamp01(elapsedTime / timeline);
                transform.position = Vector3.Lerp(fromScale, toScale, fractionComplete);
                yield return null;
            }

            Debug.Log($"Elapsed Time : {Time.time - startTime}");
        }
    }
}