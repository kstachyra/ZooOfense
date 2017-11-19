using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int x, y;
    private Action<int, int> onTileClick;
    private Tower tower;

    public void Init(int x, int y, Action<int, int> onTileClick)
    {
        UIManager.onTowerUIChange += onTowerUIChange;

        this.x = x;
        this.y = y;
        this.onTileClick = onTileClick;
    }

    public void OnMouseDown()
    {
        if(onTileClick != null)
        {
            onTileClick(x, y);
        }
    }

    public bool HasTower()
    {
        return tower != null;
    }

    public void AddTower(Tower tower)
    {
        GameObject go = Instantiate(tower, transform).gameObject;
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = new Vector3(0, 0, -1);
        var tempTower = go.GetComponent<Tower>();
        tempTower.InitTower(x, y);
        GameManager.instance.towersMap[tempTower.tileX, tempTower.tileY] = true;
        GameManager.RecalculatePaths();
        //Recalculate Paths

        this.tower = tower;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void RemoveTower()
    {
        tower = null;
    }

    public void onTowerUIChange(bool status)
    {
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().enabled = status && !HasTower();
    }

    public void OnDestroy()
    {
        UIManager.onTowerUIChange -= onTowerUIChange;
    }
}