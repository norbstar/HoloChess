using System;

using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Extended Scriptable", menuName = "Keyboard UI/Tests/Extended Scriptable", order = 2)]
    public class ExtendedScriptable : BaseScriptable
    {
        [Serializable]
        public class ExtendedData : BaseScriptable.BaseData
        {
            public string description;
        }

        [SerializeField] ExtendedData extendedData;

        public override BaseData GetData() => extendedData;
    }
}