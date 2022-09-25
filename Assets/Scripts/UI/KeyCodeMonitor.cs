using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace UI
{
    public class KeyCodeMonitor : MonoBehaviour
    {
        private List<KeyBindingButtonUIManager> managers;

        void Awake()
        {
            managers = new List<KeyBindingButtonUIManager>();
            ResolveDependencies();
        }

        private void ResolveDependencies() => managers = GetComponentsInChildren<KeyBindingButtonUIManager>().ToList();

        void OnEnable()
        {
            foreach (KeyBindingButtonUIManager manager in managers)
            {
                manager.EventReceived += OnButtonEvent; 
            }
        }

        void OnDisable()
        {
            foreach (KeyBindingButtonUIManager button in managers)
            {
                button.EventReceived -= OnButtonEvent; 
            }
        }

        private void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event)
        {
            if (@event != ButtonUIManager.Event.OnSelect) return;

            KeyBindingButtonUIManager keyBindingManager = (KeyBindingButtonUIManager) manager;
            Debug.Log($"Id : {keyBindingManager.Id}");
        }
    }
}