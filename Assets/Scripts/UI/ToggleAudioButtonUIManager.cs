using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using UnityButton = UnityEngine.UI.Button;

namespace UI
{
    public class ToggleAudioButtonUIManager : ToggleButtonUIManager, IAudioComponent
    {
        private List<IAudioComponent> audioComponents;
        private float cachedValue;

        public override void Awake()
        {
            base.Awake();
            audioComponents = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IAudioComponent>().ToList();
        }

        public override void OnClickButton(UnityButton button)
	    {
            base.OnClickButton(button);

            var newValue = (isOn) ? cachedValue : 0f;

            foreach (IAudioComponent component in audioComponents)
            {
                if (!ReferenceEquals(gameObject, component.GetObject()))
                {
                    component.SyncVolume(newValue);
                }
            }

            cachedValue = newValue;
        }

        public GameObject GetObject() => gameObject;

        public void SyncVolume(float volume)
        {
            isOn = volume > 0f;
		    cachedValue = volume;
        }
    }
}