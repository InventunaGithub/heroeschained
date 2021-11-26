using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Stun : Spell
{
    public float PrimaryDamagePercent;
    public float StunTime;
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
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, caster.transform.rotation);
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
            StartCoroutine(StunTargetHero(targetHero));
            GameObject tempEffect = Instantiate(Effects[1], target.transform.position + Vector3.up, target.transform.rotation);
            Destroy(tempEffect, 1);
        }
    }

    IEnumerator StunTargetHero(Hero targetHero)
    {
        HeroController tempHeroController = targetHero.GetComponent<HeroController>();
        tempHeroController.HeroLock = true;
        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
        GameObject stunGameObject = Instantiate(Effects[2], targetHero.transform.position + Vector3.up, spawnRotation);
        stunGameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Destroy(stunGameObject, StunTime);
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(StunTime / 100);
            if(targetHero.Health == 0)
            {
                if (tempHeroController.HeroLock)
                {
                    tempHeroController.HeroLock = false;
                }
            }
        }
        if(tempHeroController.HeroLock)
        {
            tempHeroController.HeroLock = false;
        }
    }
}

