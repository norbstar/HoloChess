using UnityEngine;
using UnityEngine.InputSystem;

using FX.UI;

using ScaleType = FX.UI.RectTransformFXManager.ScaleType;

namespace Tests
{
    [RequireComponent(typeof(RectTransformFXManager))]
    public class RectTransformFXTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] InputAction inputAction;
        [SerializeField] ScaleType scaleType;
        [SerializeField] float scaleFactor = 1.1f;

        private RectTransformFXManager rectTransformFXManager;
        private RectTransformFX rectTransformFX;
        private Vector2 originalScale;
        private bool scaledUp;

        void Awake()
        {
            ResolveDependencies();
            rectTransformFX = rectTransformFXManager.RectTransformFX;
        }

        // Start is called before the first frame update
        void Start()
        {
            rectTransformFX = rectTransformFXManager.RectTransformFX;
            originalScale = rectTransformFXManager.OriginalScale;
        }

        private void ResolveDependencies() => rectTransformFXManager = GetComponent<RectTransformFXManager>() as RectTransformFXManager;

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
        
        private void ScaleUp() => rectTransformFXManager.ScaleTween(originalScale, rectTransformFX.TweenScale, originalScale * scaleFactor, scaleType);

        private void ScaleDown() => rectTransformFXManager.ScaleTween(originalScale * scaleFactor, rectTransformFX.TweenScale, originalScale, scaleType);

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