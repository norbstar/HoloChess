using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UI
{
    [AddComponentMenu("UI/Basic Canvas UI Manager")]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    public class BasicCanvasUIManager : MonoBehaviour
    {
        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;

        void Awake()
        {
            ResolveDependencies();

#if UNITY_EDITOR
            raycaster.enabled = true;
#else
            trackedRaycaster.enabled = true;
#endif
        }

        private void ResolveDependencies()
        {
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
        }
    }
}