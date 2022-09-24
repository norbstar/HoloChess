using UnityEngine;

namespace Scriptables
{
    public abstract class CoreScriptable<T> : ScriptableObject
    {
        [SerializeField] T data;
        public T Data { get { return data; } }
    }
}