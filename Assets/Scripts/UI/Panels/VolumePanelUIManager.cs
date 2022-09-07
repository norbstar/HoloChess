using UnityEngine;

namespace UI.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    public class VolumePanelUIManager : SliderPanelUIManager
    {
        private CanvasGroup canvasGroup;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;

        public void Show() => canvasGroup.alpha = 1f;

        public void Hide() => canvasGroup.alpha = 0f;
   }
}