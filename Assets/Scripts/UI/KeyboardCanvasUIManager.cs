using System.Collections.Generic;

using UnityEngine;

using UI.Panels;

using Scriptables;

namespace UI
{
    public class KeyboardCanvasUIManager : LockableCanvasUIManager<KeyboardPanelUIManager>
    {
        [Header("Config")]
        [SerializeField] KeyboardProfile profile;

        protected override void Awake()
        {
            base.Awake();
            Show();

            // List<KeyboardProfile.Binding> bindings = profile.GetBindings();
            // Debug.Log($"Bindings : {bindings.Count}");
        }
    }
}