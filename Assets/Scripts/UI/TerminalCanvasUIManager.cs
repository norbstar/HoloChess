using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

using TMPro;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    [RequireComponent(typeof(RootResolver))]
    public class TerminalCanvasUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TextMeshProUGUI textUI;

        [Header("Config")]
        [SerializeField] float refreshInterval = 0.25f;

        private CanvasGroup canvasGroup;
        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;
        private RootResolver rootResolver;
        private GameObject root;
        public GameObject Root { get { return root; } }
        private IDictionary<string, string> elements = new Dictionary<string, string>();
        private bool refresh;

        void Awake()
        {
            ResolveDependencies();

            root = rootResolver.Root;
            textUI.text = string.Empty;
        }

        private void ResolveDependencies()
        {
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
            rootResolver = GetComponent<RootResolver>() as RootResolver;
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log($"Config.RefreshInterval:{refreshInterval} secs");
            StartCoroutine(MonitorLogs());
            Toggle();
        }

        void OnEnable() => Application.logMessageReceived += Log;

        void OnDisable() => Application.logMessageReceived -= Log;

        public void Toggle()
        {
            if (canvasGroup.alpha == 0f)
            {
#if UNITY_EDITOR
                raycaster.enabled = true;
#else
                trackedRaycaster.enabled = true;
#endif
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0f;
#if UNITY_EDITOR
                raycaster.enabled = false;
#else
                trackedRaycaster.enabled = false;
#endif
            }
        }

        public void Clear() => elements.Clear();

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
    }
}