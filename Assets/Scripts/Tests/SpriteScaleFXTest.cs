using UnityEngine;
using UnityEngine.InputSystem;

using FX;

using ScaleType = FX.ScaleFX2DManager.ScaleType;

namespace Tests
{
    [RequireComponent(typeof(ScaleFX2DManager))]
    public class SpriteScaleFXTest : MonoBehaviour
    {
        public enum ScaleMethod
        {
            Tween,
            Custom
        }

        [Header("Config")]
        [SerializeField] InputAction inputAction;
        [SerializeField] ScaleType scaleType;
        [SerializeField] float scaleFactor = 1.1f;

        private ScaleFX2DManager scaleFXManager;
        private Vector3 originalScale;
        private bool scaledUp;

        void Awake()
        {
            ResolveDependencies();
            originalScale = transform.localScale;
        }

        private void ResolveDependencies() => scaleFXManager = GetComponent<ScaleFX2DManager>() as ScaleFX2DManager;

        void OnEnable()
        {
            inputAction.Enable();
            inputAction.performed += OnSpace;
        }

        void OnDisable()
        {
            inputAction.Disable();
            inputAction.performed -= OnSpace;
        }

        private void ScaleUp() => scaleFXManager.ScaleTween(originalScale, transform.localScale, originalScale * scaleFactor);

        private void ScaleDown() => scaleFXManager.ScaleTween(transform.localScale, transform.localScale, originalScale);

        private void OnSpace(InputAction.CallbackContext context)
        {
            if (scaledUp)
            {
                ScaleDown();
            }
            else
            {
                ScaleUp();
            }

            scaledUp = !scaledUp;
        }
    }
}