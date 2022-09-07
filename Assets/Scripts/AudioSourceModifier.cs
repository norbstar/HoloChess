using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using UI;
using UI.Panels;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceModifier : MonoBehaviour
{  
    private List<IAudioComponent> audioComponents;
    private AudioSource audioSource;
    private float cachedVolume;

    void Awake()
    {
        ResolveDependencies();
        cachedVolume = audioSource.volume;
    }
    
    private void ResolveDependencies()
    {
        audioSource = GetComponent<AudioSource>() as AudioSource;
        audioComponents = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IAudioComponent>().ToList();
    }

    void OnEnable()
    {
        foreach (IAudioComponent component in audioComponents)
        {
            var monoBehaviour = component as MonoBehaviour;

            if (monoBehaviour is SliderPanelUIManager)
            {
                var manager = ((SliderPanelUIManager) monoBehaviour);
                manager.Value = audioSource.volume;
                manager.EventReceived += OnSliderEvent;
            }
            else if (monoBehaviour is ToggleAudioButtonUIManager)
            {
                var manager = ((ToggleAudioButtonUIManager) monoBehaviour);
                manager.IsOn = audioSource.volume > 0f;
                manager.ToggleEventReceived += OnToggleEvent;
            }
        }
    }

    void OnDisable()
    {
        foreach (IAudioComponent component in audioComponents)
        {
            var monoBehaviour = component as MonoBehaviour;

            if (monoBehaviour is SliderPanelUIManager)
            {
                var manager = ((SliderPanelUIManager) monoBehaviour);
                manager.EventReceived -= OnSliderEvent;
            }
            else if (monoBehaviour is ToggleAudioButtonUIManager)
            {
                var manager = ((ToggleAudioButtonUIManager) monoBehaviour);
                manager.ToggleEventReceived += OnToggleEvent;
            }
        }
    }

    private void OnSliderEvent(SliderPanelUIManager manager, float value)
    {
        audioSource.volume = value;

        if (audioSource.volume > 0f)
        {
            cachedVolume = audioSource.volume;
        }

        SyncVolume(manager.gameObject);
    }

    private void OnToggleEvent(ToggleAudioButtonUIManager manager, bool isOn)
    {
        audioSource.volume = (isOn) ? cachedVolume : 0f;

        if (audioSource.volume > 0f)
        {
            cachedVolume = audioSource.volume;
        }

        SyncVolume(manager.gameObject);
    }

    private void SyncVolume(GameObject gameObject)
    {
        foreach (IAudioComponent component in audioComponents)
        {
            var monoBehaviour = component as MonoBehaviour;

            if (!ReferenceEquals(monoBehaviour.gameObject, gameObject))
            {
                if (monoBehaviour is SliderPanelUIManager)
                {
                    ((SliderPanelUIManager) monoBehaviour).SyncVolume(audioSource.volume);
                }
                else if (monoBehaviour is ToggleAudioButtonUIManager)
                {
                    ((ToggleAudioButtonUIManager) monoBehaviour).SyncVolume(audioSource.volume);
                }
            }
        }
    }
}