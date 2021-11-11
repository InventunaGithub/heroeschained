using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChainLightningUltimateSkill : Spell
{
    //Hit the closet enemy for 160% damage and then the next closet enemy for 120% damage then 80%, 40%
    Animator casterAnimator;
    Animator targetAnimator;
    public float PrimaryDamageMultiplier;
    public float SecondaryDamageMultiplier;
    public float ThirdDamageMultiplier;
    public float FourthDamageMultiplier;
    HeroController tempHeroController;
    public override void Cast(GameObject caster, GameObject target)
    {
        tempHeroController = caster.GetComponent<HeroController>();
        tempHeroController.setIsAttacking(true);
        SetCaster(caster);
        SetTarget(target);
        if (GetTarget() == null)
        {
            throw new Exception("No target is selected");
        }
        if (GetCaster() == null)
        {
            throw new Exception("No caster is selected");
        }
        Hero casterHero = caster.GetComponent<Hero>();
        Hero targetHero = target.GetComponent<Hero>();
        CasterAnimator = caster.transform.GetComponentInChildren<Animator>();
        CasterAnimator.CrossFade("Idle", 0.1f);
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero, GameObject caster, GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        if (casterHero.Health > 0)
        {
            GameObject targetGO = null;
            GameObject TemptargetGO = null;
            CasterAnimator.CrossFade("Attack", 0.1f);
            GameObject projectile = Instantiate(Effects[1], caster.transform.position + Vector3.up, Quaternion.identity);
            List<Hero> tempList = new List<Hero>(tempHeroController.EnemyTeam);
            float travelTime = 0; 
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                travelTime = Vector3.Distance(caster.transform.position, targetGO.transform.position) * 0.05f;
                projectile.transform.DOMove(targetGO.transform.position ,  travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * PrimaryDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                TemptargetGO = targetGO;
                GameObject splash = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash, 0.5f);
            }
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                travelTime = Vector3.Distance(TemptargetGO.transform.position, targetGO.transform.position) * 0.05f;
                projectile.transform.DOMove(targetGO.transform.position, travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * SecondaryDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                TemptargetGO = targetGO;
                GameObject splash = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash, 0.5f);
            }
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                travelTime = Vector3.Distance(TemptargetGO.transform.position, targetGO.transform.position) * 0.05f;
                projectile.transform.DOMove(targetGO.transform.position, travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * ThirdDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                TemptargetGO = targetGO;
                GameObject splash = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash, 0.5f);
            }
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                travelTime = Vector3.Distance(TemptargetGO.transform.position, targetGO.transform.position) * 0.05f;
                projectile.transform.DOMove(targetGO.transform.position, travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * FourthDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                TemptargetGO = targetGO;
                Destroy(projectile , 0.3f);
                GameObject splash = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash, 0.5f);
            }
        }
        tempHeroController.setIsAttacking(false);
    }
}
