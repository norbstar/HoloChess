using UnityEngine;
using UnityEngine.InputSystem;

using FX;

using ScaleType = FX.ScaleFXManager.ScaleType;

namespace Tests
{
    [RequireComponent(typeof(ScaleFXManager))]
    public class CubeScaleFXTest : MonoBehaviour
    {
        public enum ScaleMethod
        {
            Tween,
            Custom
        }

        [Header("Config")]
        [SerializeField] InputAction inputAction;
        [SerializeReference] ScaleMethod scaleMethod;

        private static float scaleFactor = 1.1f;

        private ScaleFXManager scaleFXManager;
        private Vector3 originalScale, modifiedScale;
        private bool scaledUp;

        void Awake()
        {
            ResolveDependencies();
            originalScale = transform.localScale;
            modifiedScale = originalScale * scaleFactor;
        }

        private void ResolveDependencies() => scaleFXManager = GetComponent<ScaleFXManager>() as ScaleFXManager;

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
                    scaleFXManager.ScaleCustom(originalScale, ScaleType.RelativeToY, scaleFactor);
                    break;
            }
        }

        private void ScaleDown()
        {
            switch (scaleMethod)
            {
                case ScaleMethod.Tween:
                    scaleFXManager.ScaleTween(modifiedScale, originalScale);
                    break;

                case ScaleMethod.Custom:
                    float inverseScaleFactor = ((transform.localScale.y - originalScale.y) / transform.localScale.y) * 10f;
                    scaleFXManager.ScaleCustom(modifiedScale, ScaleType.RelativeToY, inverseScaleFactor);
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