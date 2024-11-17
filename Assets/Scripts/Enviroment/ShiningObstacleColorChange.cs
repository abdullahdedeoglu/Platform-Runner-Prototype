using UnityEngine;

public class ShiningObstacleColorChange : MonoBehaviour
{
    public ParticleSystem shiningParticleSystem;
    private ParticleSystem.MainModule particleMain;

    // �arp��ma sonras� kullan�labilecek renkler
    public Color[] hitColors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };

    private Color lastColor; // Son se�ilen rengi saklar

    void Start()
    {
        // Particle System ana mod�l�n� al
        particleMain = shiningParticleSystem.main;
        lastColor = particleMain.startColor.color; // Ba�lang�� rengini sakla
    }

    private void OnCollisionEnter(Collision collision)
    {
        // E�er �arpan nesne oyuncu ise renk de�i�tir
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeHitColor();
        }
    }

    private void ChangeHitColor()
    {
        Color newColor;

        // Ayn� renk olmamas� i�in d�ng� ile kontrol et
        do
        {
            newColor = hitColors[Random.Range(0, hitColors.Length)];
        }
        while (newColor == lastColor);

        particleMain.startColor = newColor;
        lastColor = newColor; // Yeni rengi lastColor'a kaydet
    }
}
