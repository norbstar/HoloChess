// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    [RequireComponent(typeof(RootResolver))]
    public class TerminalCanvasUIManager : MonoBehaviour
    {
        private bool isShown = false;
        public bool IsShown { get  { return isShown; } }

        private CanvasGroup canvasGroup;
        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;
        private RootResolver rootResolver;
        private GameObject root;
        public GameObject Root { get { return root; } }

        void Awake()
        {
            ResolveDependencies();
            root = rootResolver.Root;
        }

        private void ResolveDependencies()
        {
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
            rootResolver = GetComponent<RootResolver>() as RootResolver;
        }

        public void Toggle()
        {
            if (canvasGroup.alpha == 0f)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void Show()
        {
#if UNITY_EDITOR
            raycaster.enabled = true;
#else
            trackedRaycaster.enabled = true;
#endif
            canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
#if UNITY_EDITOR
            raycaster.enabled = false;
#else
            trackedRaycaster.enabled = false;
#endif
        }
    }
}