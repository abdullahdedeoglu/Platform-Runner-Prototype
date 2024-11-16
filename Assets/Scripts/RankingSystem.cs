using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public List<Transform> players; // Oyuncu ve AI karakterlerini ekle
    public Transform finishLine; // Biti� �izgisinin transformu
    public Transform mainPlayer; // Ba� karakterin transformu

    void Update()
    {
        // Karakterleri biti� �izgisine olan uzakl��a g�re s�ralar
        players.Sort((a, b) =>
            Vector3.Distance(a.position, finishLine.position).CompareTo(
                Vector3.Distance(b.position, finishLine.position))
        );

        // Ba� karakterin s�ralamas�n� bul
        int playerRank = players.IndexOf(mainPlayer) + 1;
        
        CanvasManager.Instance.ShowRanking(playerRank);
        
        
    }
}

