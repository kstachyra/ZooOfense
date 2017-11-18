using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static event Action onMapChange;

    public int money = 3000;

    public Tower[] towers;
    public GameObject tilePrefab;
    public int xSize, ySize;
    public bool[,] towersMap;
    public int[,] enemiesCount;
    Tile[,] map;

    void Awake()
    {
        Astar.Init(ySize, xSize);
        instance = this;
        map = new Tile[ySize, xSize];
        towersMap = new bool[ySize, xSize];
        enemiesCount = new int[ySize, xSize];
        Astar.SetGrid(towersMap);

        GenerateMap(ySize, xSize);
        Camera.main.transform.position = new Vector3((ySize - 3) / 2f, (xSize - 1) / 2f, -1);
    }

    void Start()
    {
        for(int i = 0; i < towers.Length; i++)
        {
            UIManager.instance.SetTower(i, towers[i].cost, towers[i].icon);
        }
        UpdateUI();
    }

    public static void RecalculatePaths()
    {
        Astar.SetGrid(GameManager.instance.towersMap);
        if(onMapChange != null)
        {
            onMapChange();
        }
    }

    void GenerateMap(int x, int y)
    {
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                GameObject go = Instantiate(tilePrefab, transform);
                go.transform.position = new Vector3(i, j, 10);
                go.name = "Tile (" + i + ", " + j + ")";
                map[i, j] = go.GetComponent<Tile>();
                map[i, j].Init(i, j, onTileClick);
            }
        }
    }

    private void UpdateUI()
    {
        UIManager.instance.SetMoney(money);
    }

    private void onTileClick(int x, int y)
    {
        int towerID = UIManager.selectedTower;
        if(towerID != -1 &&
            !map[x, y].HasTower() &&
            towers[towerID].cost <= money &&
            enemiesCount[x, y] <= 0 &&
            IsNotBlockingEnemies(x, y))
        {
            map[x, y].AddTower(towers[towerID]);
            money -= towers[towerID].cost;
            UpdateUI();
        }
    }

    public void RemoveTower(Tower tower)
    {
        map[tower.tileX, tower.tileY].RemoveTower();
        map[tower.tileX, tower.tileY] = null;
        towersMap[tower.tileX, tower.tileY] = false;
        RecalculatePaths();
    }

    public void AddMoney(int money)
    {
        this.money += money;
        UpdateUI();
    }

    private bool IsNotBlockingEnemies(int x, int y)
    {/*
        bool[,] tempArray = (bool[,])towersMap.Clone();
        tempArray[]
        for(int y = 0; y < ySize;y++)
        {
            for(int x =0; x < xSize; x++)
            {
                temp
            }
        }*/
        return true;
    }
}
