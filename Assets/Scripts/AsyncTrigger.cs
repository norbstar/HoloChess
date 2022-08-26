using System.Collections;

using UnityEngine;

public abstract class AsyncTrigger : MonoBehaviour
{
    protected abstract IEnumerator Co_Routine(object obj = null);

    private Coroutine coroutine;

    public void StartAsync(object obj = null) => StartCoroutine(Co_StartAsync(obj));  

    private IEnumerator Co_StartAsync(object obj)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Co_Routine(obj));
            yield return coroutine;
            
            Reset();
        }
    }

    public void StopAsync() => StopCoroutine();
    
    private void StopCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            Reset();
        }
    }

    private void Reset() => coroutine = null;
}