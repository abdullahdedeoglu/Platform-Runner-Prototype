using UnityEngine;

public class ShiningObstacleColorChange : MonoBehaviour
{
    public ParticleSystem shiningParticleSystem;
    private ParticleSystem.MainModule particleMain;

    // Çarpýþma sonrasý kullanýlabilecek renkler
    public Color[] hitColors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };

    private Color lastColor; // Son seçilen rengi saklar

    void Start()
    {
        // Particle System ana modülünü al
        particleMain = shiningParticleSystem.main;
        lastColor = particleMain.startColor.color; // Baþlangýç rengini sakla
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Eðer çarpan nesne oyuncu ise renk deðiþtir
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeHitColor();
        }
    }

    private void ChangeHitColor()
    {
        Color newColor;

        // Ayný renk olmamasý için döngü ile kontrol et
        do
        {
            newColor = hitColors[Random.Range(0, hitColors.Length)];
        }
        while (newColor == lastColor);

        particleMain.startColor = newColor;
        lastColor = newColor; // Yeni rengi lastColor'a kaydet
    }
}
