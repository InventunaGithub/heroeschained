using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

//Author: Mert Karavural
//Date: 4.11.2021
public class PowerStrike :  Spell
{
    public float PrimaryDamagePercent;
    public override void Cast(GameObject caster, GameObject target)
    {
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
        targetHero.GetComponent<HeroController>().CastAnimation();
        GameObject castingEffect = Instantiate(Effects[1], caster.transform.position + Vector3.up, caster.transform.rotation);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero, GameObject caster, GameObject target)
    {
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        yield return new WaitForSeconds(CastTime);
        if (!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            tempHeroController.AttackAnimation();
            targetHero.Hurt((int)(Math.Round(casterHero.Damage * (PrimaryDamagePercent / 100f))));
            targetHero.Normalise();
            GameObject tempEffect = Instantiate(Effects[0], target.transform.position + Vector3.up, target.transform.rotation);
            Destroy(tempEffect, 1);
        }
    }
}
