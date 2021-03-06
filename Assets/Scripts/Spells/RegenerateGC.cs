using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 11.2021
public class RegenerateGC : Spell
{
    public float HealAmountPercent;
    public override void CastWithPosition(Vector3 positionToCast)
    {
        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
        GameObject projectile = Instantiate(Effects[1], positionToCast, spawnRotation);
        projectile.transform.localScale = Vector3.one * 0.4f * AOERange;
        Destroy(projectile, 0.5f);
        StartCoroutine(CastingLag(positionToCast));
    }
    IEnumerator CastingLag(Vector3 positionToCast)
    {
        yield return new WaitForSeconds(0.4f);
        
        Collider[] inAOE = Physics.OverlapSphere(positionToCast, AOERange);
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            foreach (Collider hero in inAOE)
            {
                if (hero.transform.tag == "Team1")
                {
                    Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
                    GameObject splash = Instantiate(Effects[0], hero.transform.position, spawnRotation);
                    splash.transform.localScale = Vector3.one * 0.4f * AOERange;
                    Destroy(splash, 0.5f);
                    hero.GetComponent<Hero>().Health += (int)(hero.GetComponent<Hero>().BaseHealth * (HealAmountPercent/100f));
                    hero.GetComponent<Hero>().Normalise();
                }
            }
        }
        
    }
}
