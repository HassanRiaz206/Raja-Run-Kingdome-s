using UnityEngine;
using UnityEngine.UI;

public class SoundEffectsManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource runAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource doubleJumpAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource platformChangeAudioSource;
    public AudioSource uiButtonClickAudioSource;
    public AudioSource claimButtonAudioSource;

    public AudioSource winaudioSource;


    [Header("Volume Control")]
    public Slider soundEffectsSlider;

    private const string VolumePrefKey = "SoundEffectsVolume";

    void Start()
    {
        // Load saved volume or set to default (0.7) if no volume is saved
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.7f);
        soundEffectsSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Add listener for slider value changes
        soundEffectsSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    void OnSliderValueChanged()
    {
        float volume = soundEffectsSlider.value;
        SetVolume(volume);

        // Save the volume value to PlayerPrefs
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
    }

    void SetVolume(float volume)
    {
        // Set the volume of all sound effect audio sources
        runAudioSource.volume = volume;
        jumpAudioSource.volume = volume;
        doubleJumpAudioSource.volume = volume;
        deathAudioSource.volume = volume;
        platformChangeAudioSource.volume = volume;
        uiButtonClickAudioSource.volume = volume;
        claimButtonAudioSource.volume = volume;
        winaudioSource.volume = volume;
    }

    // These methods should be called by the appropriate game events or UI interactions
    public void PlayRunSound()
    {
        if (!runAudioSource.isPlaying)
        {
            runAudioSource.Play();
        }
    }

    public void StopRunSound()
    {
        runAudioSource.Stop();
    }

    public void PlayJumpSound()
    {
        jumpAudioSource.Play();
    }

    public void PlayDoubleJumpSound()
    {
        doubleJumpAudioSource.Play();
    }

    public void PlayDeathSound()
    {
        deathAudioSource.Play();
    }
    public void PlayWinSound()
    {
        winaudioSource.Play();
    }
    public void PlayPlatformChangeSound()
    {
        platformChangeAudioSource.Play();
    }

    public void PlayUIButtonClickSound()
    {
        uiButtonClickAudioSource.Play();
    }

    public void PlayClaimButtonSound()
    {
        claimButtonAudioSource.Play();
    }
}
