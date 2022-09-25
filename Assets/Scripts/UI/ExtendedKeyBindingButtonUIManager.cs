using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Scriptables;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ExtendedKeyBindingButtonUIManager : ButtonUIManager
    {
        [Header("Custom Components")]
        [SerializeField] TextMeshProUGUI textUI;

        [Header("Config")]
        [SerializeField] int id;
        public int Id { get { return id; } set { id = value; } }

        private ExtendedKeyboardProfile.ExtendedKeyboardBinding binding;
        public ExtendedKeyboardProfile.ExtendedKeyboardBinding Binding { get { return binding; } }

        public void AssignBinding(ExtendedKeyboardProfile.ExtendedKeyboardBinding binding)
        {
            this.binding = binding;

            Debug.Log($"Id : {binding.id} Character : {binding.character} isMacro {binding.isMacro}");
            textUI.text = binding.character;
        }
    }
}