using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    public List<Transform> players; // Oyuncu ve AI karakterlerini ekle
    public Transform finishLine; // Biti� �izgisinin transformu

    void Update()
    {
        // Karakterleri biti� �izgisine olan uzakl��a g�re s�ralar
        players.Sort((a, b) =>
            Vector3.Distance(a.position, finishLine.position).CompareTo(
                Vector3.Distance(b.position, finishLine.position))
        );

        // S�ralamay� g�ncelle
        string ranking = "";
        for (int i = 0; i < players.Count; i++)
        {
            ranking += (i + 1) + ". " + players[i].name + "\n";
        }

        CanvasManager.Instance.ShowRanking(ranking);
    }
}
