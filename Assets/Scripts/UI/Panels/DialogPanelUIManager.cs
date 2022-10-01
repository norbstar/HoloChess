using UnityEngine;

using TMPro;

namespace UI.Panels
{
    [AddComponentMenu("UI/Panels/Dialog Panel UI Manager")]
    [RequireComponent(typeof(TextReceiver))]
    [RequireComponent(typeof(CanvasGroup))]
    public class DialogPanelUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TextMeshProUGUI textUI;

        [Header("Config")]
        [SerializeField] float onsetDelay = 0.5f;

        public string Text
        {
            set
            {
                textUI.text = value;
                
                if (textUI.text.Length > 0)
                {
                    canvasGroup.alpha = 1f;
                }
            }   
        }

        private TextReceiver textReceiver;
        private CanvasGroup canvasGroup;

        void Awake() => ResolveDependencies();
        
        private void ResolveDependencies()
        {
            textReceiver = GetComponent<TextReceiver>() as TextReceiver;
            canvasGroup = GetComponent<CanvasGroup>() as CanvasGroup;
        }

        void OnEnable() => textReceiver.EventReceived += OnText;

        void OnDisable() => textReceiver.EventReceived -= OnText;

        private void Hide()
        {
            canvasGroup.alpha = 0f;
            textUI.text = default(string);
        }

        private void OnText(string text)
        {
            if (text.Length > 0)
            {
                Text = text;
            }
            else
            {
                Hide();
            }
        }
    }
}