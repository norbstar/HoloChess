using UnityEngine;
using UnityEngine.InputSystem;

using Mutator;

namespace UI
{
    [RequireComponent(typeof(CanvasGroupAlphaMutator))]
    [RequireComponent(typeof(PositionMutator))]
    public class NavCanvasManager : CachedObject<SceneNavigator>
    {
        [Header("Config")]
        [SerializeField] InputAction inputAction;

        public enum State
        {
            Shown,
            Hidden
        }

        private CanvasGroupAlphaMutator alphaMutator;
        private PositionMutator positionMutator;
        private State state;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();

            state = State.Hidden;
        }

        private void ResolveDependencies()
        {
            alphaMutator = GetComponent<CanvasGroupAlphaMutator>() as CanvasGroupAlphaMutator;
            positionMutator = GetComponent<PositionMutator>() as PositionMutator;
        }

        void OnEnable()
        {
            inputAction.Enable();
            inputAction.performed += OnEscape;

            positionMutator.EventReceived += OnPositionChange;
        }

        void OnDisable()
        {
            inputAction.Disable();
            inputAction.performed -= OnEscape;

            positionMutator.EventReceived -= OnPositionChange;
        }

        private void OnEscape(InputAction.CallbackContext context)
        {
            if (state == State.Shown)
            {
                positionMutator.MoveTo(new Vector3(0f, -425f, 0f));
                state = State.Hidden;
            }
            else
            {
                positionMutator.MoveTo(new Vector3(0f, 0f, 0f));
                state = State.Shown;
            }
        }

        private void OnPositionChange(PositionMutator mutator, float fractionComplete) => alphaMutator.SyncTo((state == State.Shown) ? fractionComplete : 1f - fractionComplete);
   }
}