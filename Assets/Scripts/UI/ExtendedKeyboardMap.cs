using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Scriptables;

namespace UI
{
    [AddComponentMenu("UI/Extended Keyboard Map")]
    public class ExtendedKeyboardMap : BaseKeyboardMap
    {
        [Header("Components")]
        [SerializeField] List<ExtendedKeyBindingButtonUIManager> buttons;

        [Header("Config")]
        [SerializeField] ExtendedKeyboardProfile profile;

        public delegate void OnMapButtonEvent(ExtendedKeyBindingButtonUIManager manager);
        public event OnMapButtonEvent EventReceived;

        void Awake()
        {
            ResolveDependencies();
            Configure();
        }

        private void ResolveDependencies() => buttons = GetComponentsInChildren<ExtendedKeyBindingButtonUIManager>().ToList();

        public override void Configure()
        {
            int id = 0;

            List<ExtendedKeyboardProfile.ExtendedKeyboardBinding> bindings = profile.GetBindings();

            foreach (ExtendedKeyBindingButtonUIManager button in buttons)
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
         
            EventReceived?.Invoke((ExtendedKeyBindingButtonUIManager) manager);
        }
    }
}