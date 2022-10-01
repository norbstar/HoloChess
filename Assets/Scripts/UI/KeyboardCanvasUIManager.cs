using System;
using System.Collections.Generic;

using UnityEngine;

using UI.Panels;

namespace UI
{
    [AddComponentMenu("UI/Keyboard Canvas UI Manager")]
    public class KeyboardCanvasUIManager : LockableCanvasUIManager<KeyboardPanelUIManager>
    {
        [Header("Components")]
        [SerializeField] List<BaseKeyboardMap> maps;

        public delegate void OnKeyEvent(string unicode, string character);
        public event OnKeyEvent EventReceived;

        protected override void Awake()
        {
            base.Awake();
            Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (BaseKeyboardMap map in maps)
            {
                Type type = map.GetType();

                if (type == typeof(KeyboardMap))
                {
                    ((KeyboardMap) map).EventReceived += OnMapButtonEvent;
                }
                else if (type == typeof(ExtendedKeyboardMap))
                {
                    ((ExtendedKeyboardMap) map).EventReceived += OnMapButtonEvent;
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (BaseKeyboardMap map in maps)
            {
                Type type = map.GetType();

                if (type == typeof(KeyboardMap))
                {
                    ((KeyboardMap) map).EventReceived -= OnMapButtonEvent;
                }
                else if (type == typeof(ExtendedKeyboardMap))
                {
                    ((ExtendedKeyboardMap) map).EventReceived -= OnMapButtonEvent;
                }
            }
        }

        private void OnMapButtonEvent(KeyBindingButtonUIManager manager)
        {
            var binding = manager.Binding;
            Debug.Log($"OnMapButtonEvent Id : {binding.id} Character : {binding.character}");

            EventReceived?.Invoke(binding.unicode, binding.character);
        }

        private void OnMapButtonEvent(ExtendedKeyBindingButtonUIManager manager)
        {
            var binding = manager.Binding;
            Debug.Log($"Id : {binding.id} Character : {binding.character} isMacro {binding.isMacro}");

            EventReceived?.Invoke(binding.unicode, binding.character);
        }
    }
}