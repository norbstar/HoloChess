using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class NavCanvasManager : CachedObject<SceneNavigator>
    {
        [Header("Config")]
        [SerializeField] InputAction inputAction;

        public enum State
        {
            Shown,
            Hidden
        }

        private Animator animator;
        private State state;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();

            state = State.Hidden;
        }

        private void ResolveDependencies() => animator = GetComponent<Animator>() as Animator;

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
            if (state == State.Shown)
            {
                animator.SetTrigger("Hide");
                state = State.Hidden;
            }
            else
            {
                animator.SetTrigger("Show");
                state = State.Shown;
            }
        }
   }
}