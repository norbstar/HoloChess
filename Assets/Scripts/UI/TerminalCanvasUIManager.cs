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
    public class TerminalCanvasUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TerminalPanelUIManager panel;
        public TerminalPanelUIManager Panel { get { return panel; } }
        [SerializeField] GameObject sphere;

        private bool isShown = false;
        public bool IsShown { get  { return isShown; } }

        private CanvasGroup canvasGroup;
        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;
        private RootResolver rootResolver;
        private GameObject root;
        public GameObject Root { get { return root; } }
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

            panel.EnableDragBar(sphere.activeSelf);
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

        public void Hide()
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
        }

        private void OnRaycastEvent(GameObject origin, RaycastHit hit)
        {
            if (!panel.DragBar.IsPointerDown) return;

            Vector3 offset = panel.transform.position - panel.DragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}