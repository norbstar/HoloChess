using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

using UI.Panels;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    [RequireComponent(typeof(RootResolver))]
    public class MenuCanvasUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] NavigationPanelUIManager navigationManager;
        [SerializeField] GameObject sphere;

        [Header("Audio")]
        [SerializeField] AudioClip onRevealClip;

        [Header("Config")]
        [SerializeField] GameObject referencePrefab;

        [Serializable]
        public class Reference
        {
            public string name;
            public GameObject gameObject;
        }

        private bool isShown = false;
        public bool IsShown { get { return isShown; } }

        private CanvasGroup canvasGroup;
        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;
        private RootResolver rootResolver;
        private GameObject root;
        public GameObject Root { get { return root; } }
        private new Camera camera;
        public Camera Camera { get { return camera; } }
        private float originalOffset;
        private RaycastNotifier leftHandNotifier;

        void Awake()
        {
            ResolveDependencies();

            root = rootResolver.Root;
            originalOffset = transform.localPosition.z;
        }

        private void ResolveDependencies()
        {
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
            rootResolver = GetComponent<RootResolver>() as RootResolver;
            camera = Camera.main;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (TryGet.XR.TryGetControllerWithCharacteristics(HandController.LeftHandCharacteristics, out HandController controller))
            {
                leftHandNotifier = controller.Notifier;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (canvasGroup.alpha == 0f) return;
            
            Vector3 offset = transform.position - (root.transform.position + new Vector3(0f, sphere.transform.position.y, 0f));
            transform.LookAt(transform.position + offset);
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

        private void Show()
        {
            root.transform.position = new Vector3(camera.transform.position.x, 0f, camera.transform.position.z);
            sphere.SetActive(true);

            LayerMask menuLayerMask = LayerMask.GetMask("Menu");

            float parentToChildMultiplier = navigationManager.transform.localScale.x / transform.localScale.x;
            var ray = new Ray(camera.transform.position + camera.transform.forward * (originalOffset * parentToChildMultiplier), -camera.transform.forward);
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, menuLayerMask);

            Vector3? spawnPoint = null;
            
            if (hasHit)
            {
                spawnPoint = hit.point;
                transform.position = spawnPoint.Value;
            }

            if (onRevealClip != null)
            {
                AudioSource.PlayClipAtPoint(onRevealClip, Vector3.zero, 1.0f);
            }

            if (leftHandNotifier != null)
            {
                leftHandNotifier.EventReceived += OnRaycastEvent;
            }
#if UNITY_EDITOR
            raycaster.enabled = true;
#else
            trackedRaycaster.enabled = true;
#endif
            canvasGroup.alpha = 1f;
        }

        private void Hide()
        {
            if (leftHandNotifier != null)
            {
                leftHandNotifier.EventReceived -= OnRaycastEvent;
            }

            canvasGroup.alpha = 0f;
#if UNITY_EDITOR
            raycaster.enabled = false;
#else
            trackedRaycaster.enabled = false;
#endif
            sphere.SetActive(false);
        }

        private void OnRaycastEvent(GameObject origin, RaycastHit hit)
        {
            if (!navigationManager.DragBar.IsPointerDown) return;

            Vector3 offset = navigationManager.transform.position - navigationManager.DragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}