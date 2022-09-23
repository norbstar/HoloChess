using UI.Panels;

namespace UI
{
    public class KeyboardCanvasUIManager : LockableCanvasUIManager<KeyboardPanelUIManager>
    {
        protected override void Awake()
        {
            base.Awake();
            Show();
        }
    }
}