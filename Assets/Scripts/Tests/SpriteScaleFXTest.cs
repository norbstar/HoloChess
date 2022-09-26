using UnityEngine;
using UnityEngine.InputSystem;

using FX;

namespace Tests
{
    [RequireComponent(typeof(ScaleFX2DManager))]
    public class SpriteScaleFXTest : MonoBehaviour
    {
        [SerializeField] InputAction inputAction;

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

        private void OnSpace(InputAction.CallbackContext context)
        {
            if (scaledUp)
            {
                scaleFXManager.ScaleFromTo(new Vector3(originalScale.x * 1.1f, originalScale.y * 1.1f, originalScale.z), originalScale);
            }
            else
            {
                scaleFXManager.ScaleFromTo(originalScale, new Vector3(originalScale.x * 1.1f, originalScale.y * 1.1f, originalScale.z));
            }

            scaledUp = !scaledUp;
        }
    }
}