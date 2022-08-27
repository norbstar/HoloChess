using UnityButton = UnityEngine.UI.Button;

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
        public static event OnClickEvent EventReceived;

        public override void OnClickButton(ButtonContainer container)
        {
            base.OnClickButton(container);

            var name = container.button.name;

            if (name.Equals("Game Button"))
            {
                EventReceived?.Invoke(Identity.Game);
            }
            else if (name.Equals("Settings Button"))
            {
                EventReceived?.Invoke(Identity.Settings);
            }
            else if (name.Equals("About Button"))
            {
                EventReceived?.Invoke(Identity.About);
            }
            else if (name.Equals("Exit Button"))
            {
                EventReceived?.Invoke(Identity.Exit);
            }
        }
    }
}