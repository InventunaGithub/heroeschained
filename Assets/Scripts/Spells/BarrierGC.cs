using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 9.11.2021
public class BarrierGC : Spell
{
    public int Damage;
    public override void CastWithPosition(Vector3 positionToCast)
    {
        GameObject projectile = Instantiate(Effects[1], positionToCast + (Vector3.up * 5) + Vector3.forward * -3, Quaternion.identity);
        projectile.transform.localScale = Vector3.one * 0.4f * AOERange;
        projectile.transform.DOMove(positionToCast, 0.4f);
        Destroy(projectile, 0.5f);
        StartCoroutine(CastingLag(positionToCast));
    }
    IEnumerator CastingLag(Vector3 positionToCast)
    {
        yield return new WaitForSeconds(0.4f);
        GameObject splash = Instantiate(Effects[0], positionToCast, Quaternion.identity);
        splash.transform.localScale = Vector3.one * 0.4f * AOERange;
        Destroy(splash, 0.5f);
        Collider[] inAOE = Physics.OverlapSphere(positionToCast, AOERange);
        foreach (Collider hero in inAOE)
        {
            if (hero.transform.tag == "Team2")
            {
                hero.GetComponent<Hero>().Hurt(Damage);
            }
        }
    }
}
