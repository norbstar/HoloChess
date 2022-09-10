using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class HomePanelUIManager : ShortcutPanelUIManager
    {
        [Header("Components")]
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }
        [SerializeField] ButtonGroupUIManager buttonGroupManager;
        public ButtonGroupUIManager ButtonGroupManager { get { return buttonGroupManager; } }

        private static string SCENE_BUTTON = "Scene Button";
        private static string TERMINAL_TOGGLE_BUTTON = "Terminal Toggle Button";
        private static string VOLUME_TOGGLE_BUTTON = "Volume Toggle Button";
        private static string SETTINGS_TOGGLE_BUTTON = "Settings Toggle Button";
        private static string BLANK_TOGGLE_BUTTON = "Blank Toggle Button";
        private static string EXIT_PROGESS_BUTTON = "Exit Progress Button";

        private AudioSourceModifier audioSourceModifier;
        private TerminalCanvasUIManager terminalCanvasUIManager;
        private SettingsCanvasUIManager settingsCanvasUIManager;
        private BlankCanvasUIManager blankCanvasUIManager;
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
            blankCanvasUIManager = FindObjectOfType<BlankCanvasUIManager>();
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

            if (blankCanvasUIManager != null)
            {
                blankCanvasUIManager.Panel.CloseEventReceived += OnBlankCloseEvent;
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

            if (blankCanvasUIManager != null)
            {
                blankCanvasUIManager.Panel.CloseEventReceived -= OnBlankCloseEvent;
            }
        }

        private void ConfigButtons()
        {
            if (terminalCanvasUIManager == null) return;

            if (TryResolveButtonByName("Terminal Toggle Button", out ButtonUIManager manager))
            {
                if (((ToggleButtonUIManager) manager).IsOn)
                {
                    if (!terminalCanvasUIManager.IsShown)
                    {
                        terminalCanvasUIManager.Show();
                    }
                }
                else
                {
                    if (terminalCanvasUIManager.IsShown)
                    {
                        terminalCanvasUIManager.Hide();
                    }
                }
            }
        }

        private void OnTerminalCloseEvent()
        {
            terminalCanvasUIManager.Hide();

            if (TryResolveButtonByName(TERMINAL_TOGGLE_BUTTON, out ButtonUIManager manager))
            {
                ((ToggleButtonUIManager) manager).IsOn = false;
            }
        }

        private void OnSettingsCloseEvent()
        {
            settingsCanvasUIManager.Hide();

            if (TryResolveButtonByName(SETTINGS_TOGGLE_BUTTON, out ButtonUIManager manager))
            {
                ((ToggleButtonUIManager) manager).IsOn = false;
            }
        }

        private void OnBlankCloseEvent()
        {
            blankCanvasUIManager.Hide();

            if (TryResolveButtonByName(BLANK_TOGGLE_BUTTON, out ButtonUIManager manager))
            {
                ((ToggleButtonUIManager) manager).IsOn = false;
            }
        }
        
        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            // Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals(SCENE_BUTTON))
            {
                // TODO
            }
            else if (name.Equals(TERMINAL_TOGGLE_BUTTON))
            {
                terminalCanvasUIManager?.Toggle();
            }
            else if (name.Equals(SETTINGS_TOGGLE_BUTTON))
            {
                settingsCanvasUIManager?.Toggle();
            }
            else if (name.Equals(EXIT_PROGESS_BUTTON))
            {
                Application.Quit();
            }
        }
    }
}