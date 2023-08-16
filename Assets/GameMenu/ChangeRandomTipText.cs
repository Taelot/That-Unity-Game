using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRandomTipText : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> tips = new List<string>();
    void Start()
    {
        tips.Add("Not all blue cherries are good cherries");
        tips.Add("Collect three mistery tools to active trapdoor!");
        tips.Add("Boots allow you walk through mud");
        tips.Add("Potions make you jump Hiiiiigher");
        tips.Add("Always go for the stars");
        tips.Add("Collect 50 points to win!");
        tips.Add("Don't fall into water, you can't swim");
        tips.Add("Mistery tools spwan when you have some points");
        tips.Add("Where does the trapdoor leads to? God Knows!");
        tips.Add("When you are red, eat red or eat dead");
        int num = Random.Range(0, tips.Count);
        this.GetComponent<Text>().text = tips[num];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
