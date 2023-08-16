using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bootsSpawner : MonoBehaviour
{
    public GameObject boot;
    public int count = 0;

    Grid grid;
    public GameObject gridObject;
    List<Vector3> spawnableItemCell;
    List<Vector3> tempList;

    // Start is called before the first frame update
    void Start()
    {
        tempList = new List<Vector3>();
        grid = gridObject.GetComponent<Grid>();
        InvokeRepeating("Spawn", 0f, 25f);
        InvokeRepeating("Spawn", 10f, 25f);
        InvokeRepeating("addBackToSpawnableItemList", 30f, 25f);
        InvokeRepeating("addBackToSpawnableItemList", 40f, 25f);
    }

    public void Spawn()
    {
        spawnableItemCell = grid.spawnableItemCell;
        int v = (int)Random.Range(0f, spawnableItemCell.Count);
        Instantiate(boot, new Vector3(spawnableItemCell[v].x, spawnableItemCell[v].y - 1, spawnableItemCell[v].z), Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0)));
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
