
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class ToggleSliderPanelUIManager : SliderPanelUIManager
    {
        [Header("Toggle Components")]
        [SerializeField] Toggle toggle;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            toggle.onValueChanged.AddListener(delegate {
                OnToggleChanged(toggle.isOn);
            });

            toggle.isOn = (slider.value == slider.minValue);
        }

        protected override void OnValueChanged(float value)
        {
            toggle.isOn = (value == slider.minValue);

            if (!toggle.isOn && !toggle.interactable)
            {
                toggle.interactable = true;
            }

            base.OnValueChanged(value);
        }

        public void OnToggleChanged(bool isOn)
        {
            if (!isOn) return;

            toggle.interactable = false;
            slider.value = slider.minValue;
        }
    }
}