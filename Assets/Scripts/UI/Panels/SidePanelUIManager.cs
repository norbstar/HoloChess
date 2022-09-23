using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace UI.Panels
{
    public class SidePanelUIManager : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] Sprite highBatteryIcon;
        [SerializeField] Sprite mediumBatteryIcon;
        [SerializeField] Sprite lowBatteryIcon;
        [SerializeField] Sprite fullIcon;
        [SerializeField] Sprite chargingIcon;
        [SerializeField] Sprite notChargingIcon;
        [SerializeField] Sprite unknownIcon;

        [Header("Components")]
        [SerializeField] TextMeshProUGUI timeUI;
        [SerializeField] Image chargeStateImage;
        [SerializeField] Image chargeLevelImage;
        [SerializeField] TextMeshProUGUI batteryPercentageUI;

        // Update is called once per frame
        void Update()
        {
            // if (!isShown) return;

            var time = DateTime.Now.ToString("HH:mm");
            timeUI.text = time;

            var batteryStatus = SystemInfo.batteryStatus;
            // Debug.Log($"Battery Status : {batteryStatus.ToString()}");

            switch (batteryStatus)
            {
                case BatteryStatus.Full:
                    chargeStateImage.sprite = fullIcon;
                    break;

                case BatteryStatus.Charging:
                    chargeStateImage.sprite = chargingIcon;
                    break;

                case BatteryStatus.NotCharging:
                case BatteryStatus.Discharging:
                    chargeStateImage.sprite = notChargingIcon;
                    break;

                case BatteryStatus.Unknown:
                    chargeStateImage.sprite = unknownIcon;
                    break;
            }

            var batteryLevel = SystemInfo.batteryLevel;
            // Debug.Log($"Battery Level : {batteryLevel.ToString()}");
            
            if (batteryLevel > 0.66f)
            {
                chargeLevelImage.sprite = highBatteryIcon;
            }
            else if (batteryLevel >= 0.33f && batteryLevel <= 0.66f)
            {
                chargeLevelImage.sprite = mediumBatteryIcon;
            }
            else if (batteryLevel < 0.33f)
            {
                chargeLevelImage.sprite = lowBatteryIcon;
            }

            if (batteryLevel >= 0)
            {
                int percentage = System.Convert.ToInt32(100 * batteryLevel);
                batteryPercentageUI.text = $"{percentage.ToString()}%";
            }
        }
    }
}