using System.Collections;

using UnityEngine;

public abstract class AsyncTrigger : MonoBehaviour
{
    public enum Mode
    {
        Cancellable,
        NonCancellable
    }

    [Header("Mode")]
    [SerializeField] Mode mode = Mode.Cancellable;

    protected abstract IEnumerator Co_Routine(object obj = null);

    private Coroutine coroutine;

    public void StartAsync(object obj = null) => StartCoroutine(Co_StartAsync(obj));  

    private IEnumerator Co_StartAsync(object obj)
    {
        switch (mode)
        {
            case Mode.Cancellable:
                StopCoroutine();
                coroutine = StartCoroutine(Co_Routine(obj));
                break;

            case Mode.NonCancellable:
                if (coroutine == null)
                {
                    yield return coroutine = StartCoroutine(Co_Routine(obj));
                    coroutine = null;
                }
                break;
        }
        
        yield return coroutine;
    }

    public void StopAsync() => StopCoroutine();
    
    private void StopCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}