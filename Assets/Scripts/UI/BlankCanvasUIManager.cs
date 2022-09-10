using UI.Panels;

namespace UI
{
    public class BlankCanvasUIManager : BaseCanvasUIManager<BasePanelUIManager>
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Show();
        }
    }
}