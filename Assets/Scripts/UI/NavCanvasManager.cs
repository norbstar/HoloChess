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

        private CanvasGroupAlphaMutator alphaMutator;
        private PositionMutator positionMutator;
        private bool panelShown;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
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
            if (panelShown)
            {
                alphaMutator.FadeOut();
                // positionMutator.MoveTo(new Vector3(0f, -425f, 0f));
            }
            else
            {
                alphaMutator.FadeIn();
                // positionMutator.MoveTo(new Vector3(0f, 0f, 0f));
            }

            panelShown = !panelShown;
        }

        private void OnPositionChange(PositionMutator mutator, float fractionComplete) { }
   }
}