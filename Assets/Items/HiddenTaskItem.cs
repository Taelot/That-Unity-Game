using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HiddenTaskItem : MonoBehaviour
{
    // Start is called before the first frame update
    private float createTime;
    public GameObject floatingText;
    PlayerStat ps;
    void Start()
    {
        createTime = Time.time;
        ps = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - createTime > 90) 
        {
            Destroy(gameObject);
        }
        
    }

    public void OnCollisionEnter(Collision collision)
    {   
        
        if (collision.gameObject.name == "Player"){
            
            if (gameObject.name == "Trapdoor(Clone)")
            {
                if (ps.GetTask() == 3){
                    SceneManager.LoadScene("EscapeEnd");
                }
                else 
                {
                    showText("Collect all 3 tools to unlock");
                }
                
            }
            else
            {
                showText("Obtained "+gameObject.name.Remove(gameObject.name.Length - 7));
                GameObject.FindGameObjectWithTag("collectSound").GetComponent<AudioSource>().Play();
                ps.CompleteTask();
                Destroy(this.gameObject);
            }
        }
        
        
    }

    public void showText(string textMessage)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        var text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.GetComponent<TextMesh>().text = textMessage;
    }
}
