﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static event Action onMapChange;

    public static Node destination;


    public int lifes = 10;
    public int captured = 0;
    public int money = 3000;
    public EnemySpawner spawner;
    public Tower[] towers;
    public GameObject tilePrefab;
    public int xSize, ySize;
    public bool[,] towersMap;
    public int[,] enemiesCount;
    Tile[,] map;

    public GameObject gameOverWindow;


    void Awake()
    {
        destination = new Node(0, 3);
        Astar.Init(ySize, xSize);
        instance = this;
        map = new Tile[ySize, xSize];
        towersMap = new bool[ySize, xSize];
        enemiesCount = new int[ySize, xSize];
        for(int i = 0; i < xSize; i++)
        {
            enemiesCount[ySize - 1, i] = 1;
        }
        Astar.SetGrid(towersMap);

        GenerateMap(ySize, xSize);
        Camera.main.transform.position = new Vector3((ySize - 3) / 2f, (xSize ) / 2f, -1);
    }

    void Start()
    {
        for(int i = 0; i < towers.Length; i++)
        {
            UIManager.instance.SetTower(i, towers[i].cost, towers[i].icon);
        }
        UpdateUI();
        spawner.StartNewLevel();
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
        towersMap[tower.tileX, tower.tileY] = false;
        RecalculatePaths();
    }

    public void AddMoney(int money)
    {
        this.money += money;
        UpdateUI();
    }

    public void ChangeLifes(int lifes)
    {
        this.lifes += lifes;
        if(this.lifes <= 0)
            GameOver();
    }

    private void GameOver()
    {
        Destroy(spawner);
        Debug.Log("loser!");


        //Okno przegranej i do wybory
        gameOverWindow.SetActive(true);

        //backToMenu();
        //restart();
    }

    private bool IsNotBlockingEnemies(int X, int Y)
    {
        towersMap[X, Y] = true;
        Astar.SetGrid(towersMap);

        for(int y = 0; y < ySize; y++)
        {
            for(int x = 0; x < xSize; x++)
            {
                if(enemiesCount[y, x] > 0)
                {
                    if(Astar.CalcPath(new Node(y, x), destination).Count == 0)
                    {
                        towersMap[X, Y] = false;
                        return false;
                    }
                }
            }
        }

        towersMap[X, Y] = false;
        return true;
    }

    public void restart()
    {
        gameOverWindow.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void backToMenu()
    {
        gameOverWindow.SetActive(false);
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("back", 1);
    }
}