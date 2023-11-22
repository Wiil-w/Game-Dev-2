using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    [SerializeField] Sprite[] wallSprites;
    [SerializeField] TileSprite wallPrefab;
    [SerializeField] GameObject collectablePrefab;

    private List<TileSprite> walls = new();
    private List<GameObject> collectables = new();

    private int setMapWidth;
    private int setMapHeight;

    [SerializeField] int mapWidth = 50;
    [SerializeField] int mapHeight = 50;
    [SerializeField] float spaceSize = 1f;
    [SerializeField] int spriteScale = 3;

    private int[,] map;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            setMapHeight = mapHeight;
            setMapWidth = mapWidth;
            Reset();
            CreateRandomMap();
            CreateWalls();
            // CreateCollectables();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            setMapHeight = 11;
            setMapWidth = 20;
            Reset();
            CreateDefaultMap();
            CreateWalls();
            // CreateCollectables();
        }
    }

    private void CreateRandomMap()
    {
        // Create a new map
        map = new int[setMapWidth, setMapHeight];

        // Initialize the random map with the edges as walls
        for (int x = 0; x < setMapWidth; x++)
            for (int y = 0; y < setMapHeight; y++)
            {
                // If the position is on the edge of the map it is a wall
                if (x == 0 || x == setMapWidth - 1 || y == 0 || y == setMapHeight - 1) map[x, y] = 1;
                else map[y, x] = 0;
            }
    }

    private void CreateDefaultMap()
    {
        map = new int[,] {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
            {1,0,1,1,0,1,0,1,1,1,1,1,1,0,1,0,1,1,0,1},
            {1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1},
            {1,0,1,0,1,1,0,1,1,1,1,1,1,0,1,1,0,1,0,1},
            {1,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,1},
            {1,0,1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1,0,1},
            {1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1},
            {1,0,1,1,0,1,0,1,1,1,1,1,1,0,1,0,1,1,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };
    }

    private void CreateWalls()
    {
        // Loop through the map and create a wall where there is a 1
        for (int x = 0; x < setMapWidth; x++)
            for (int y = 0; y < setMapHeight; y++)
                if (map[y, x] == 1)
                {
                    TileSprite wall = Instantiate(wallPrefab, new Vector3(x * spaceSize, y * spaceSize, 0), Quaternion.identity);
                    wall.SetSprite(SelectWallSprite(x, y));
                    wall.SetScale(spriteScale);
                    wall.transform.parent = transform;
                    walls.Add(wall);
                }
    }

    private Sprite SelectWallSprite(int x, int y) {
        // Get the surrounding walls
        int top = y < setMapHeight - 1 ? map[y + 1, x] : 0;
        int bottom = y > 0 ? map[y - 1, x] : 0;
        int left = x > 0 ? map[y, x - 1] : 0;
        int right = x < setMapWidth - 1 ? map[y, x + 1] : 0;

        // Calculate the position in the sprite array
        int pos = top * 8 + bottom * 4 + left * 2 + right;
        return wallSprites[pos];
    }


    private void CreateCollectables()
    {
        // Loop through the map and create a collectable where there is a 0
        for (int x = 0; x < setMapWidth; x++)
            for (int y = 0; y < setMapHeight; y++)
                if (map[y, x] == 0)
                {
                    GameObject collectable = Instantiate(collectablePrefab, new Vector3(x * spaceSize, y * spaceSize, 0), Quaternion.identity);
                    collectable.transform.parent = transform;
                    collectables.Add(collectable);
                }
    }

    private void Reset()
    {
        // Destroy all walls
        foreach (TileSprite wall in walls) Destroy(wall.gameObject);
        walls.Clear();

        // Destroy all collectables
        foreach (GameObject collectable in collectables) Destroy(collectable);
        collectables.Clear();
    }
}
