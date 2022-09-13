using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using UnityButton = UnityEngine.UI.Button;

namespace UI
{
    public class ToggleAudioButtonUIManager : ToggleButtonUIManager, IAudioComponent
    {
        public delegate void OnToggleEvent(ToggleAudioButtonUIManager manager, bool isOn);
        public event OnToggleEvent ToggleEventReceived;

        private List<IAudioComponent> audioComponents;

        public override void Awake()
        {
            base.Awake();
            ResolveDependencies();
       }

        private void ResolveDependencies() => audioComponents = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IAudioComponent>().ToList();

        public override void OnClickButton(UnityButton button)
	    {
            base.OnClickButton(button);
            ToggleEventReceived?.Invoke(this, isOn);
        }

        public void SyncVolume(float volume) => IsOn = volume > 0f;
   }
}