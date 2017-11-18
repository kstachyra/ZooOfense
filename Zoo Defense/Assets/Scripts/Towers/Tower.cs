using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attackPower;
    public float speedFrequency; // lower means better, 0.5 means attack every 0.5s 
    public float range;
    public int cost;
    public Sprite icon;
    public int tileX, tileY;

    private EnemyFinder enemyFinder;
    private List<Enemy> enemiesInSight;
    private bool hasEnemiesInSight = false;

    public void Awake()
    {
        enemiesInSight = new List<Enemy>();
        enemyFinder = GetComponentInChildren<EnemyFinder>();
        enemyFinder.onEnemyEnter = OnEnemyEnter;
        enemyFinder.onEnemyExit = OnEnemyExit;
        enemyFinder.SetRadius(range);
    }

    IEnumerator WaitAndAttack()
    {
        while(true)
        {
            if(!hasEnemiesInSight)
            {
                yield return null;
                continue;
            }
            Debug.Log("atack!");

            while(enemiesInSight.Count > 0 && enemiesInSight[0] == null)
            {
                enemiesInSight.RemoveAt(0);
            }

            if(enemiesInSight.Count > 0)
            {
                enemiesInSight[0].DealDamage(attackPower);
            }
            else
            {
                hasEnemiesInSight = false;
            }

            yield return new WaitForSeconds(speedFrequency);
        }
    }

    public void InitTower(int x, int y)
    {
        Debug.Log("Tower Created!");
        tileX = x;
        tileY = y;
        enemyFinder.SetRadius(range);

        StartCoroutine(WaitAndAttack());
    }

    public void OnMouseDown()
    {
        Debug.Log("towerClicked");
        //needs some ui display
    }

    public void DestroyTower()
    {
        GameManager.instance.RemoveTower(this);
        Destroy(gameObject);
    }

    public void OnEnemyEnter(Enemy enemy)
    {
        if(!enemiesInSight.Contains(enemy))
        {
            enemiesInSight.Add(enemy);
            hasEnemiesInSight = true;
        }
    }

    public void OnEnemyExit(Enemy enemy)
    {
        if(enemiesInSight.Contains(enemy))
        {
            enemiesInSight.Remove(enemy);
            hasEnemiesInSight = enemiesInSight.Count != 0;
        }
    }

    public void ShowUI()
    {
        //Shows ui do destroy Tower
    }
}
