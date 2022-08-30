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

        public delegate void OnClickEvent(Identity identity);
        public event OnClickEvent ClickEventReceived;

        public override void OnClickButton(ButtonAccessor container)
        {
            base.OnClickButton(container);

            var name = container.manager.Button.name;

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