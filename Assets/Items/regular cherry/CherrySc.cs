using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherrySc : MonoBehaviour
{
    // Start is called before the first frame update
    private float createTime;
    public GameObject floatingText;
    PlayerStat ps;

    //EatcherryScript canvas;
    void Start()
    {
        this.createTime = Time.time;
        //canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<EatcherryScript>();
        ps = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - createTime > 15) 
        {
            Destroy(gameObject);
            //add into list
        }
        // if collide then eat and destroy
    }

    public void OnCollisionEnter(Collision collision)
    {   
        // if (collision.transform.name =="Player Controller" ) {
        //     Destroy(gameObject);
        // }
        //Debug.Log("Hello: ");
        if (collision.gameObject.name == "Player"){
            GameObject.FindGameObjectWithTag("eatSound").GetComponent<AudioSource>().Play();
            ps.EatCherry();
            showText("Gained health!");
            Destroy(this.gameObject);
        }
        
        //call on screen dimming function
    }

    public void showText(string textMessage)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = textMessage;
    }
}
