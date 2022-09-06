using UnityEngine;
using UnityEngine.UI;

using UnityButton = UnityEngine.UI.Button;

namespace UI
{
    public class ToggleButtonUIManager : ButtonUIManager
    {
        [Header("Custom Components")]
        [SerializeField] Image bar;

        [Header("Custom Config")]
        [SerializeField] Color offColor;
        [SerializeField] Color onColor;

        protected bool isOn = false;
        public bool IsOn
        {
            get
            {
                return isOn;
             }
             
             set
             {
                isOn = value;
                bar.color = (isOn) ? onColor : offColor;
            }
        }

        public override void OnClickButton(UnityButton button)
        {
            bar.color = (isOn) ? offColor : onColor;
            isOn = !isOn;

            base.OnClickButton(button);
        }
    }
}