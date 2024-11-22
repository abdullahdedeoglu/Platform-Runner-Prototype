using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class Coin : MonoBehaviour
{
    public GameObject coinAnimationPrefab; // Coin UI prefab used for animation
    public Transform uiTarget; // Target UI element (e.g., the coin counter object)
    public Canvas canvas; // Reference to the main Canvas
    public int coinValue = 1; // Value of the coin
    public float animationSpeed = 1f; // Speed of the animation (adjustable in the Inspector)
    public float animationDelay = 0.1f; // Delay between animations

    private RectTransform canvasRectTransform; // RectTransform of the Canvas
    private bool isCollected = false; // Flag to check if the coin has already been collected

    private MeshRenderer _meshRenderer; // MeshRenderer of the coin object

    private List<GameObject> animatedCoins = new List<GameObject>(); // List to keep track of spawned animated objects

    private void Start()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>(); // Get the RectTransform of the Canvas
        _meshRenderer = this.gameObject.GetComponent<MeshRenderer>(); // Get the MeshRenderer of the coin object
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return; // Prevent multiple collections of the same coin

        if (other.CompareTag("Player")) // Check if the collider belongs to the Player
        {
            isCollected = true; // Mark the coin as collected
            SoundManager.Instance.PlayCoinCollectSound(); // Play the coin collection sound
            StartCoroutine(CollectCoin()); // Start the coin collection process
        }
    }

    private IEnumerator CollectCoin()
    {
        // Disable the coin's mesh to hide it from view
        _meshRenderer.enabled = false;

        // Start the coin animation sequence
        yield return StartCoroutine(SpawnAndAnimateCoins());

        GameManager.Instance.AddCoin(coinValue); // Add the coin's value to the player's total score

        // Destroy the coin object after completing the animations
        Destroy(gameObject);
    }

    private IEnumerator SpawnAndAnimateCoins()
    {
        // Get the screen position of the coin in the game world
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Convert the screen position to a position within the Canvas
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, cameraPosition, canvas.worldCamera, out Vector2 canvasPosition))
        {
            yield break; // Stop if the conversion fails
        }

        for (int i = 0; i < 5; i++) // Create 5 animated coin objects
        {
            GameObject animatedCoin = Instantiate(coinAnimationPrefab, canvas.transform); // Instantiate the coin prefab as a child of the Canvas
            RectTransform animatedCoinRect = animatedCoin.GetComponent<RectTransform>();

            if (animatedCoinRect != null)
            {
                animatedCoinRect.anchoredPosition = canvasPosition; // Set the initial position of the animated coin

                // Start the animation moving towards the UI target
                animatedCoinRect
                    .DOMove(uiTarget.GetComponent<RectTransform>().position, animationSpeed) // Move to the target position
                    .SetEase(Ease.InOutQuad) // Apply a smooth easing effect to the animation
                    .OnComplete(() =>
                    {
                        Destroy(animatedCoin); // Destroy the animated coin after the animation is complete
                    });
            }

            // Add a delay before spawning the next animated coin
            yield return new WaitForSeconds(animationDelay);
        }
    }
}
