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
                        Debug.Log($"{gameObject.name} OnDragBarEvent : Event : {@event}");
                        leftHandNotifier.EventReceived += OnRaycastEvent;
                    }
                    break;

                case DragBarUIManager.Event.OnPointerUp:
                    if (leftHandNotifier != null)
                    {
                        Debug.Log($"{gameObject.name} OnDragBarEvent : Event : {@event}");
                        leftHandNotifier.EventReceived -= OnRaycastEvent;
                    }
                    break;
            }
        }

        protected override void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit[] hits)
        {
            Debug.Log($"{gameObject.name} OnRaycastEvent [1]");
            
            foreach (RaycastHit hit in hits)
            {
                var target = hit.transform.gameObject;
                Debug.Log($"{gameObject.name} OnRaycastEvent [2] Target : {target.name} Layer : {layer.gameObject.name}");

                if (GameObject.ReferenceEquals(target, layer.gameObject))
                {
                    Debug.Log($"{gameObject.name} OnRaycastEvent [3] Target Pertains To Layer : {layer.gameObject.name}");
                    ProcessRaycastEvent(source, origin, direction, target, hit);
                }
            }
        }

        protected virtual void ProcessRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, GameObject target, RaycastHit hit)
        {
            Debug.Log($"{gameObject.name} ProcessRaycastEvent Default Impl");

            Vector3 relativeDirection = (hit.point - layer.transform.position).normalized;
            var point = layer.transform.position + relativeDirection * (layer.transform.localScale.z * 0.5f);
            Vector3 offset = panel.GetObject().transform.position - dragBar.transform.position;
            transform.position = point + offset;
        }
    }
}