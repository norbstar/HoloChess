using System;

using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Base Scriptable", menuName = "Keyboard UI/Tests/Base Scriptable", order = 2)]
    public class BaseScriptable : ScriptableObject
    {
        [Serializable]
        public class BaseData
        {
            public int id;
        }

        [SerializeField] BaseData data;

        // public BaseData Data { get { return data; } }

        public virtual BaseData GetData() => data;
    }
}