using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    public Action<Enemy> onEnemyEnter;
    public Action<Enemy> onEnemyExit;
    private bool isCheckingStaying = true;


    private void Start()
    {
        StartCoroutine(WaitAndDisableStaying());
    }

    IEnumerator WaitAndDisableStaying()
    {
        yield return new WaitForSeconds(0.1f);
        isCheckingStaying = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            onEnemyEnter(collision.gameObject.GetComponent<Enemy>());
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(isCheckingStaying && collision.tag == "Enemy")
        {
            onEnemyEnter(collision.gameObject.GetComponent<Enemy>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            onEnemyExit(collision.gameObject.GetComponent<Enemy>());
        }
    }

    public void SetRadius(float radius)
    {
        GetComponent<CircleCollider2D>().radius = radius;
    }
}
