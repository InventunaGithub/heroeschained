using System;
using System.Collections;
using UnityEngine;

//Author: Mert Karavural
//Date: 4.11.2021

public class BloodDrain : Spell
{
    public float PrimaryDamagePercent;
    public float HealAmountPercent;

    //Attack closet enemy for 130% damage and self heal for 50% of damage
    public override void Cast(GameObject caster , GameObject target)
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
        caster.GetComponent<HeroController>().CastAnimation();
        GameObject castingEffect = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero ,GameObject caster , GameObject target)
    {
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        yield return new WaitForSeconds(CastTime);
        if(!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            tempHeroController.AttackAnimation();
            targetHero.Hurt((int)(Math.Round(casterHero.Damage * (PrimaryDamagePercent / 100f))));
            casterHero.Health += (int)(Math.Round((casterHero.Damage * (PrimaryDamagePercent / 100f)) * (HealAmountPercent / 100f)));
            casterHero.Normalise();
            Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
            GameObject tempEffect = Instantiate(Effects[0], caster.transform.position, spawnRotation);
            Destroy(tempEffect, 3);
            GameObject tempEffect2 = Instantiate(Effects[1], target.transform.position + Vector3.up, target.transform.rotation);
            Destroy(tempEffect2, 3);
        }
    }
}
