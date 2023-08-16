using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class loseText : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;
    scoreDisplay scoreDisplay;

    
    string timeString;
    public string deathQuote;

    void Awake()
    {   
       
        scoreDisplay = GameObject.FindGameObjectWithTag("timeScore").GetComponent<scoreDisplay>();
        timeString = scoreDisplay.timeString;
        txt.text = deathQuote + "\nTime of death: " + timeString+"\nScore: "+ scoreDisplay.ps.getScore(); 
    }
}
