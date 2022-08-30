using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuCanvasUIManager : CachedObject<MenuCanvasUIManager>
    {
        [SerializeField] NavigationPanelUIManager navigation;

        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            ResolveDependencies();
        }

        private void ResolveDependencies() => canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;

        public void Toggle()
        {
            if (canvasGroup.alpha == 0f)
            {
                navigation.gameObject.SetActive(true);
                canvasGroup.alpha = 1f;
            }
            else
            {
                canvasGroup.alpha = 0f;
                navigation.gameObject.SetActive(false);
            }
        }
    }
}