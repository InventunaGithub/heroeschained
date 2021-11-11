using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RainOfArrowsUltimateSkill : Spell
{
    //Shoots 3 arrows of light into the sky that multiply and hit all enemies 3 times for 100% damage while reduce their def by 10% per hit for (x)secs
    Animator casterAnimator;
    Animator targetAnimator;
    public float PrimaryDamageMultiplier;
    public float HealAmountMultiplier;
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
        //StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero, GameObject caster, GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        if (casterHero.Health > 0)
        {
            CasterAnimator.CrossFade("Attack", 0.1f);
            targetHero.Hurt((int)(Math.Round(casterHero.Damage * PrimaryDamageMultiplier)));
            casterHero.Health += (int)(Math.Round(casterHero.Damage * HealAmountMultiplier));
            casterHero.Normalise();
            Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
            GameObject tempEffect = Instantiate(Effects[0], caster.transform.position, spawnRotation);
            Destroy(tempEffect, 3);
            GameObject tempEffect2 = Instantiate(Effects[1], target.transform.position + Vector3.up, target.transform.rotation);
            Destroy(tempEffect2, 3);
        }
        tempHeroController.setIsAttacking(false);
    }
}
