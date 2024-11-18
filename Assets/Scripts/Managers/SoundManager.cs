using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource; // Arka plan m�zi�i i�in
    [SerializeField] private AudioSource sfxSource;             // Ses efektleri i�in

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;         // �ark�
    [SerializeField] private AudioClip coinCollectSound;        // Para Toplama
    [SerializeField] private AudioClip deathSound;              // �lme
    [SerializeField] private AudioClip collisionSound;          // �arp��ma/U�ma
    [SerializeField] private AudioClip cameraTransitionSound;   // Kamera Ge�i�
    [SerializeField] private AudioClip gameWinSound;            // Oyunu Bitirme

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne ge�i�lerinde yok olmamas� i�in
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    /// <summary>
    /// Arka plan m�zi�ini �alar.
    /// </summary>
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusicSource != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    /// <summary>
    /// Coin toplama sesini �alar.
    /// </summary>
    public void PlayCoinCollectSound()
    {
        PlaySFX(coinCollectSound);
    }

    /// <summary>
    /// �lme sesini �alar.
    /// </summary>
    public void PlayDeathSound()
    {
        PlaySFX(deathSound);
    }

    /// <summary>
    /// �arp��ma veya u�ma sesini �alar.
    /// </summary>
    public void PlayCollisionSound()
    {
        PlaySFX(collisionSound);
    }

    /// <summary>
    /// Kamera ge�i� sesini �alar.
    /// </summary>
    public void PlayCameraTransitionSound()
    {
        PlaySFX(cameraTransitionSound);
    }

    /// <summary>
    /// Oyunu kazanma sesini �alar.
    /// </summary>
    public void PlayGameWinSound()
    {
        PlaySFX(gameWinSound);
    }

    /// <summary>
    /// Genel ses efektlerini �alar.
    /// </summary>
    /// <param name="clip">�al�nacak ses dosyas�.</param>
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
