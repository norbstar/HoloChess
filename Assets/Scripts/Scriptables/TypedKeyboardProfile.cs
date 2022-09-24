using System.Collections.Generic;

using UnityEngine;

namespace Scriptables
{
    public abstract class TypedKeyboardProfile<T> : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] List<T> bindings;

        public List<T> GetBindings() => bindings;
    }
}