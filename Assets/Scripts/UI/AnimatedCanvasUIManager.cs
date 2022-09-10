using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(TrackedDeviceGraphicRaycaster))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(RootResolver))]
    public abstract class AnimatedCanvasUIManager : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] AudioClip onRevealClip;
        [SerializeField] AudioClip onConcealClip;

        protected bool isShown = false;
        public bool IsShown { get  { return isShown; } }

        protected CanvasGroup canvasGroup;
        protected GameObject root;

        private GraphicRaycaster raycaster;
        private TrackedDeviceGraphicRaycaster trackedRaycaster;
        private RootResolver rootResolver;
        public GameObject Root { get { return root; } }
        private RaycastNotifier leftHandNotifier;
        private Animator animator;

        protected virtual void Awake()
        {
            ResolveDependencies();
            root = rootResolver.Root;
        }

        private void ResolveDependencies()
        {
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
            raycaster = GetComponent<GraphicRaycaster>() as GraphicRaycaster;
            trackedRaycaster = GetComponent<TrackedDeviceGraphicRaycaster>() as TrackedDeviceGraphicRaycaster;
            animator = GetComponent<Animator>() as Animator;
            rootResolver = GetComponent<RootResolver>() as RootResolver;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (TryGet.XR.TryGetControllerWithCharacteristics(HandController.LeftHandCharacteristics, out HandController controller))
            {
                leftHandNotifier = controller.Notifier;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (!isShown) return;
            
            OnUpdate();
        }

        protected abstract void OnUpdate();

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

        public virtual void Show()
        {
            if (leftHandNotifier != null)
            {
                leftHandNotifier.EventReceived += OnRaycastEvent;
            }

            if (onRevealClip != null)
            {
                AudioSource.PlayClipAtPoint(onRevealClip, Vector3.zero, 1.0f);
            }
#if UNITY_EDITOR
            raycaster.enabled = true;
#else
            trackedRaycaster.enabled = true;
#endif
            animator.SetTrigger("Show");
            isShown = true;
        }

        public virtual void Hide()
        {
            if (leftHandNotifier != null)
            {
                leftHandNotifier.EventReceived -= OnRaycastEvent;
            }

            if (onConcealClip != null)
            {
                AudioSource.PlayClipAtPoint(onConcealClip, Vector3.zero, 1.0f);
            }

            animator.SetTrigger("Hide");
            isShown = false;
#if UNITY_EDITOR
            raycaster.enabled = false;
#else
            trackedRaycaster.enabled = false;
#endif
        }

        protected abstract void OnRaycastEvent(GameObject source, Vector3 origin, Vector3 direction, RaycastHit hit);
    }
}