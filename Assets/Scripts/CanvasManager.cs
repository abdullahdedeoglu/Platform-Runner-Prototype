using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{

    public static CanvasManager Instance { get; private set; }


    //public Canvas canvas;
    public TextMeshProUGUI rankText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ShowRanking();    
    }

    public void ShowRanking(int ranking)
    {
        rankText.text = ranking + ".";
    }


}
