namespace UI
{
    public class ShortcutPanelUIManager : ButtonGroupPanelUIManager
    {
        public enum Identity
        {
            Game,
            Settings,
            About,
            Exit
        }

        public delegate void OnSelectEvent(Identity identity);
        public event OnSelectEvent ClickEventReceived;

        protected override void OnButtonEvent(ButtonUIManager manager, ButtonUIManager.Event @event)
        {
            if (@event != ButtonUIManager.Event.OnSelect) return;

            var name = manager.Button.name;

            if (name.Equals("Game Button"))
            {
                ClickEventReceived?.Invoke(Identity.Game);
            }
            else if (name.Equals("Settings Button"))
            {
                ClickEventReceived?.Invoke(Identity.Settings);
            }
            else if (name.Equals("About Button"))
            {
                ClickEventReceived?.Invoke(Identity.About);
            }
            else if (name.Equals("Exit Button"))
            {
                ClickEventReceived?.Invoke(Identity.Exit);
            }
        }
    }
}