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
        [SerializeField] bool overloadLabel = false;
        [SerializeField] string label;
        public string Label { get { return label; } }

        public override void Awake()
        {
            base.Awake();

            if (!overloadLabel)
            {
                label = ((char) keyCode).ToString();
            }

            textUI.text = label;
        }
    }
}