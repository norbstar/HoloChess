using UnityEngine;

namespace UI.Panels
{
    [AddComponentMenu("UI/Panels/Selected Button Group Panel UI Manager")]
    public abstract class SelectedButtonGroupPanelUIManager : ButtonGroupPanelUIManager
    {
        protected abstract void OnSelectEvent(ButtonUIManager manager);

        protected override void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event)
        {
            if (@event != ButtonUIManager.Event.OnSelect) return;

            OnSelectEvent(manager);
        }
    }
}