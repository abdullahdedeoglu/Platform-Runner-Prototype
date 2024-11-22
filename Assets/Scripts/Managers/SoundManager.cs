using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance for global access
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource; // Audio source for background music
    [SerializeField] private AudioSource sfxSource;             // Audio source for sound effects

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;         // Background music track
    [SerializeField] private AudioClip coinCollectSound;        // Sound for collecting coins
    [SerializeField] private AudioClip deathSound;              // Sound for player death
    [SerializeField] private AudioClip collisionSound;          // Sound for collisions or flying off
    [SerializeField] private AudioClip cameraTransitionSound;   // Sound for camera transitions
    [SerializeField] private AudioClip gameWinSound;            // Sound for winning the game

    private void Awake()
    {
        // Ensure there is only one instance of SoundManager and persist it across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction during scene transitions
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }


    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusicSource != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true; // Loop the background music
            backgroundMusicSource.Play();
        }
    }


    public void PlayCoinCollectSound()
    {
        PlaySFX(coinCollectSound);
    }

    public void PlayDeathSound()
    {
        PlaySFX(deathSound);
    }

    public void PlayCollisionSound()
    {
        PlaySFX(collisionSound);
    }

    public void PlayCameraTransitionSound()
    {
        PlaySFX(cameraTransitionSound);
    }

    public void PlayGameWinSound()
    {
        PlaySFX(gameWinSound);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip); // Play the clip once
        }
    }
}
