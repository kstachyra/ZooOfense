using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower3 : Tower
{
    public GameObject nutPrefab;

    public override void Attack()
    {
        base.FocusEnemy();
        GetComponent<Animator>().SetTrigger("Shoot");
        StartCoroutine(WaitAndSpawnNut());
    }

    IEnumerator WaitAndSpawnNut()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject kula = Instantiate(nutPrefab);
        kula.transform.position = transform.position;
        if(focusedEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, focusedEnemy.transform.position);
            kula.GetComponent<Rigidbody2D>().AddTorque(1000, ForceMode2D.Force);
            kula.transform.DOMove(focusedEnemy.transform.position + new Vector3(0, -0.2f, -0.5f), 0.03f * distance).SetEase(Ease.InSine).OnComplete(() =>
            {
                kula.transform.DOScale(Vector3.zero, 0.05f).OnComplete(() =>
                {
                    Destroy(kula);
                    base.DealDamage();
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
        }
    }
}
