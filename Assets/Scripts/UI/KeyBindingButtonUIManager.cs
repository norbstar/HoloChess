using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class KeyBindingButtonUIManager : ButtonUIManager
    {
        [Header("Custom Components")]
        [SerializeField] TextMeshProUGUI textUI;

        [Header("Custom Config")]
        [SerializeField] KeyCode keyCode;
        public KeyCode KeyCode { get { return keyCode; } }

        public override void Awake()
        {
            base.Awake();
            textUI.text = ((char) keyCode).ToString();
        }
    }
}