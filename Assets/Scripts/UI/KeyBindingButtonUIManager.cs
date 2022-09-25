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

        [Header("Config")]
        [SerializeField] int id;
        public int Id { get { return id; } set { id = value; } }

        public override void Awake()
        {
            base.Awake();

            // TODO
        }
    }
}