using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSc : MonoBehaviour
{
    private float createTime;
    PlayerStat ps;
    StarSpawn starSpawn;
    public GameObject floatingText;

    void Start()
    {
        this.createTime = Time.time;
        ps = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerStat>();
        starSpawn = GameObject.FindGameObjectWithTag("starSpawner").GetComponent<StarSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - createTime > 80) 
        {
            Destroy(gameObject);
            //add into list
        }
    }

    public void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.name == "Player"){
            GameObject.FindGameObjectWithTag("collectSound").GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
            ps.EatStar();
            showText("Extra points!");
            //call spawn
            starSpawn.Spawn();
        }
        
    }
    public void showText(string textMessage)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = textMessage;
    }
}
