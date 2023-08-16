using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSc : MonoBehaviour
{
    private float createTime;
    PotionSpawn potionSpawn;
    PlayerJump playerJump;
    public GameObject floatingText;

    void Start()
    {
        this.createTime = Time.time;
        potionSpawn = GameObject.FindGameObjectWithTag("potionSpawner").GetComponent<PotionSpawn>();
        playerJump = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerJump>();
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
            //call spawn
            playerJump.eatPotion();
            potionSpawn.Spawn();
            showText("Increased Jump!");
        }
        
    }
    public void showText(string textMessage)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = textMessage;
    }
}
