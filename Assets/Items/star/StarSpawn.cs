using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarSpawn : MonoBehaviour
{   
    public GameObject star;
    public int spawnRate = 3600;
    public float count = 3550;

    Grid grid;
    public GameObject gridObject;
    List<Vector3> spawnableItemCell;
    List<Vector3> tempList;

    // Start is called before the first frame update
    void Start()
    {
        tempList = new List<Vector3>();
        grid = gridObject.GetComponent<Grid>();
    }

    void Update()
    {
        count += Time.deltaTime * 60;
        if (count >= spawnRate)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        print("spawn");
        if (tempList.Count>0) //not empty
        {
            grid.spawnableItemCell.Add(tempList[0]);
            tempList.Clear();
        }
        spawnableItemCell = grid.spawnableItemCell;
        int v = (int)Random.Range(0f, spawnableItemCell.Count);
        // Instantiate(cherry, new Vector3(Random.Range(Player.position.x - 5, Player.position.x + 5),Player.position.y-0f,Random.Range(Player.position.z - 5, Player.position.z + 5)), Quaternion.Euler(new Vector3(-90,0,0)));
        Instantiate(star, new Vector3(spawnableItemCell[v].x, spawnableItemCell[v].y - 1, spawnableItemCell[v].z), Quaternion.Euler(new Vector3(90, Random.Range(0, 360f), 0)));
        //remove at v
        spawnableItemCell.RemoveAt(v);
        //add to temp stack
        tempList.Add(spawnableItemCell[v]);
        this.count = 0;
    }

}
