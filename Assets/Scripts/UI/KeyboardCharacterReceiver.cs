using UnityEngine;

using TMPro;

namespace UI
{
    public class KeyboardCharacterReceiver : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TextMeshProUGUI textUI;

        private KeyboardCanvasUIManager manager;

        void Awake() => ResolveDependencies();

        private void ResolveDependencies() => manager = FindObjectOfType<KeyboardCanvasUIManager>() as KeyboardCanvasUIManager;

        void OnEnable() => manager.EventReceived += OnCharacterEvent;

        void OnDisable() => manager.EventReceived -= OnCharacterEvent;

        private void OnCharacterEvent(string character) => textUI.text = $"{textUI.text}{character}";
    }
}