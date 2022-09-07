using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class NavigationPanelUIManager : ShortcutPanelUIManager
    {
        [Header("Components")]
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }
        [SerializeField] ButtonGroupUIManager buttonGroupManager;
        public ButtonGroupUIManager ButtonGroupManager { get { return buttonGroupManager; } }

        private AudioSourceModifier audioSourceModifier;
        private TerminalCanvasUIManager terminalCanvasUIManager;
        private RootResolver rootResolver;

        public override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        // Start is called before the first frame update
        void Start() => ConfigButtons();

        private void ResolveDependencies()
        {
            audioSourceModifier = FindObjectOfType<AudioSourceModifier>();
            terminalCanvasUIManager = FindObjectOfType<TerminalCanvasUIManager>();
            rootResolver = GetComponent<RootResolver>() as RootResolver;
        }

        private void ConfigButtons()
        {
            if (TryResolveButtonByName("Terminal Toggle Button", out ButtonUIManager manager))
            {
                if (((ToggleButtonUIManager) manager).IsOn)
                {
                    terminalCanvasUIManager.Show();
                }
                else
                {
                    terminalCanvasUIManager.Hide();
                }
            }
        }

        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            // Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals("Terminal Toggle Button"))
            {
                terminalCanvasUIManager.Toggle();
            }
            else if (name.Equals("Exit Button"))
            {
                Application.Quit();
            }
        }
    }
}