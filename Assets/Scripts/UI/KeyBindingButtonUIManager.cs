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

        private KeyboardBinding binding;
        public KeyboardBinding Binding { get { return binding; } }

        public void AssignBinding(KeyboardBinding binding)
        {
            this.binding = binding;

            Debug.Log($"Id : {binding.id} Character : {binding.character}");
            textUI.text = binding.label;
        }
    }
}