using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDimming : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerStat ps;
    [SerializeField] private Image myImage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        float hp = (float)ps.getHp();
        //Debug.Log(hp);
        float screen_light = (1 - hp/1800 > 0.8f) ? 0.8f : 1 - hp/1800;
        myImage.color = new Color(0.4905f,0f,0f,screen_light);
    }

    // public void OnClick()
    // {
    //     myImage.color += new Color(0f,0f,0f,-0.3f);
    // }
}
