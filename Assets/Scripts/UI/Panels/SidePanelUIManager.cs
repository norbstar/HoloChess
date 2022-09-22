using System;

using UnityEngine;

using TMPro;

namespace UI.Panels
{
    public class SidePanelUIManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] TextMeshProUGUI timeUI;
        public string Time { get { return timeUI.text; } }

        // Update is called once per frame
        void Update()
        {
            // if (!isShown) return;

            var time = DateTime.Now.ToString("HH:mm");
            timeUI.text = time;
        }
    }
}