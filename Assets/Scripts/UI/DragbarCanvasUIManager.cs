using System.Collections.Generic;

using UnityEngine;

namespace UI
{
    public abstract class DragbarCanvasUIManager<T> : AnimatedCanvasUIManager where  T : IDragbarPanel
    {
        [Header("Components")]
        [SerializeField] protected T panel;
        public T Panel { get { return panel; } }
        [SerializeField] protected MenuLayer layer;
        public MenuLayer Layer { get { return layer; } }
        protected DragBarUIManager dragBar;

        private HomeCanvasUIManager homeCanvasUIManager;
        private RaycastNotifier leftHandNotifier, rightHandNotifier;
        private bool isPointerDown;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
            dragBar = panel.GetDragBar();

            if (layer != null)
            {
                transform.position = new Vector3(
                    layer.transform.position.x,
                    layer.transform.position.y,
                    layer.transform.position.z + (layer.transform.localScale.z * 0.5f)
                );
            }
        }

        private void ResolveDependencies() => homeCanvasUIManager = FindObjectOfType<HomeCanvasUIManager>() as HomeCanvasUIManager;

        protected virtual void OnEnable() => dragBar.EventReceived += OnDragBarEvent;

        protected virtual void OnDisable() => dragBar.EventReceived -= OnDragBarEvent;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (TryGet.XR.TryGetControllers(out List<HandController> controllers))
            {
                foreach (HandController controller in controllers)
                {
                    if (controller.gameObject.activeSelf)
                    {
                        switch (controller.WhichHand)
                        {
                            case HandController.Hand.Left:
                                leftHandNotifier = controller.Notifier;
                                break;

                            case HandController.Hand.Right:
                                rightHandNotifier = controller.Notifier;
                                break;
                        }
                    }
                }
            }
        }

        protected void LookAtRoot()
        {
            Vector3 offset = transform.position - layer.transform.position;
            transform.LookAt(transform.position + offset);
        }

        protected override void OnUpdate()
        {
            if (isShown)
            {
                LookAtRoot();
            }

            panel.EnableDragBar(homeCanvasUIManager.Layer.IsShown);
        }

        public override void Show()
        {
            layer.gameObject.SetActive(true);
            layer.Attach(gameObject);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            layer.Detach(gameObject);

            if (!layer.HasChildren)
            {
                layer.gameObject.SetActive(false);
            }
        }

        private void OnDragBarEvent(DragBarUIManager manager, DragBarUIManager.Event @event, GameObject source)
        {
            HandController controller = source?.GetComponent<HandController>() as HandController;
            if (controller == null || !controller.gameObject.activeSelf) return;

            switch (@event)
            {
                case DragBarUIManager.Event.OnPointerDown:
                    switch (controller.WhichHand)
                    {
                        case HandController.Hand.Left:
                            leftHandNotifier.EventReceived += OnRaycastEvent;
                            rightHandNotifier.EventReceived -= OnRaycastEvent;
                            break;

                        case HandController.Hand.Right:
                            rightHandNotifier.EventReceived += OnRaycastEvent;
                            leftHandNotifier.EventReceived -= OnRaycastEvent;
                            break;
                    }

                    isPointerDown = true;
                    break;

                case DragBarUIManager.Event.OnPointerUp:
                    switch (controller.WhichHand)
                    {
                        case HandController.Hand.Left:
                            leftHandNotifier.EventReceived -= OnRaycastEvent;
                            break;

                        case HandController.Hand.Right:
                            rightHandNotifier.EventReceived -= OnRaycastEvent;
                            break;
                    }

                    isPointerDown = false;
                    break;
            }
        }

        protected override void OnRaycastEvent(GameObject source, List<RaycastNotifier.HitInfo> hits)
        {
            if (!isPointerDown) return;

            foreach (RaycastNotifier.HitInfo hitInfo in hits)
            {
                var target = hitInfo.hit.transform.gameObject;

                if (GameObject.ReferenceEquals(target, layer.gameObject))
                {
                    ProcessRaycastEvent(source, hitInfo.origin, hitInfo.direction, target, hitInfo.hit);
                }
            }
        }

        protected virtual void ProcessRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            Vector3 relativeDirection = (hit.point - layer.transform.position).normalized;
            var point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);
            Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
            transform.position = point/* + offset*/;
            
            PointProjectorDatabase.PlotPoint($"Target Point", $"Target Point", PointProjector.Type.Purple, transform.position);
            float distance = Vector3.Distance(layer.transform.position, transform.position);
            Debug.Log($"<color=purple>Target Point</color> : {point} {offset} {transform.position} {distance}");
        }
    }
}