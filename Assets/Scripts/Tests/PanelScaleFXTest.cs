using UnityEngine;
using UnityEngine.InputSystem;

using FX;

using ScaleType = FX.ScaleFX2DManager.ScaleType;

namespace Tests
{
    [RequireComponent(typeof(ScaleFX2DManager))]
    public class PanelScaleFXTest : MonoBehaviour
    {
        public enum ScaleMethod
        {
            Tween,
            Custom
        }

        [Header("Config")]
        [SerializeField] InputAction inputAction;
        [SerializeField] ScaleMethod scaleMethod;
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

        private void ScaleUp()
        {
            switch (scaleMethod)
            {
                case ScaleMethod.Tween:
                    scaleFXManager.ScaleTween(originalScale, originalScale * scaleFactor);
                    break;

                case ScaleMethod.Custom:
                    scaleFXManager.ScaleCustom(originalScale, scaleType, scaleFactor);
                    break;
            }
        }

        private void ScaleDown()
        {
            switch (scaleMethod)
            {
                case ScaleMethod.Tween:
                    scaleFXManager.ScaleTween(transform.localScale, originalScale);
                    break;

                case ScaleMethod.Custom:
                    float inverseScaleFactor = scaleFXManager.CalculateInverseScaleFactor(transform.localScale, originalScale, scaleType);
                    scaleFXManager.ScaleCustom(transform.localScale, scaleType, inverseScaleFactor);
                    break;
            }
        }

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