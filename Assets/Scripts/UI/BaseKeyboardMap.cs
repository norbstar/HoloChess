using UnityEngine;

namespace UI
{
    [AddComponentMenu("UI/Base Keyboard Map")]
    public abstract class BaseKeyboardMap : MonoBehaviour
    {
        public abstract void Configure();
    }
}