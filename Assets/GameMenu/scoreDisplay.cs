using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class scoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;
    public PlayerStat ps;
    float timer = 0.0f;
    string secondString;
    string minuteString;
    public string timeString;

    void Start()
    {
        timer = 0f;
        timeString = "";
    }

    // Update is called once per frame
    void Update()
    {
        //System.TimeSpan gameDuration = System.DateTime.Now - gameStartTime;
        if (!ps.gameStop)
        {
            timer += Time.deltaTime;
            int seconds = (int)timer % 60;
            if (seconds < 10)
                secondString = "0" + seconds.ToString();
            else
                secondString = seconds.ToString();
            int minutes = (int)Math.Floor(timer / 60);
            if (minutes < 10)
                minuteString = "0" + minutes.ToString();
            else
                minuteString = minutes.ToString();
        }
        timeString = minuteString + ":" + secondString;
        txt.text = timeString +  "\n Score: " + ps.getScore();
    }
}
