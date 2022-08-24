using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(CanvasGroupFader))]
    public class NavCanvasManager : CachedObject<SceneNavigator>
    {
        [Header("Config")]
        [SerializeField] InputAction inputAction;

        private CanvasGroupFader canvasGroupFader;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        private void ResolveDependencies() => canvasGroupFader = GetComponent<CanvasGroupFader>() as CanvasGroupFader;

        void OnEnable()
        {
            inputAction.Enable();
            inputAction.performed += OnEscape;
        }

        void OnDisable()
        {
            inputAction.Disable();
            inputAction.performed -= OnEscape;
        }

        private void OnEscape(InputAction.CallbackContext context)
        {
            if (canvasGroupFader.Alpha == 0)
            {
                canvasGroupFader.FadeIn();
            }
            else
            {
                canvasGroupFader.FadeOut();
            }
        }
   }
}