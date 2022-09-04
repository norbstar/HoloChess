using UnityEngine;

using UI.Panels;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceModifier : MonoBehaviour
{
    [SerializeField] SliderPanelUIManager source;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        ResolveDependencies();

        if (source != null)
        {
            source.Value = audioSource.volume;
        }
    }

    private void ResolveDependencies() => audioSource = GetComponent<AudioSource>() as AudioSource;

    void OnEnable() => SliderPanelUIManager.EventReceived += OnSliderEvent;

    void OnDisable() => SliderPanelUIManager.EventReceived -= OnSliderEvent;

    public float Volume { get { return audioSource.volume; } set { audioSource.volume = value; } }

    private void OnSliderEvent(float value) => audioSource.volume = value;
}