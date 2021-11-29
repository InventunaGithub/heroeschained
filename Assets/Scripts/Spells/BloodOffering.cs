using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 11.2021
public class BloodOffering : Spell
{
    public float PrimaryLoseHPPercent;
    public float PrimarySpeedBuff;
    public float BuffTime;

    //Increase speed of all allies by 20% for (x) secs, reduces self HP by 20% overtime.

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
            foreach (Hero hero in tempHeroController.Team)
            {
                if(hero != casterHero)
                {
                    StartCoroutine(SpeedBuff(hero));
                }
            }
            GameObject bleedingEffect = Instantiate(Effects[1], caster.transform.position + Vector3.up, Quaternion.identity , caster.transform);
            Destroy(bleedingEffect, BuffTime);
            for (int i = 0; i < 10; i++)    
            {
                yield return new WaitForSeconds(BuffTime / 10);
                casterHero.Hurt((int)(casterHero.Health * (PrimaryLoseHPPercent/100f)) / 10);
            }

        }
    }
    IEnumerator SpeedBuff(Hero hero)
    {
        HeroController heroController = hero.GetComponent<HeroController>();
        float tempAttackSpeed = hero.AttackSpeed;
        hero.AttackSpeed += hero.AttackSpeed * (PrimarySpeedBuff / 100f);
        float tempRunSpeed = heroController.Agent.speed;
        heroController.Agent.speed += heroController.Agent.speed * (PrimarySpeedBuff / 100f);
        heroController.CalculateNormalAttackCooldown();
        GameObject buffEffect = Instantiate(Effects[2], hero.transform.position + Vector3.up, Quaternion.identity, hero.transform);
        Destroy(buffEffect, BuffTime);

        yield return new WaitForSeconds(BuffTime);

        hero.AttackSpeed = tempAttackSpeed;
        heroController.Agent.speed = tempRunSpeed;
        heroController.CalculateNormalAttackCooldown();
    }
}
