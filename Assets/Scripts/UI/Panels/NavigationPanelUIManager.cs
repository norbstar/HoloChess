using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class NavigationPanelUIManager : ShortcutPanelUIManager
    {
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }
        
        private RootResolver rootResolver;

        public override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        private void ResolveDependencies() => rootResolver = GetComponent<RootResolver>() as RootResolver;

        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;

            if (name.Equals("Exit Button"))
            {
                Application.Quit();
            }
        }
    }
}