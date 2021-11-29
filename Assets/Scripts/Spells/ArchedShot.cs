using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 11.2021
public class ArchedShot : Spell
{
    HeroController tempHeroController;
    GameObject projectileGO;
    public Vector3 offset = new Vector3(0, 1, 0);
    private float travelTime;
    public float primaryDamagePercent;

    //Hits enemy in the back for 120% damage

    public override void Cast(GameObject caster, GameObject target)
    {
        tempHeroController = caster.GetComponent<HeroController>();
        target = tempHeroController.FurthestEnemy(tempHeroController.EnemyTeam);
        travelTime = Vector3.Distance(caster.transform.position, target.transform.position) * 0.15f;
        tempHeroController.CastAnimation();
        tempHeroController.SetIsAttacking(true);
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(WaitForAnimation(caster, target));
    }
    IEnumerator WaitForAnimation(GameObject caster, GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        tempHeroController.AttackAnimation();
        projectileGO = Instantiate(Effects[1], caster.transform.position + offset, caster.transform.rotation);
        projectileGO.transform.DOMove(Vector3.Lerp(caster.transform.position , target.transform.position , 0.5f) + (Vector3.up * 2) , travelTime / 2);
        yield return new WaitForSeconds(travelTime / 2);
        projectileGO.transform.DOMove(target.transform.position + offset, travelTime / 2);
        Destroy(projectileGO, (travelTime/2) + 0.5f);
        StartCoroutine(TravelTime(caster , target , tempHeroController));
    }

    IEnumerator TravelTime(GameObject caster , GameObject target , HeroController tempHeroController)
    {
        Hero casterHero = caster.GetComponent<Hero>();
        Hero targetHero = target.GetComponent<Hero>();
        yield return new WaitForSeconds(travelTime);
        GameObject splashGO = Instantiate(Effects[2], target.transform.position + offset, Quaternion.identity);
        targetHero.Hurt((int)(casterHero.Damage * (primaryDamagePercent / 100f)));
        Destroy(splashGO, 0.3f);
        tempHeroController.SetIsAttacking(false);
    }
}
