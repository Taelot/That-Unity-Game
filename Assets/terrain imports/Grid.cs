using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public GameObject treePrefabs;
    public GameObject grassPrefabs;
    public Material terrainMaterial;
    public Material edgeMaterial;
    public float waterLevel = .3f;
    public float sandLevel = .33f;
    public float scale = .07f;
    public int size = 180;
    public float treeNoiseScale = 0.1f; // how many clusters
    public float treeDensity = 0.15f; // how big the clusters are
    public float grassNoiseScale = 0.2f; // how many clusters
    public float grassDensity = 0.3f; // how big the clusters are
    Cell[,] grid;
    List<Vector3> mudCell;
    List<Vector3> spawnCell;
    List<Vector3> sandCell;
    public List<Vector3> spawnableItemCell;

    public Color mudColor;
    public Color grassColor;
    public Color sandColor;

    public float mudNoiseScale = 0.05f; // how many clusters
    public float mudDensity = 0.5f; // how big the clusters are


    public Transform Player;
    PlayerController playerController;
    public GameObject playerObject;
    PlayerAnimationController playerAnimationController;

    void Awake()
    {
        playerController = playerObject.GetComponent<PlayerController>();
        playerAnimationController = playerObject.transform.GetChild(0).GetComponent<PlayerAnimationController>();
    }

    void Start()
    {
        spawnableItemCell = new List<Vector3>();
        spawnCell = new List<Vector3>();
        mudCell = new List<Vector3>();
        sandCell = new List<Vector3>();
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                bool isSand = false;
                if (noiseValue > waterLevel && noiseValue < sandLevel)
                {
                    isSand = true;
                    Vector3 sandPosition = new Vector3(x, 2, y);
                    sandCell.Add(sandPosition);
                }
                Cell cell = new Cell(isWater, false, false, isSand);
                if (noiseValue > 0.8f)
                {
                    Vector3 spawnPosition = new Vector3(x, 2, y);
                    spawnCell.Add(spawnPosition);
                }
                grid[x, y] = cell;
            }
        }

        DrawTerrainMesh(grid);
        DrawEdgeMesh(grid);
        GenerateMud(grid);
        GenerateTree(grid);
        GenerateGrass(grid); //do last
        getSpawnableCells(grid);
        DrawTexture(grid);
        gameObject.AddComponent<MeshCollider>();
        if (!(Player == null))
        {
            SpawnPlayer(grid);
        }
    }

    void DrawTerrainMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                    Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                    Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                    Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                    Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                    Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
                    Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                    Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        BoxCollider waterColider = gameObject.AddComponent<BoxCollider>();
        waterColider.center = new Vector3(90, -1.8f, 90);
    }

    void DrawEdgeMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, y];
                        if (left.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y > 0)
                    {
                        Cell down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y < size - 1)
                    {
                        Cell up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObj = new GameObject("Edge");
        edgeObj.transform.SetParent(transform);

        MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;
        edgeObj.AddComponent<MeshCollider>();
    }

    void GenerateMud(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * mudNoiseScale + xOffset, y * mudNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater && !cell.isSand)
                {
                    float v = Random.Range(0f, mudDensity);
                    if (noiseMap[x, y] < v)
                    {
                        //create object
                        cell.isMud = true;
                        Vector3 mudPosition = new Vector3(x, 2, y);
                        mudCell.Add(mudPosition);
                    }
                }
            }
        }
    }

    void GenerateTree(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * treeNoiseScale + xOffset, y * treeNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater && !cell.isMud && !cell.hasObject && !cell.isSand)
                {
                    float v = Random.Range(0f, treeDensity);
                    if (noiseMap[x, y] < v)
                    {
                        //create object
                        cell.hasObject = true;
                        GameObject objectItem = Instantiate(treePrefabs, transform);
                        objectItem.transform.position = new Vector3(x, 0, y);
                        objectItem.transform.Rotate(0f, 0f, Random.Range(0, 360f), Space.Self);
                    }
                }
            }
        }
    }

    void GenerateGrass(Cell[,] grid)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * grassNoiseScale + xOffset, y * grassNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater && !cell.isMud && !cell.hasObject && !cell.isSand)
                {
                    float v = Random.Range(0f, grassDensity);
                    if (noiseMap[x, y] < v)
                    {
                        //create object
                        //cell.hasObject = true;
                        GameObject grassItem = Instantiate(grassPrefabs, transform);
                        grassItem.transform.position = new Vector3(x, 0, y);
                        grassItem.transform.Rotate(0f, Random.Range(0, 360f), 0f, Space.Self);
                    }
                }
            }
        }
    }
    void DrawTexture(Cell[,] grid)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    colorMap[y * size + x] = Color.blue;
                if (cell.isMud == true)
                    colorMap[y * size + x] = mudColor;
                if (cell.isSand == true)
                    colorMap[y * size + x] = sandColor;
                if (!cell.isSand && !cell.isMud && !cell.isWater)
                    colorMap[y * size + x] = grassColor; //change color of land
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }
    void SpawnPlayer(Cell[,] grid)
    {
        int v = (int)Random.Range(0f, spawnCell.Count);
        if (spawnableItemCell.Contains(spawnCell[v]))
        {
            Player.transform.position = (spawnCell[v]);
        }
        else
        {
            SpawnPlayer(grid);
        }

    }



    void Update()
    {
        bool notOnGrassAnim = true;
        for (int i = 0; i < mudCell.Count; i++)
        {
            if ((Player.transform.position.x < mudCell[i].x + 0.5 && Player.transform.position.x > mudCell[i].x - 0.5) && (Player.transform.position.z < mudCell[i].z + 0.5 && Player.transform.position.z > mudCell[i].z - 0.5) && (Player.transform.position.y < 0.1)) //make it so that player can jump over
            {
                // print("in mud");
                playerController.onMud();
                notOnGrassAnim = false;
                playerAnimationController.onMud();

            }
            else
            {
                playerController.onGrass();
            }
        }

        for (int i = 0; i < sandCell.Count; i++)
        {
            if ((Player.transform.position.x < sandCell[i].x + 0.5 && Player.transform.position.x > sandCell[i].x - 0.5) && (Player.transform.position.z < sandCell[i].z + 0.5 && Player.transform.position.z > sandCell[i].z - 0.5) && (Player.transform.position.y < 0.1)) //make it so that player can jump over
            {
                // print("on sand");
                playerController.onSand();
                notOnGrassAnim = false;
                playerAnimationController.onSand();
            }
            else
            {
                playerController.onGrass();
            }
        }
        if (notOnGrassAnim)
        {
            playerAnimationController.onGrass();

        }
    }

    void getSpawnableCells(Cell[,] grid)
    {
        spawnableItemCell = new List<Vector3>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater && !cell.hasObject)
                {
                    Vector3 cellPosition = new Vector3(x, 2, y);
                    spawnableItemCell.Add(cellPosition);
                }
            }
        }

    }
}