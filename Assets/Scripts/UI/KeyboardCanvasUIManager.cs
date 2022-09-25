using UnityEngine;

using UI.Panels;

namespace UI
{
    public class KeyboardCanvasUIManager : LockableCanvasUIManager<KeyboardPanelUIManager>
    {
        [Header("Components")]
        [SerializeField] KeyboardMap keyboardMap;

        protected override void Awake()
        {
            base.Awake();
            Show();
        }
    }
}