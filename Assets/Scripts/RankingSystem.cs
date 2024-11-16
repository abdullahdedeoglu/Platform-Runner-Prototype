using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    public List<Transform> players; // Oyuncu ve AI karakterlerini ekle
    public Transform finishLine; // Bitiþ çizgisinin transformu
    public Transform mainPlayer; // Baþ karakterin transformu

    void Update()
    {
        // Karakterleri bitiþ çizgisine olan uzaklýða göre sýralar
        players.Sort((a, b) =>
            Vector3.Distance(a.position, finishLine.position).CompareTo(
                Vector3.Distance(b.position, finishLine.position))
        );

        // Baþ karakterin sýralamasýný bul
        int playerRank = players.IndexOf(mainPlayer) + 1;
        
        CanvasManager.Instance.ShowRanking(playerRank);
        
        
    }
}

