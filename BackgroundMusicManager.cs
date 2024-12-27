using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource menuAudioSource;
    public AudioSource gameplayAudioSource;

    private bool menuMusicShouldPlay = true;  // Flag to control if menu music should play

    void Start()
    {
        PlayMenuMusic(); // Play menu music by default at the start
    }

    public void PlayMenuMusic()
    {
        // Ensure gameplay music stops and menu music plays only if explicitly called
        gameplayAudioSource.Stop();

        if (menuMusicShouldPlay && !menuAudioSource.isPlaying)
        {
            menuAudioSource.Play();
        }
    }

    public void PlayGameplayMusic()
    {
        // Prevent menu music from playing when gameplay music stops
        menuMusicShouldPlay = false;

        menuAudioSource.Stop();
        if (!gameplayAudioSource.isPlaying)
        {
            gameplayAudioSource.Play();
        }
    }

    public void StopMusic()
    {
        // Stop both music tracks without triggering menu music to play again
        menuAudioSource.Stop();
        gameplayAudioSource.Stop();
    }

    public void EnableMenuMusic()
    {
        // Allow menu music to play when PlayMenuMusic is called
        menuMusicShouldPlay = true;
    }

    public void OnPlayerDeath()
    {
        // Example usage if needed: Disable both music sources on player death
        StopMusic();
    }
}
