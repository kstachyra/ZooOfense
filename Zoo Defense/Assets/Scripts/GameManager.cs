using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int money = 3000;

    public Tower[] towers;
    public GameObject tilePrefab;
    public int xSize, ySize;
    Tile[,] map;

    void Awake()
    {
        instance = this;
        map = new Tile[ySize, xSize];
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
            towers[towerID].cost <= money)
        {
            map[x, y].AddTower(towers[towerID]);
            money -= towers[towerID].cost;
            UpdateUI();
        }
    }

    public void AddMoney(int money)
    {
        this.money += money;
        UpdateUI();
    }
}
