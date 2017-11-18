﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public float speed; // 
    public float life;
    public int moneyDrop;

    private Tile destination;
    private Queue<Vector2> wayPoints;

    public void SetDestination(Tile tile)
    {
        destination = tile;
    }

    private void Start()
    {
        Queue<Vector2> stack = new Queue<Vector2>();
        stack.Enqueue(new Vector2(7, 8));
        stack.Enqueue(new Vector2(6, 8));
        stack.Enqueue(new Vector2(6, 7));
        stack.Enqueue(new Vector2(6, 6));
        stack.Enqueue(new Vector2(6, 5));
        stack.Enqueue(new Vector2(5, 5));

        SetPath(stack);
    }

    public void SetPath(Queue<Vector2> wayPoints)
    {
        this.wayPoints = wayPoints;
        MoveTo(this.wayPoints.Dequeue());
    }

    private void MoveTo(Vector2 point)
    {
        transform.DOMove(point, 1 / speed).SetEase(Ease.Linear).OnComplete(() =>
              {
                  if(wayPoints.Count == 0)
                  {
                      HurtPlayer();
                  }
                  else
                  {
                      MoveTo(wayPoints.Dequeue());
                  }
              });
    }

    public void DealDamage(float damage)
    {
        life -= damage;
        Debug.Log("Ouch " + life);
        if(life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.AddMoney(moneyDrop);
        Destroy(gameObject);
    }

    private void HurtPlayer()
    {

        Destroy(gameObject);
    }
}
