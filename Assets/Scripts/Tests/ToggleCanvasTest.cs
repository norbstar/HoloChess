using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ToggleCanvasTest : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;

        public void Toggle() => canvasGroup.alpha = 1f - canvasGroup.alpha;
    }
}