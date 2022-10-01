using UnityEngine;

using TMPro;

namespace UI
{
    [AddComponentMenu("UI/Keyboard Character Receiver")]
    public class KeyboardCharacterReceiver : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TextMeshProUGUI textUI;

        private KeyboardCanvasUIManager manager;
        private string text;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => manager = FindObjectOfType<KeyboardCanvasUIManager>() as KeyboardCanvasUIManager;

        void OnEnable() => manager.EventReceived += OnCharacterEvent;

        void OnDisable() => manager.EventReceived -= OnCharacterEvent;

        private void OnCharacterEvent(string unicode, string character)
        {
            text = string.Concat(textUI.text, character);
            textUI.text = text;
        }
    }
}