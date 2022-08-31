using System.Collections.Generic;

using UnityEngine;

namespace UI.Panels
{
    public abstract class BaseButtonGroupPanelUIManager : MonoBehaviour
    {
        protected List<ButtonUIManager> instances;

        void Awake() => instances = ResolveInstances();

        protected abstract List<ButtonUIManager> ResolveInstances();

        void OnEnable()
        {
            foreach (var instance in instances)
            {
                instance.EventReceived += OnButtonEvent;
            }
        }

        void OnDisable()
        {
            foreach (var instance in instances)
            {
                instance.EventReceived -= OnButtonEvent;
            }
        }

        protected abstract void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event);

        public void Reset()
        {
            foreach (var instance in instances)
            {
                instance.Reset();
            }
        }
    }
}