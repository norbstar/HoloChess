using UnityEngine;
using UnityEngine.InputSystem;

using FX;

using ScaleType = FX.ScaleFXManager.ScaleType;

namespace Tests
{
    [AddComponentMenu("Tests/Cube Scale FX Test")]
    [RequireComponent(typeof(ScaleFXManager))]
    public class CubeScaleFXTest : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] InputAction inputAction;

        [Header("Scaling")]
        [SerializeField] ScaleType scaleType;
        [SerializeField] float scaleFactor = 1.1f;

        private ScaleFXManager scaleFXManager;
        private Vector3 originalScale;
        private bool scaledUp;

        void Awake()
        {
            ResolveDependencies();
            originalScale = transform.localScale;
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
            Vector3 fromScale = originalScale;
            Vector3 toScale = originalScale * scaleFactor;
            Vector3 tweenScale = transform.localScale;
            Vector3 endScale = toScale;
            Debug.Log($"{gameObject.name} ScaleUp TweenScale : {tweenScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()} ScaleType : {scaleType}");
            scaleFXManager.Tween(fromScale, toScale, tweenScale, endScale, scaleType);
        }

        private void ScaleDown()
        {
            Vector3 fromScale = originalScale * scaleFactor;
            Vector3 toScale = originalScale;
            Vector3 tweenScale = transform.localScale;
            Vector3 endScale = toScale;
            Debug.Log($"{gameObject.name} ScaleDown TweenScale : {tweenScale.ToPrecisionString()} ToScale : {toScale.ToPrecisionString()} ScaleType : {scaleType}");
            scaleFXManager.Tween(fromScale, toScale, tweenScale, toScale, ScaleFX2DManager.ScaleType.Proportional);
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