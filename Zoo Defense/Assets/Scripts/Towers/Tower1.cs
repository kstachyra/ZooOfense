using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tower1 : Tower
{
    public Transform hand;
    public GameObject kula_Pingwin;
	public GameObject particle_sys_ping;

    public override void Attack()
    {
        base.FocusEnemy();
        GameObject kula = Instantiate(kula_Pingwin);
        kula.transform.position = transform.position;
		GameObject particle = Instantiate(particle_sys_ping);
		particle.transform.position = transform.position;
        if(focusedEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, focusedEnemy.transform.position);
            kula.transform.DOMove(focusedEnemy.transform.position + new Vector3(0, -0.2f, -0.5f), 0.01f * distance).SetEase(Ease.InSine).OnComplete(() =>
              {
                  kula.transform.DOScale(Vector3.zero, 0.05f).OnComplete(() =>
                   {
                       kula.transform.GetComponent<SpriteRenderer>().enabled = false;
                       kula.transform.GetChild(0).gameObject.SetActive(true);
                       kula.transform.DOScale(Vector3.one, 0.2f).OnComplete(() => { Destroy(kula); base.DealDamage(); });
                   });
              });
        }
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
