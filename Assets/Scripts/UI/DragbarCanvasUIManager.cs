using System;

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
        private RaycastNotifier leftHandNotifier;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
            dragBar = panel.GetDragBar();
        }

        private void ResolveDependencies() => homeCanvasUIManager = FindObjectOfType<HomeCanvasUIManager>() as HomeCanvasUIManager;

        void OnEnable() => dragBar.EventReceived += OnDragBarEvent;

        void OnDisable() => dragBar.EventReceived -= OnDragBarEvent;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (TryGet.XR.TryGetControllerWithCharacteristics(HandController.LeftHandCharacteristics, out HandController controller))
            {
                leftHandNotifier = controller.Notifier;
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

        private void OnDragBarEvent(DragBarUIManager manager, DragBarUIManager.Event @event)
        {
            switch (@event)
            {
                case DragBarUIManager.Event.OnPointerDown:
                    if (leftHandNotifier != null)
                    {
                        leftHandNotifier.EventReceived += OnRaycastEvent;
                    }
                    break;

                case DragBarUIManager.Event.OnPointerUp:
                    if (leftHandNotifier != null)
                    {
                        leftHandNotifier.EventReceived -= OnRaycastEvent;
                    }
                    break;
            }
        }

        protected override void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            if (!target.Equals(layer.gameObject)) return;

            Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
            transform.position = hit.point + offset;
        }
    }
}