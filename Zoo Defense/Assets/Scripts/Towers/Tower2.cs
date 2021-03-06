﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2 : Tower
{
    public AudioSource lionSource;
    public override void Attack()
    {
        base.Attack();
        GetComponent<Animator>().SetTrigger("Bite");
        lionSource.Play();
    }

    public void Update()
    {
        if(focusedEnemy != null)
        {
            var dir = (focusedEnemy.transform.position - transform.position).normalized;
            transform.localScale = new Vector3(dir.x >= 0 ? 1 : -1, 1, 1);
        }
    }

    public override void SetUpgrade()
    {
        GetComponent<Animator>().SetTrigger("LevelUp");
    }
}
