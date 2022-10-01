using UnityEngine;

namespace UI.Panels
{
    [AddComponentMenu("UI/Panels/Volume Panel UI Manager")]
    [RequireComponent(typeof(CanvasGroup))]
    public class VolumePanelUIManager : ToggleSliderPanelUIManager
    {
        private CanvasGroup canvasGroup;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;

        public void Show() => canvasGroup.alpha = 1f;

        public void Hide() => canvasGroup.alpha = 0f;
   }
}