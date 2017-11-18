using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float life;
    public int moneyDrop;
    public GameObject cage;

    private Queue<Node> wayPoints;
    private bool pathChanged = false;
    private bool died = false;

    public void Start()
    {//temp test
        GameManager.onMapChange += OnRecalculatePath;
    }

    private void SetPath(Queue<Node> wayPoints)
    {
        this.wayPoints = wayPoints;
        wayPoints.Dequeue();
        MoveTo(this.wayPoints.Dequeue());
    }

    private void MoveTo(Node targetPosition)
    {
        GameManager.instance.enemiesCount[targetPosition.X, targetPosition.Y]++;

        transform.localScale = new Vector3(targetPosition.X - transform.localPosition.x > 0 ? 1 : -1, 1, 1);

        transform.DOMove(new Vector3(targetPosition.X, targetPosition.Y, transform.position.z), 1 / speed).SetEase(Ease.Linear).OnComplete(() =>
              {
                  if(wayPoints.Count == 0)
                  {
                      HurtPlayer();
                  }
                  else
                  {
                      GameManager.instance.enemiesCount[targetPosition.X, targetPosition.Y]--;
                      if(!pathChanged)
                          MoveTo(wayPoints.Dequeue());
                      else
                      {
                          CalculatePath(targetPosition, GameManager.destination);
                          pathChanged = false;
                      }
                  }
              });
    }

    public void CalculatePath(Node from, Node to)
    {
        SetPath(new Queue<Node>(Astar.CalcPath(from, to).AsEnumerable().Reverse()));
    }

    public void DealDamage(float damage)
    {
        life -= damage;
        Debug.Log("Ouch " + life);
        if(life <= 0 && !died)
        {
            Die();
        }
    }

    private void Die()
    {
        died = true;
        PlayDeathAnimation();
    }

    private void PlayDeathAnimation()
    {
        transform.DOKill();
        cage.SetActive(true);
        GetComponent<Animator>().enabled = false;
        transform.DOScale(Vector3.zero, 0.4f).SetDelay(0.4f).OnComplete(() =>
        {
            Destroy(gameObject);
            GameManager.instance.AddMoney(moneyDrop);
        });
    }

    private void HurtPlayer()
    {
        Destroy(gameObject);
    }

    void OnRecalculatePath()
    {
        pathChanged = true;
    }

    public bool IsDead()
    {
        return died;
    }
}
