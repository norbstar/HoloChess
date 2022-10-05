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

    private void IncrementCount()
    {
        ++count;
        // Debug.Log($"IncrementCount {count}");
    }

    protected void DecrementCount()
    {
        --count;
        // Debug.Log($"DecrementCount {count}");
    }

    public void StartAsync(object obj = null) => StartCoroutine(Co_StartAsync(obj));

    private IEnumerator Co_StartAsync(object obj)
    {
        StopCoroutine();
        
        if (id == Int32.MaxValue)
        {
            id = 0;
        }
        
        ++id;
        // Debug.Log($"Co_StartAsync Id : {id}");

        var coroutine = StartCoroutine(Co_Routine(id, obj));

        contextData = new ContextData
        {
            coroutine = coroutine,
            id = id
        };
        
        // Debug.Log($"Co_StartAsync Setting ContextData Has Coroutine : {coroutine != null}");

        IncrementCount();
        yield return coroutine;
    }

    public void StopAsync() => StopCoroutine();
    
    private void StopCoroutine()
    {
        // Debug.Log($"StopCoroutine Has ContextData : {contextData != null}");
        if (contextData == null) return;
        // Debug.Log($"StopCoroutine");

        StopCoroutine(contextData.coroutine);
        MarkEndCoroutine();
    }

    protected void MarkEndCoroutine()
    {
        // Debug.Log($"[Pre] MarkEndCoroutine Has ContextData : {contextData != null}");
        DecrementCount();
        contextData = null;
        // Debug.Log($"[Post] MarkEndCoroutine Has ContextData : {contextData != null}");
    }
}