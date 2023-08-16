using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCherrySc : MonoBehaviour
{
    // Start is called before the first frame update
    private float createTime;
    PlayerStat ps;
    public GameObject floatingText;
    PlayerController playerController;
    //EatcherryScript canvas;
    void Start()
    {
        this.createTime = Time.time;
        //canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<EatcherryScript>();
        ps = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerStat>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - createTime > 20)
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
        if (collision.gameObject.name == "Player")
        {
            GameObject.FindGameObjectWithTag("eatSound").GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
            randomBoost(randomHealthEffect());
        }

        //call on screen dimming function
    }

    public void showText(string textMessage)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = textMessage;
    }
    private string randomHealthEffect()
    {

        float v = Random.Range(0f, 10f);
        if (v < 1)
        {
            ps.EatBadCherry();
            print("Cherry was rotton, Lost Health");
            return "Cherry was rotton, Lost Health";
        }
        else if (v < 3 && v >= 1)
        {
            ps.EatMaxCherry();
            print("full");
            return "Full Hp!";
        }
        else
        {
            ps.EatBigCherry();
            print("ext");
            return "Extra health!";
        }
    }
    private void randomBoost(string text)
    {
        float j = Random.Range(0f, 10f);
        if (j < 1)
        {
            playerController.speedDebuff();
            showText(text + "\nSlowed");
            print("slow");
        }
        else if (j < 3 && j >= 1)
        {
            showText(text + "\nSpeed Boost!");
            playerController.speedBuff();
            print("fast");
        }
        else
        {
            showText(text);
        }
    }
}
