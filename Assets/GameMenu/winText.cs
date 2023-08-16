using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class winText : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;
    scoreDisplay scoreDisplay;
    string timeString;

    public string Wintext;

    void Awake()
    {
        scoreDisplay = GameObject.FindGameObjectWithTag("timeScore").GetComponent<scoreDisplay>();
        timeString = scoreDisplay.timeString;
        txt.text = Wintext + "\nTime taken: " + timeString;
        //Destroy(GameObject.FindGameObjectWithTag("timeScore"));
        //Destroy(GameObject.FindGameObjectWithTag("PlayerStat"));

    }

}
