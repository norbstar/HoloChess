using System.Collections.Generic;

using UnityEngine;

namespace UI
{
    public abstract class BaseButtonGroupPanelUIManager : MonoBehaviour
    {
        public class ButtonAccessor
        {
            public ButtonUIManager manager;
            public Vector3 originalScale;

            public ButtonAccessor(ButtonUIManager manager)
            {
                this.manager = manager;
                originalScale = manager.Button.transform.localScale;
            }
        }

        protected List<ButtonAccessor> accessors;

        void Awake() => accessors = ResolveAccessors();

        protected abstract List<ButtonAccessor> ResolveAccessors();

        void OnEnable()
        {
            foreach (ButtonAccessor container in accessors)
            {
                container.manager.EventReceived += OnButtonEvent;
            }
        }

        void OnDisable()
        {
            foreach (ButtonAccessor container in accessors)
            {
                container.manager.EventReceived -= OnButtonEvent;
            }
        }

        protected abstract void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event);

        public void Reset()
        {
            foreach (ButtonAccessor container in accessors)
            {
                container.manager.Reset();
            }
        }
    }
}