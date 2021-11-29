using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 11.2021
public class Adrenaline : Spell
{
    public float PrimarySpeedIncreasePercent;
    public float PrimaryReduceDefPercent;
    public float BuffTime;

    //Increase speed by 25% and reduce def by 50% for (x)secs
    public override void Cast(GameObject caster, GameObject target)
    {
        Hero casterHero = caster.GetComponent<Hero>();
        Hero targetHero = target.GetComponent<Hero>();
        caster.GetComponent<HeroController>().CastAnimation();
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero, GameObject caster, GameObject target)
    {
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        yield return new WaitForSeconds(CastTime);
        if (!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            float tempAttackSpeed = casterHero.AttackSpeed;
            casterHero.AttackSpeed += casterHero.AttackSpeed *(PrimarySpeedIncreasePercent/100f);
            float tempRunSpeed = tempHeroController.Agent.speed;
            tempHeroController.Agent.speed += tempHeroController.Agent.speed * (PrimarySpeedIncreasePercent/100f);
            tempHeroController.CalculateNormalAttackCooldown();
            casterHero.Normalise();
            int tempDefence = casterHero.Defence;
            casterHero.Defence -= (int)(casterHero.Defence * (PrimaryReduceDefPercent / 100f));
            GameObject tempEffect = Instantiate(Effects[1], caster.transform.position + Vector3.up , Quaternion.identity);
            Destroy(tempEffect, 3);
            yield return new WaitForSeconds(BuffTime);
            casterHero.AttackSpeed = tempAttackSpeed;
            tempHeroController.Agent.speed = tempRunSpeed;
            tempHeroController.CalculateNormalAttackCooldown();
            casterHero.Defence = tempDefence;

        }
    }
}
