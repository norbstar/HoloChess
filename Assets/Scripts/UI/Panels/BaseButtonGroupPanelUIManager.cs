using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace UI.Panels
{
    [AddComponentMenu("UI/Panels/Base Button Group Panel UI Manager")]
    public abstract class BaseButtonGroupPanelUIManager : MonoBehaviour
    {
        protected List<ButtonUIManager> instances;

        public virtual void Awake() => instances = ResolveInstances();

        protected abstract List<ButtonUIManager> ResolveInstances();

        public virtual void OnEnable()
        {
            foreach (var instance in instances)
            {
                instance.EventReceived += OnButtonEvent;
            }
        }

        public virtual void OnDisable()
        {
            foreach (var instance in instances)
            {
                instance.EventReceived -= OnButtonEvent;
            }
        }

        protected abstract void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event);

        protected ButtonUIManager ResolveButton(string name) => (instances != null) ? instances.FirstOrDefault(b => b.gameObject.name.Equals(name)) : null;
    }
}