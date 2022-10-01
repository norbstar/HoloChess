using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Scriptables;

namespace UI
{
    [AddComponentMenu("UI/Keyboard Map")]
    public class KeyboardMap : BaseKeyboardMap
    {
        [Header("Components")]
        [SerializeField] List<KeyBindingButtonUIManager> buttons;

        [Header("Config")]
        [SerializeField] KeyboardProfile profile;

        public delegate void OnMapButtonEvent(KeyBindingButtonUIManager manager);
        public event OnMapButtonEvent EventReceived;

        void Awake()
        {
            ResolveDependencies();
            Configure();
        }

        private void ResolveDependencies() => buttons = GetComponentsInChildren<KeyBindingButtonUIManager>().ToList();

        public override void Configure()
        {
            int id = 0;

            List<KeyboardBinding> bindings = profile.GetBindings();

            foreach (KeyBindingButtonUIManager button in buttons)
            {
                button.Id = ++id;

                var binding = bindings.FirstOrDefault(b => b.id == button.Id);
                
                if (binding != null)
                {
                    button.AssignBinding(binding);
                }

                button.EventReceived += OnButtonEvent;
            }
        }

        private void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event)
        {
            if (@event != ButtonUIManager.Event.OnSelect) return;
         
            EventReceived?.Invoke((KeyBindingButtonUIManager) manager);
        }
    }
}