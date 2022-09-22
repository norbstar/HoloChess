using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace UI.Panels
{
    [RequireComponent(typeof(RootResolver))]
    public class StaticTerminalPanelUIManager : SelectedButtonGroupPanelUIManager
    {
        [Header("Components")]
        [SerializeField] ButtonGroupUIManager buttonGroupManager;
        public ButtonGroupUIManager ButtonGroupManager { get { return buttonGroupManager; } }
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] TextMeshProUGUI textUI;

        [Header("Config")]
        [SerializeField] float refreshInterval = 0.25f;

        private static string TOP_BUTTON = "Top Button";
        private static string BOTTOM_BUTTON = "Bottom Button";
        private static string CLEAR_PROGRESS_BUTTON = "Clear Progress Button";
        private static string LOCK_SWAP_BUTTON = "Lock Swap Button";

        private IDictionary<string, string> elements = new Dictionary<string, string>();
        private bool refresh;

        public override void Awake()
        {
            base.Awake();
            textUI.text = string.Empty;
        }

        // Start is called before the first frame update
        void Start() => StartCoroutine(MonitorLogs());

        public override void OnEnable()
        {
            base.OnEnable();
            Application.logMessageReceived += Log;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Application.logMessageReceived -= Log;
        }

        public void Clear()
        {
            elements.Clear();
            refresh = true;
        }

        private void Log(string logString, string stackTrace, LogType type)
        {
            refresh = true;

            switch (type)
            {
                case LogType.Log:
                case LogType.Warning:
                    LogMessage(logString);
                    break;

                case LogType.Error:
                case LogType.Exception:
                    LogMessageAndStackTrace(logString, stackTrace);
                    break;
            }
        }

        private void LogMessage(string logString)
        {
            string[] splitString = logString.Split(new[] { ':' }, 2);
            string key = splitString[0];
            string value = (splitString.Length > 1) ? splitString[1] : null;

            if (elements.ContainsKey(key))
            {
                elements[key] = value;
            }
            else
            {
                elements.Add(key, value);
            }
        }

        private void LogMessageAndStackTrace(string logString, string stackTrace)
        {
            string[] splitString = logString.Split(new[] { ':' }, 2);
            string key = splitString[0];
            string value = (splitString.Length > 1) ? $"{splitString[1]} {stackTrace}" : $"{stackTrace}";
            
            if (elements.ContainsKey(key))
            {
                elements[key] = value;
            }
            else
            {
                elements.Add(key, value);
            }
        }

        private IEnumerator MonitorLogs()
        {
            while (isActiveAndEnabled)
            {
                if (refresh)
                {
                    refresh = false;
                    textUI.text = string.Empty;

                    var sortedElements = from e in elements orderby e.Key select e;

                    foreach(KeyValuePair<string, string> element in sortedElements)
                    {
                        if (textUI.text.Length > 0)
                        {
                            textUI.text = $"{textUI.text}\n";
                        }

                        textUI.text = (element.Value != null) ? $"{textUI.text}{element.Key} : {element.Value}" : $"{textUI.text}{element.Key}";
                    }
                }

                yield return new WaitForSeconds(refreshInterval);
            }
        }

        protected override void OnSelectEvent(ButtonUIManager manager)
        {
            var name = manager.Button.name;
            // Debug.Log($"{Time.time} OnSelect {name}");

            if (name.Equals(TOP_BUTTON))
            {
                scrollRect.ScrollToTop();
            }
            else if (name.Equals(BOTTOM_BUTTON))
            {
                scrollRect.ScrollToBottom();
            }
            else if (name.Equals(CLEAR_PROGRESS_BUTTON))
            {
                Clear();
            }
        }
    }
}