using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI
{
    [AddComponentMenu("UI/Lockable Canvas UI Manager")]
    public abstract class LockableCanvasUIManager<T> : DragbarCanvasUIManager<T> where  T : IDragbarPanel
    {
        protected bool isLocked;
        public bool IsLocked { get { return isLocked; } }

        // Start is called before the first frame update
        // protected override void Start()
        // {
        //     base.Start();
        //     isLocked = panel.IsLocked;
        // }

        // protected override void OnEnable()
        // {
        //     base.OnEnable();
        //     panel.LockedEventReceived += OnLockSwapEvent;
        // }

        // protected override void OnDisable()
        // {
        //     base.OnDisable();
        //     panel.LockedEventReceived -= OnLockSwapEvent;
        // }

        private void OnLockSwapEvent(bool isLocked) => this.isLocked = isLocked;
    }
}