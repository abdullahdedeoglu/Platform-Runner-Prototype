using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource; // Arka plan müziði için
    [SerializeField] private AudioSource sfxSource;             // Ses efektleri için

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;         // Þarký
    [SerializeField] private AudioClip coinCollectSound;        // Para Toplama
    [SerializeField] private AudioClip deathSound;              // Ölme
    [SerializeField] private AudioClip collisionSound;          // Çarpýþma/Uçma
    [SerializeField] private AudioClip cameraTransitionSound;   // Kamera Geçiþ
    [SerializeField] private AudioClip gameWinSound;            // Oyunu Bitirme

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne geçiþlerinde yok olmamasý için
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
    /// Arka plan müziðini çalar.
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
    /// Coin toplama sesini çalar.
    /// </summary>
    public void PlayCoinCollectSound()
    {
        PlaySFX(coinCollectSound);
    }

    /// <summary>
    /// Ölme sesini çalar.
    /// </summary>
    public void PlayDeathSound()
    {
        PlaySFX(deathSound);
    }

    /// <summary>
    /// Çarpýþma veya uçma sesini çalar.
    /// </summary>
    public void PlayCollisionSound()
    {
        PlaySFX(collisionSound);
    }

    /// <summary>
    /// Kamera geçiþ sesini çalar.
    /// </summary>
    public void PlayCameraTransitionSound()
    {
        PlaySFX(cameraTransitionSound);
    }

    /// <summary>
    /// Oyunu kazanma sesini çalar.
    /// </summary>
    public void PlayGameWinSound()
    {
        PlaySFX(gameWinSound);
    }

    /// <summary>
    /// Genel ses efektlerini çalar.
    /// </summary>
    /// <param name="clip">Çalýnacak ses dosyasý.</param>
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
