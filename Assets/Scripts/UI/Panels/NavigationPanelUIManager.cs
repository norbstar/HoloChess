using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class NavigationPanelUIManager : ShortcutPanelUIManager
    {
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

        private void ResolveDependencies()
        {
            audioSourceModifier = FindObjectOfType<AudioSourceModifier>();
            terminalCanvasUIManager = FindObjectOfType<TerminalCanvasUIManager>();
            rootResolver = GetComponent<RootResolver>() as RootResolver;
        }

        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals("Scene Button"))
            {
                // TODO
            }
            else if (name.Equals("Terminal Toggle Button"))
            {
                terminalCanvasUIManager.Toggle();
            }
            else if (name.Equals("Volume Toggle Button"))
            {
                audioSourceModifier.Volume = (((ToggleButtonUIManager) manager).IsOn) ? 1f : 0f;
            }
            else if (name.Equals("Settings Button"))
            {
                // TODO
            }
            else if (name.Equals("Exit Button"))
            {
                Application.Quit();
            }
        }
    }
}