using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    public class MenuCanvasUIManager : CachedObject<MenuCanvasUIManager>
    {
        [SerializeField] GameObject referencePrefab;

        private CanvasGroup canvasGroup;
        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;
        private new Camera camera;
        private GameObject reference;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        private void ResolveDependencies()
        {
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
            camera = Camera.main;
        }

        public void Toggle()
        {
            if (canvasGroup.alpha == 0f)
            {
                Vector3 spawnPoint = camera.transform.position + camera.transform.forward * 5f;
                reference = Instantiate(referencePrefab, spawnPoint, Quaternion.identity);
                transform.position = reference.transform.position;
                Vector3 offset = transform.position - camera.transform.position;
                transform.LookAt(transform.position + offset);
                trackedRaycaster.enabled = raycaster.enabled = true;
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0f;
                trackedRaycaster.enabled = raycaster.enabled = false;
                Destroy(reference);
            }
        }
    }
}