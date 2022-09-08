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

        // private static string SCENE_BUTTON = "Scene Button";
        // private static string TERMINAL_TOGGLE_BUTTON = "Terminal Toggle Button";
        // private static string VOLUME_TOGGLE_BUTTON = "Volume Toggle Button";
        // private static string SETTINGS_BUTTON = "Settings Button";
        // private static string EXIT_PROGESS_BUTTON = "Exit Progress Button";

        private AudioSourceModifier audioSourceModifier;
        private TerminalCanvasUIManager terminalCanvasUIManager;
        private SettingsCanvasUIManager settingsCanvasUIManager;
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
            settingsCanvasUIManager = FindObjectOfType<SettingsCanvasUIManager>();
            rootResolver = GetComponent<RootResolver>() as RootResolver;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (terminalCanvasUIManager != null)
            {
                terminalCanvasUIManager.Panel.CloseEventReceived += OnTerminalCloseEvent;
            }

            if (settingsCanvasUIManager != null)
            {
                settingsCanvasUIManager.Panel.CloseEventReceived += OnSettingsCloseEvent;
            }
        }

        public override void OnDisable()
        {
            base.OnEnable();

            if (terminalCanvasUIManager != null)
            {
                terminalCanvasUIManager.Panel.CloseEventReceived -= OnTerminalCloseEvent;
            }

            if (settingsCanvasUIManager != null)
            {
                settingsCanvasUIManager.Panel.CloseEventReceived -= OnSettingsCloseEvent;
            }
        }

        private void ConfigButtons()
        {
            if (terminalCanvasUIManager == null) return;

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

        private void OnSettingsCloseEvent() => settingsCanvasUIManager.Hide();

        private void OnTerminalCloseEvent()
        {
            terminalCanvasUIManager.Hide();

            if (TryResolveButtonByName("Terminal Toggle Button", out ButtonUIManager manager))
            {
                ((ToggleButtonUIManager) manager).IsOn = false;
            }
        }
        
        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            // Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals("Terminal Toggle Button"))
            {
                terminalCanvasUIManager?.Toggle();
            }
            if (name.Equals("Settings Button"))
            {
                settingsCanvasUIManager?.Toggle();
            }
            else if (name.Equals("Exit Progress Button"))
            {
                Application.Quit();
            }
        }
    }
}