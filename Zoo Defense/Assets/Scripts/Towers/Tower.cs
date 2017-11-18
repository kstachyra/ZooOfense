using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    public float attackPower;
    public float speedFrequency; // lower means better, 0.5 means attack every 0.5s 
    public float range;
    public int cost;
    public Sprite icon;
    public int tileX, tileY;

    protected EnemyFinder enemyFinder;
    protected List<Enemy> enemiesInSight;
    protected bool hasEnemiesInSight = false;
    protected Enemy focusedEnemy;

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
                focusedEnemy = null;
                yield return null;
                continue;
            }
            Debug.Log("atack!");

            Attack();

            yield return new WaitForSeconds(speedFrequency);
        }
    }

    public virtual void Attack()
    {
        FocusEnemy();
        DealDamage();
    }

    public virtual void DealDamage()
    {
        if(focusedEnemy != null)
        {
            focusedEnemy.DealDamage(attackPower);
        }
    }

    public virtual void FocusEnemy()
    {
        while(enemiesInSight.Count > 0 && enemiesInSight[0] == null && !enemiesInSight[0].IsDead())
        {
            enemiesInSight.RemoveAt(0);
        }

        if(enemiesInSight.Count > 0)
        {
            focusedEnemy = enemiesInSight[0];
        }
        else
        {
            hasEnemiesInSight = false;
            focusedEnemy = null;
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
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("towerClicked");
            UIManager.instance.SetSellCanvas(DestroyTower, transform.position);
            UIManager.instance.DisableTowerClick();
        }
    }

    public void DestroyTower()
    {
        GameManager.instance.RemoveTower(this);
        UIManager.instance.HideSellButton();
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
