using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tower1 : Tower
{
    public Transform hand;

    public override void Attack()
    {
        base.Attack();

    }

    public void Update()
    {
        if(focusedEnemy != null)
        {
            var dir = (focusedEnemy.transform.position - transform.position).normalized;
            transform.localScale = new Vector3(dir.x >= 0 ? 1 : -1, 1, 1);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            hand.rotation = Quaternion.AngleAxis(angle + (transform.localScale.x == 1 ? 0 : 180), Vector3.forward);
        }
    }
}
