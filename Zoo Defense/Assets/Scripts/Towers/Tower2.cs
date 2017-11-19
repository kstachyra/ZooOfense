using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2 : Tower
{
    public GameObject particle_sys_lion;
    public override void Attack()
    {
        base.Attack();
        GetComponent<Animator>().SetTrigger("Bite");
        GameObject particle = Instantiate(particle_sys_lion);
        particle.transform.position = transform.position;
    }

    public void Update()
    {
        if(focusedEnemy != null)
        {
            var dir = (focusedEnemy.transform.position - transform.position).normalized;
            transform.localScale = new Vector3(dir.x >= 0 ? 1 : -1, 1, 1);
        }
    }
}
