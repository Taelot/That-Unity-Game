using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerStat : MonoBehaviour
{
    // Start is called before the first frame update
    private float hp = 1800;
    private float time;
    private int score;
    private int winScore = 50;
    public bool gameStop = false;
    
    private int taskProgress = 0;
    void Start()
    {
        score = 0;
        gameStop = false;
    }

    // Update is called once per frame
    // lose 60 Hp per second
    // player dies in 30 seconds
    void Update()
    {   
        hp = hp > 1800 ? 1800 : hp;
        hp -= Time.deltaTime*60;
        
        if (hp <= 0)
        {
            gameStop = true;
            SceneManager.LoadScene("StarveEnd");
        }
        //print(hp);

        if (score >= winScore)
        {
            //show win screen
            gameStop = true;
            SceneManager.LoadScene("GameWin");
        }

        if (hp <= 400)
        {
            if (!GameObject.FindGameObjectWithTag("starvingSound").GetComponent<AudioSource>().isPlaying)
            {
                GameObject.FindGameObjectWithTag("starvingSound").GetComponent<AudioSource>().PlayOneShot(GameObject.FindGameObjectWithTag("starvingSound").GetComponent<AudioSource>().clip);
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("starvingSound").GetComponent<AudioSource>().isPlaying)
            {
                GameObject.FindGameObjectWithTag("starvingSound").GetComponent<AudioSource>().Stop();
            }
        }
    }
    // extend extra 6s 
    public void EatCherry()
    {
        hp += 360;
        score += 1;
    }
    public void EatBigCherry()
    {
        hp += 720;
        score += 2;
    }

    public void EatMaxCherry()
    {
        hp = 1800;
        score += 2;
    }

    public void EatBadCherry()
    {
        hp -= 180;
        score += 2;
    }

    public float getHp()
    {
        return hp;
    }
    public int getScore()
    {
        return score;
    }

    public void EatStar()
    {
        score += 10;
        //print(score);
    }
    public void CompleteTask()
    {
        taskProgress += 1;
    }
    public int GetTask()
    {
        return taskProgress;
    }
    
}
