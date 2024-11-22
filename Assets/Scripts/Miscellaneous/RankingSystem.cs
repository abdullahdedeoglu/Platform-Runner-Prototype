using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public List<Transform> players; // List of all players and AI characters
    public Transform finishLine;   // Transform of the finish line
    public Transform mainPlayer;   // Transform of the main player

    void Update()
    {
        // Sort players by their distance to the finish line
        players.Sort((a, b) =>
            Vector3.Distance(a.position, finishLine.position).CompareTo(
                Vector3.Distance(b.position, finishLine.position))
        );

        // Determine the main player's rank
        int playerRank = players.IndexOf(mainPlayer) + 1;

        // Display the player's rank on the UI
        CanvasManager.Instance.ShowRanking(playerRank);
    }
}
