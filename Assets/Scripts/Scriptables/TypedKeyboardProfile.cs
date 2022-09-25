using System.Collections.Generic;

using UnityEngine;

namespace Scriptables
{
    public abstract class TypedKeyboardProfile<T> : KeyboardProfileScriptable where T : class
    {
        [Header("Config")]
        [SerializeField] List<T> bindings;

        public List<T> GetBindings() => bindings;

        void OnEnable()
        {
            foreach (T binding in bindings)
            {
                if (binding == null) continue;
                MapBinding(binding);
            }
        }

        public abstract void MapBinding(T binding);
    }
}