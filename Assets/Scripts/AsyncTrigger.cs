using System;
using System.Collections;

using UnityEngine;

public abstract class AsyncTrigger : MonoBehaviour
{
    protected abstract IEnumerator Co_Routine(int id, object obj = null);

    public class ContextData
    {
        public Coroutine coroutine;
        public int id;
    }

    private ContextData contextData;
    private int id, count;

    private void IncrementCount() => ++count;

    protected void DecrementCount() => --count;

    public void StartAsync(object obj = null) => StartCoroutine(Co_StartAsync(obj));

    private IEnumerator Co_StartAsync(object obj)
    {
        StopCoroutine();
        
        if (id == Int32.MaxValue)
        {
            id = 0;
        }
        
        ++id;

        var coroutine = StartCoroutine(Co_Routine(id, obj));

        contextData = new ContextData
        {
            coroutine = coroutine,
            id = id
        };
        
        IncrementCount();
        yield return coroutine;
    }

    public void StopAsync() => StopCoroutine();
    
    private void StopCoroutine()
    {
        if (contextData == null) return;

        StopCoroutine(contextData.coroutine);
        MarkEndCoroutine();
    }

    protected void MarkEndCoroutine()
    {
        DecrementCount();
        contextData = null;
    }
}