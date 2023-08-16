using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTaskScript : MonoBehaviour
{   
    public GameObject shovel;
    public GameObject pickaxe;

    public GameObject trapdoor;
    public GameObject screwdriver;
    private  int count;

    Grid grid;
    public GameObject gridObject;
    List<Vector3> spawnableItemCell;
    List<Vector3> tempList;

    PlayerStat ps;

    // Start is called before the first frame update
    void Start()
    {   
        Debug.Log("start check");
        count = 0;
        tempList = new List<Vector3>();
        grid = gridObject.GetComponent<Grid>();
        ps = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerStat>();
    }
    void Update()
    {   
        if (ps.getScore() >= 4 && count == 0)
        {   
            count += 1;
            Spawn(shovel);
        }
        if (ps.getScore() >= 8 && count == 1) 
        {
            count += 1;
            Spawn(pickaxe);
        }
        if (ps.getScore() >= 12 && count == 2)
        {
            count += 1;
            Spawn(screwdriver);
        }
        if (ps.getScore() >= 20 && count == 3)
        {
            count += 1;
            Spawn(trapdoor);
        }
        
    }
    public void Spawn(GameObject target)
    {
        spawnableItemCell = grid.spawnableItemCell;
        int v = (int)Random.Range(0f, spawnableItemCell.Count);
        // Instantiate(cherry, new Vector3(Random.Range(Player.position.x - 5, Player.position.x + 5),Player.position.y-0f,Random.Range(Player.position.z - 5, Player.position.z + 5)), Quaternion.Euler(new Vector3(-90,0,0)));
        
        if (target == shovel)
        {
            Instantiate(target, new Vector3(spawnableItemCell[v].x, spawnableItemCell[v].y - 1, spawnableItemCell[v].z), Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0)));
        }
        else
        {
            Instantiate(target, new Vector3(spawnableItemCell[v].x, spawnableItemCell[v].y - 1, spawnableItemCell[v].z), Quaternion.Euler(new Vector3(-90, Random.Range(0, 360f), 0)));
        }
        //remove at v
        spawnableItemCell.RemoveAt(v);
        //add to temp stack
        tempList.Add(spawnableItemCell[v]);
    }

    public void addBackToSpawnableItemList()
    {
        //print("add back to list");
        spawnableItemCell = grid.spawnableItemCell;
        //push first item in temp stack to grid
        grid.spawnableItemCell.Add(tempList[0]);
        //remove first item in temp stack
        tempList.RemoveAt(0);
    }
}