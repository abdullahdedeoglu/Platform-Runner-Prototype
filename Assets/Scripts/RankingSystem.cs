using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    public List<Transform> players; // Oyuncu ve AI karakterlerini ekle
    public Transform finishLine; // Bitiþ çizgisinin transformu

    void Update()
    {
        // Karakterleri bitiþ çizgisine olan uzaklýða göre sýralar
        players.Sort((a, b) =>
            Vector3.Distance(a.position, finishLine.position).CompareTo(
                Vector3.Distance(b.position, finishLine.position))
        );

        // Sýralamayý güncelle
        string ranking = "";
        for (int i = 0; i < players.Count; i++)
        {
            ranking += (i + 1) + ". " + players[i].name + "\n";
        }

        CanvasManager.Instance.ShowRanking(ranking);
    }
}
