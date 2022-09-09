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
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(RootResolver))]
    public class HomeCanvasUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] HomePanelUIManager panel;
        public HomePanelUIManager Panel { get { return panel; } }
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
        private float parentToChildMultiplier;
        private RaycastNotifier leftHandNotifier;
        private Animator animator;

        void Awake()
        {
            ResolveDependencies();

            root = rootResolver.Root;
            originalOffset = transform.localPosition.z;
            parentToChildMultiplier = panel.transform.localScale.x / transform.localScale.x;
        }

        private void ResolveDependencies()
        {
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
            rootResolver = GetComponent<RootResolver>() as RootResolver;
            animator = GetComponent<Animator>() as Animator;
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
            Debug.Log($"Scale : {transform.localScale.x}:{transform.localScale.y}:{transform.localScale.z}");
            Debug.Log($"Alpha : {canvasGroup.alpha}");
            Debug.Log($"Position {transform.position}");

            if (canvasGroup.alpha == 0f) return;
            
            Vector3 offset = transform.position - (root.transform.position + new Vector3(0f, sphere.transform.position.y, 0f));
            transform.LookAt(transform.position + offset);
        }

        public void Toggle()
        {
            if (!isShown)
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

            var ray = new Ray(camera.transform.position + camera.transform.forward * (originalOffset * parentToChildMultiplier), -camera.transform.forward);
            bool hasHit = Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, menuLayerMask);

            Vector3? spawnPoint = null;
            
            if (hasHit)
            {
                spawnPoint = hit.point;
                Debug.Log($"{Time.time} Spawn Point : {spawnPoint}");
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
            animator.SetTrigger("Show");
            isShown = true;
        }

        private void Hide()
        {
            if (leftHandNotifier != null)
            {
                leftHandNotifier.EventReceived -= OnRaycastEvent;
            }

            animator.SetTrigger("Hide");
            isShown = false;
#if UNITY_EDITOR
            raycaster.enabled = false;
#else
            trackedRaycaster.enabled = false;
#endif
            sphere.SetActive(false);
        }

        private void OnRaycastEvent(GameObject origin, RaycastHit hit)
        {
            if (!panel.DragBar.IsPointerDown) return;

            Vector3 offset = panel.transform.position - panel.DragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}