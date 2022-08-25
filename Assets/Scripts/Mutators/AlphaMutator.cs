using System.Collections;

using UnityEngine;

namespace Mutator
{
    public class AlphaMutator : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] GameObject target;
        public GameObject Target { get { return target; } }
        // [SerializeField] float speed = 1f;
        // public float Speed { get { return speed; } }
        [SerializeField] float timespan = 0.5f;
        public float Timespan { get { return timespan; } }

        public delegate void Event(AlphaMutator mutator, float fractionComplete);
        public event Event EventReceived;

        public float Alpha { get { return renderer.material.color.a; } }

        private Coroutine coroutine;
        private new Renderer renderer;

        void Awake()
        {
            ResolveDependencies();

            var name = renderer.material.name.Replace(" (Instance)","");
            renderer.material = Instantiate(Resources.Load($"Materials/{name}") as Material);
        }

        private void ResolveDependencies() => renderer = target.GetComponent<Renderer>() as Renderer;

        public void FadeIn()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(Co_FadeIn());
        }

        private IEnumerator Co_FadeIn()
        {
            float startTime = Time.time;
            float fractionComplete = 0f;
            // var originalAlpha = renderer.material.color.a;

            while (fractionComplete < 1f)
            {
                // float distance = (Time.time - startTime) * speed;
                // fractionComplete = distance / 1f;
                // renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Lerp(0f, 1f, fractionComplete));
                
                fractionComplete = ((Time.time - startTime) / timespan) + renderer.material.color.a;
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Lerp(0f, 1f, fractionComplete));
                
                Debug.Log($"Color : {renderer.material.color}");
                EventReceived?.Invoke(this, fractionComplete);
                yield return null;
            }
        }

        public void FadeOut()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(Co_FadeOut());
        }

        private IEnumerator Co_FadeOut()
        {
            float startTime = Time.time;
            float fractionComplete = 0f;
            // var originalAlpha = renderer.material.color.a;

            while (fractionComplete < 1f)
            {
                // float distance = (Time.time - startTime) * speed;
                // fractionComplete = distance / 1f;
                // renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Lerp(0f, 1f, 1f - fractionComplete));

                fractionComplete = ((Time.time - startTime) / timespan) + (1f - renderer.material.color.a);
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Lerp(1f, 0f, fractionComplete));

                Debug.Log($"Color : {renderer.material.color}");
                EventReceived?.Invoke(this, fractionComplete);
                yield return null;
            }
        }
    }
}