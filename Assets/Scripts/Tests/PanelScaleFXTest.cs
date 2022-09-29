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
            Debug.Log($"{gameObject.name} ScaleUp FromScale : {originalScale} TweenScale : {transform.localScale} ToScale : {originalScale * scaleFactor} ScaleType : {scaleType}");
            scaleFXManager.ScaleTween(/*originalScale, */transform.localScale, originalScale * scaleFactor, scaleType);
        }

        private void ScaleDown()
        {
            Debug.Log($"{gameObject.name} ScaleDown FromScale : {originalScale * scaleFactor} TweenScale : {transform.localScale} ToScale : {originalScale} ScaleType : {scaleType}");
            scaleFXManager.ScaleTween(/*originalScale * scaleFactor, */transform.localScale, originalScale, scaleType);
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