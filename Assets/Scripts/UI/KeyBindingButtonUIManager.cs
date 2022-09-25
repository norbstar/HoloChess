using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Scriptables;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class KeyBindingButtonUIManager : ButtonUIManager
    {
        [Header("Custom Components")]
        [SerializeField] TextMeshProUGUI textUI;

        [Header("Config")]
        [SerializeField] int id;
        public int Id { get { return id; } set { id = value; } }

        public void AssignBinding(KeyboardProfileScriptable scriptable)
        {
            Type type = scriptable.GetType();

            if (type == typeof(KeyboardProfile))
            {
                KeyboardProfile profile = (KeyboardProfile) scriptable;
                List<KeyboardBinding> bindings = profile.GetBindings();

                foreach (KeyboardBinding binding in bindings)
                {
                    Debug.Log($"Id : {binding.id} Character : {binding.character}");
                }
            }
            else if (type == typeof(ExtendedKeyboardProfile))
            {
                ExtendedKeyboardProfile profile = (ExtendedKeyboardProfile) scriptable;
                List<ExtendedKeyboardProfile.ExtendedKeyboardBinding> bindings = profile.GetBindings();
                
                foreach (ExtendedKeyboardProfile.ExtendedKeyboardBinding binding in bindings)
                {
                    Debug.Log($"Id : {binding.id} Character : {binding.character} isMacro {binding.isMacro}");
                }
            }
        }
    }
}