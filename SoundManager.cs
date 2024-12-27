using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider soundSlider;
    public AudioSource menuAudioSource;
    public AudioSource gameplayAudioSource;

    private const string VolumePrefKey = "MusicVolume";

    void Start()
    {
        // Load saved volume or set to default (0.7) if no volume is saved
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.7f);
        soundSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Add listener for slider value changes
        soundSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    void OnSliderValueChanged()
    {
        float volume = soundSlider.value;
        SetVolume(volume);

        // Save the volume value to PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
    }

    void SetVolume(float volume)
    {
        // Set the volume of both audio sources
        menuAudioSource.volume = volume;
        gameplayAudioSource.volume = volume;
    }
}
