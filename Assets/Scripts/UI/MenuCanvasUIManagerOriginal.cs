using System;
using System.Linq;
using System.Collections.Generic;

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
    public class MenuCanvasUIManagerOriginal : CachedObject<MenuCanvasUIManagerOriginal>
    {
        [Header("Components")]
        [SerializeField] NavigationPanelUIManager navigationManager;
        [SerializeField] GameObject sphere;

        [Header("Config")]
        [SerializeField] GameObject referencePrefab;
        [SerializeField] GameObject raycastPrefab;

        private static float parentToChildMultiplier = 10f;

        [Serializable]
        public class Reference
        {
            public string name;
            public GameObject gameObject;
        }

        private CanvasGroup canvasGroup;
        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;
        // private CollisionDetector collisionDetector;
        // private HandController leftHandController;
        private RootResolver rootResolver;
        private GameObject root;
        public GameObject Root { get { return root; } }
        private new Camera camera;
        public Camera Camera { get { return camera; } }
        private List<Reference> references;
        private float originalOffset;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();

            root = rootResolver.Root;
            originalOffset = transform.localPosition.z;
            references = new List<Reference>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // if (TryGet.TryGetControllerWithCharacteristics(HandController.LeftHandCharacteristics, out HandController controller))
            // {
            //     leftHandController = controller;
            // }

            // sphere.transform.localScale = Vector3.one * originalOffset * parentToChildMultiplier;
            // sphere.transform.localScale = Vector3.one * originalOffset;
        }

        private void ResolveDependencies()
        {
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
            // collisionDetector = sphere.GetComponent<CollisionDetector>() as CollisionDetector;
            rootResolver = GetComponent<RootResolver>() as RootResolver;
            camera = Camera.main;
        }

        // void OnEnable() => collisionDetector.EventReceived += OnCollisionEvent;

        // Update is called once per frame
        void Update()
        {
            if (canvasGroup.alpha == 0f) return;
            
            // Debug.Log($"Camera Position : {camera.transform.position}");
            // Debug.Log($"Origin Position : {root.transform.position}");
            // Debug.Log($"Menu Position : {transform.position}");
            // Debug.Log($"Sphere Position : {sphere.transform.position}");

            Vector3 offset = transform.position - root.transform.position;
            transform.LookAt(transform.position + offset);
        }

        void OnDisable()
        {
            // collisionDetector.EventReceived -= OnCollisionEvent;
            
            foreach (Reference reference in references)
            {
                Destroy(reference.gameObject);
            }

            references.Clear();
        }

        public void Toggle()
        {
            if (canvasGroup.alpha == 0f)
            {
                root.transform.position = camera.transform.position;

                Reference reference = null;
                reference = references.FirstOrDefault(r => r.name.Equals("Origin"));

                if (reference == null)
                {
                    reference = new Reference
                    {
                        name = "Origin",
                        gameObject = Instantiate(referencePrefab, root.transform.position, Quaternion.identity)
                    };

                    reference.gameObject.name = reference.name;
                    references.Add(reference);
                }
                else
                {
                    reference.gameObject.transform.position = root.transform.position;
                }

                Vector3 spawnPoint = camera.transform.position + camera.transform.forward * originalOffset;

                reference = references.FirstOrDefault(r => r.name.Equals("Canvas"));

                if (reference == null)
                {
                    reference = new Reference
                    {
                        name = "Canvas",
                        gameObject = Instantiate(referencePrefab, spawnPoint, Quaternion.identity)
                    };

                    reference.gameObject.name = reference.name;
                    references.Add(reference);
                }
                else
                {
                    reference.gameObject.transform.position = spawnPoint;
                }

                transform.position = reference.gameObject.transform.position;

                // sphere.transform.position = root.transform.position;
                // sphere.SetActive(true);

                // if (leftHandController != null)
                // {
                //     leftHandController.RaycastEventReceived += OnRaycastEvent;
                // }

                Debug.Log($"Init Camera Position : {camera.transform.position}");
                Debug.Log($"Init Origin Position : {root.transform.position}");
                Debug.Log($"Init Menu Position : {transform.position}");
                // Debug.Log($"Init Sphere Position : {sphere.transform.position}");

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
                // sphere.SetActive(false);

                // if (leftHandController != null)
                // {
                //     leftHandController.RaycastEventReceived -= OnRaycastEvent;
                // }
            }
        }

        // private void OnCollisionEvent(GameObject source, CollisionDetector.Event @event, Vector3? nearestPoint)
        // {
        //     if (!navigationManager.DragBar.IsPointerDown) return;
        //     root.transform.position = nearestPoint.Value;
        // }

        private void OnRaycastEvent(HandController controller, GameObject source, Vector3 point)
        {
            if (!navigationManager.DragBar.IsPointerDown) return;

            if (controller.WhichHand == HandController.Hand.Left)
            {
                root.transform.position = point;
            }
        }

// #if UNITY_EDITOR
//         public void OnDrawGizmos()
//         {
//             if (canvasGroup.alpha != 1f) return;
//             Gizmos.DrawSphere(root.transform.position, originalOffset * 0.5f);
//         }
// #endif
    }
}