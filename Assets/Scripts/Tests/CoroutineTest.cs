using System.Collections;

using UnityEngine;

namespace Tests
{
    public class CoroutineTest : MonoBehaviour
    {
        private Coroutine coroutine;

        // Start is called before the first frame update
        void Start()
        {
            coroutine = StartCoroutine(Co_Yield());
            Debug.Log($"Has Coroutine : {coroutine != null}");

            // Note that this routine does not return a coroutine one yields
            coroutine = StartCoroutine(Co_NoYield(0));
            Debug.Log($"Has Coroutine : {coroutine != null}");

            coroutine = StartCoroutine(Co_YieldBreak());
            Debug.Log($"Has Coroutine : {coroutine != null}");
        }

        private IEnumerator Co_Yield()
        {
            Debug.Log($"Co_Yield Start");
            
            yield return null;

            Debug.Log($"Co_Yield End");
        }

        private IEnumerator Co_NoYield(int value)
        {
            Debug.Log($"Co_NoYield Start");

            if (value != 0) yield return null;

            Debug.Log($"Co_NoYield End");
        }

        private IEnumerator Co_YieldBreak()
        {
            Debug.Log($"Co_YieldBreak Start");

            // yield return null;
            yield break;

            Debug.Log($"Co_YieldBreak End");
        }

#if false
        [Header("Config")]
        [SerializeField] int iterations = 100;
        [SerializeField] float startDelay = 0.5f;
        [SerializeField] float sleepDelay = 1.5f;

        private Coroutine coroutine;
        private int id, count;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            for (int itr = 0; itr < iterations; itr++)
            {
                if (coroutine != null)
                {
                    Debug.Log($"StopCoroutine Id : {id}");
                    StopCoroutine(coroutine);
                    ModifyCount(--count);
                }

                Debug.Log($"StartCoroutine Id : {++id}");
                coroutine = StartCoroutine(Co_DoWork(id));
                ModifyCount(++count);
                yield return new WaitForSeconds(startDelay);
            }
        }

        private IEnumerator Co_DoWork(int id)
        {
            Debug.Log($"Co_DoWork Id : {id} Start");
            
            yield return new WaitForSeconds(sleepDelay);

            Debug.Log($"Co_DoWork Id : {id} End");
            ModifyCount(--count);
        }

        private void ModifyCount(int count) => Debug.Log($"Active Count : {count}");
#endif
    }
}