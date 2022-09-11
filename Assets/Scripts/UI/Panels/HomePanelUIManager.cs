using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class HomePanelUIManager : ShortcutPanelUIManager, IDragbarPanel
    {
        [Header("Components")]
        [SerializeField] DragBarUIManager dragBar;
        public DragBarUIManager DragBar { get { return dragBar; } }
        [SerializeField] ButtonGroupUIManager buttonGroupManager;
        public ButtonGroupUIManager ButtonGroupManager { get { return buttonGroupManager; } }

        private static string SCENE_TOGGLE_BUTTON = "Scene Toggle Button";
        private static string TERMINAL_TOGGLE_BUTTON = "Terminal Toggle Button";
        private static string VOLUME_TOGGLE_BUTTON = "Volume Toggle Button";
        private static string SETTINGS_TOGGLE_BUTTON = "Settings Toggle Button";
        private static string EXIT_PROGESS_BUTTON = "Exit Progress Button";

        private AudioSourceModifier audioSourceModifier;
        private SceneCanvasUIManager sceneCanvasUIManager;
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
            sceneCanvasUIManager = FindObjectOfType<SceneCanvasUIManager>();
            terminalCanvasUIManager = FindObjectOfType<TerminalCanvasUIManager>();
            settingsCanvasUIManager = FindObjectOfType<SettingsCanvasUIManager>();
            rootResolver = GetComponent<RootResolver>() as RootResolver;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (sceneCanvasUIManager != null)
            {
                sceneCanvasUIManager.Panel.CloseEventReceived += OnSceneCloseEvent;
            }

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

            if (sceneCanvasUIManager != null)
            {
                sceneCanvasUIManager.Panel.CloseEventReceived -= OnSceneCloseEvent;
            }

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
            ButtonUIManager manager;

            if (TryResolveButtonByName("Scene Toggle Button", out manager))
            {
                if (((ToggleButtonUIManager) manager).IsOn)
                {
                    if (!sceneCanvasUIManager.IsShown)
                    {
                        sceneCanvasUIManager.Show();
                    }
                }
                else
                {
                    if (sceneCanvasUIManager.IsShown)
                    {
                        sceneCanvasUIManager.Hide();
                    }
                }
            }

            if (TryResolveButtonByName("Terminal Toggle Button", out manager))
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

            if (TryResolveButtonByName("Settings Toggle Button", out manager))
            {
                if (((ToggleButtonUIManager) manager).IsOn)
                {
                    if (!settingsCanvasUIManager.IsShown)
                    {
                        settingsCanvasUIManager.Show();
                    }
                }
                else
                {
                    if (settingsCanvasUIManager.IsShown)
                    {
                        settingsCanvasUIManager.Hide();
                    }
                }
            }
        }

        private void OnSceneCloseEvent()
        {
            sceneCanvasUIManager.Hide();

            if (TryResolveButtonByName(SCENE_TOGGLE_BUTTON, out ButtonUIManager manager))
            {
                ((ToggleButtonUIManager) manager).IsOn = false;
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

        public GameObject GetObject() => gameObject;

        public DragBarUIManager GetDragBar() => dragBar;

        public void EnableDragBar(bool enable) => dragBar.gameObject.SetActive(enable);
        
        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            // Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals(SCENE_TOGGLE_BUTTON))
            {
                sceneCanvasUIManager?.Toggle();
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