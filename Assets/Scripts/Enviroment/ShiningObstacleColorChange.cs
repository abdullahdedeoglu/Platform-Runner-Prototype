using UnityEngine;

public class ShiningObstacleColorChange : MonoBehaviour
{
    public ParticleSystem shiningParticleSystem; // Reference to the Particle System that shines on the obstacle
    private ParticleSystem.MainModule particleMain; // Main module of the Particle System

    // Colors that the obstacle can change to after a collision
    public Color[] hitColors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan };

    private Color lastColor; // Stores the last selected color to avoid repetition

    void Start()
    {
        // Get the Main module of the Particle System
        particleMain = shiningParticleSystem.main;

        // Store the initial color of the Particle System
        lastColor = particleMain.startColor.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the object colliding with the obstacle is the player, change the color
        if (collision.gameObject.CompareTag("Player"))
        {
            ChangeHitColor();
        }
    }

    private void ChangeHitColor()
    {
        Color newColor;

        // Ensure the new color is different from the last one
        do
        {
            newColor = hitColors[Random.Range(0, hitColors.Length)]; // Randomly select a color from the array
        }
        while (newColor == lastColor);

        // Apply the new color to the Particle System
        particleMain.startColor = newColor;

        // Update the last color to the newly selected color
        lastColor = newColor;
    }
}
