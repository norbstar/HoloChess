using UnityEngine;
using UnityEngine.InputSystem;

using FX.UI;

using ScaleType = FX.UI.RectTransformFXManager.ScaleType;

namespace Tests
{
    [AddComponentMenu("Tests/Rect Transform FX Test")]
    [RequireComponent(typeof(RectTransformFXManager))]
    public class RectTransformFXTest : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] InputAction inputAction;
        [SerializeField] ScaleType scaleType;
        [SerializeField] float scaleFactor = 1.1f;

        private RectTransformFXManager rectTransformFXManager;
        private RectTransformFX rectTransformFX;
        private Vector2 originalSize;
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
            originalSize = rectTransformFXManager.OriginalSize;
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
        
        private void ScaleUp()
        {
            Vector2 fromSize = originalSize;
            Vector2 toSize = originalSize * scaleFactor;
            Vector2 tweenSize = rectTransformFX.TweenSize;
            Vector2 endSize = toSize;
            // Debug.Log($"{gameObject.name} ScaleUp TweenSize : {tweenSize.ToPrecisionString()} ToSize : {toSize.ToPrecisionString()} ScaleType : {scaleType}");
            rectTransformFXManager.Tween(fromSize, toSize, tweenSize, endSize, scaleType);
        }

        private void ScaleDown()
        {
            Vector2 fromSize = originalSize * scaleFactor;
            Vector2 toSize = originalSize;
            Vector2 tweenSize = rectTransformFX.TweenSize;
            Vector2 endSize = toSize;
            // Debug.Log($"{gameObject.name} ScaleDown TweenScale : {tweenSize.ToPrecisionString()} ToScale : {toSize.ToPrecisionString()} ScaleType : {scaleType}");
            rectTransformFXManager.Tween(fromSize, toSize, tweenSize, toSize, RectTransformFXManager.ScaleType.Proportional);
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