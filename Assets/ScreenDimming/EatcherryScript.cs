using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EatcherryScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Image myImage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        if (myImage.color.a >= 0.3){
            myImage.color += new Color(0f,0f,0f,-0.3f);
        }
        else {
            myImage.color -= new Color(0f,0f,0f,myImage.color.a);
        }
    }

    public void test(){
        
    }
}
