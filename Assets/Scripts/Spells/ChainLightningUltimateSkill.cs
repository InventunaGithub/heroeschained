using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChainLightningUltimateSkill : Spell
{
    //Hit the closet enemy for 160% damage and then the next closet enemy for 120% damage then 80%, 40%
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
        CasterAnimator.CrossFade("Cast", 0.1f);
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero, GameObject caster, GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        if (!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            GameObject targetGO = null;
            CasterAnimator.CrossFade("Attack", 0.1f);
            GameObject projectile = Instantiate(Effects[1], caster.transform.position + Vector3.up, Quaternion.identity);
            projectile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            List<Hero> tempList = new List<Hero>(tempHeroController.EnemyTeam);
            float travelTime = 0.3f;
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                projectile.transform.DOMove(targetGO.transform.position + Vector3.up ,  travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * PrimaryDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                GameObject splash1 = Instantiate(Effects[2], targetGO.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash1, 0.5f);
            }
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                projectile.transform.DOMove(targetGO.transform.position + Vector3.up, travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * SecondaryDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                GameObject splash2 = Instantiate(Effects[2], targetGO.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash2, 0.5f);
            }
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                projectile.transform.DOMove(targetGO.transform.position + Vector3.up, travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * ThirdDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                GameObject splash3 = Instantiate(Effects[2], targetGO.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash3, 0.5f);
            }
            if (tempList.Count > 0)
            {
                targetGO = tempHeroController.ClosestEnemy(tempList);
                projectile.transform.DOMove(targetGO.transform.position + Vector3.up, travelTime);
                yield return new WaitForSeconds(travelTime);
                targetGO.GetComponent<Hero>().Hurt((int)(casterHero.Damage * FourthDamageMultiplier));
                tempList.Remove(tempHeroController.ClosestEnemy(tempList).GetComponent<Hero>());
                GameObject splash4 = Instantiate(Effects[2], targetGO.transform.position + Vector3.up, Quaternion.identity);
                Destroy(splash4, 0.5f);
            }
            Destroy(projectile, 0.1f);
        }
        tempHeroController.setIsAttacking(false);
    }
}
