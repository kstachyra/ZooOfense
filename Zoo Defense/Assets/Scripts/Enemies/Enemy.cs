using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float life;
    public int moneyDrop;

    private Tile destination;
    private Queue<Vector2> wayPoints;

    public void SetDestination(Tile tile)
    {
        destination = tile;
    }

    public void Start()
    {//temp test
        Queue<Vector2> stack = new Queue<Vector2>();
        stack.Enqueue(new Vector2(3, 4));
        stack.Enqueue(new Vector2(3, 3));
        stack.Enqueue(new Vector2(3, 2));
        stack.Enqueue(new Vector2(3, 1));
        stack.Enqueue(new Vector2(3, 0));
        stack.Enqueue(new Vector2(2, 0));
        stack.Enqueue(new Vector2(1, 0));
        stack.Enqueue(new Vector2(0, 0));

        SetPath(stack);
    }

    public void SetPath(Queue<Vector2> wayPoints)
    {
        this.wayPoints = wayPoints;
        MoveTo(this.wayPoints.Dequeue());
    }

    private void MoveTo(Vector2 point)
    {
        GameManager.instance.enemiesCount[(int)point.x, (int)point.y]++;

        transform.localScale = new Vector3(point.x - transform.localPosition.x > 0 ? 1 : -1, 1, 1);

        transform.DOMove(point, 1 / speed).SetEase(Ease.Linear).OnComplete(() =>
              {
                  if(wayPoints.Count == 0)
                  {
                      HurtPlayer();
                  }
                  else
                  {
                      GameManager.instance.enemiesCount[(int)point.x, (int)point.y]--;
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
