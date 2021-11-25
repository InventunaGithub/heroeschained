using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 11.2021
public class RainOfArrowsUltimateSkill : Spell
{
    //Shoots 3 arrows of light into the sky that multiply and hit all enemies 3 times for 100% damage while reduce their def by 10% per hit for (10)secs
    public float PrimaryDamagePercent;
    public float ReduceDefPercent;
    public int HitEachEnemyWithAmount;
    public int ReduceDefSeconds;
    HeroController tempHeroController;
    List<GameObject> arrows = new List<GameObject>();
    public override void Cast(GameObject caster, GameObject target)
    {
        tempHeroController = caster.GetComponent<HeroController>();
        tempHeroController.SetIsAttacking(true);
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
        tempHeroController.CastAnimation();
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero, GameObject caster, GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        if (!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            tempHeroController.AttackAnimation();
            for (int i = 0; i < tempHeroController.EnemyTeam.Count * HitEachEnemyWithAmount; i++)
            {
                GameObject arrow = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
                arrow.transform.DOMove(caster.transform.position + new Vector3(0, 4, i % HitEachEnemyWithAmount), 0.5f);
                arrows.Add(arrow);
            }
            yield return new WaitForSeconds(0.55f);
            List<Hero> tempHeroList = new List<Hero>(tempHeroController.EnemyTeam);
            for (int i = 0; i < HitEachEnemyWithAmount; i++)
            {
                foreach (Hero hero in tempHeroList)
                {
                    if(hero.Health > 0)
                    {
                        arrows[arrows.Count - 1].transform.DOMove(hero.transform.position, 0.2f);
                        yield return new WaitForSeconds(0.4f);
                        Destroy(arrows[arrows.Count - 1] , 0.5f);
                        arrows.RemoveAt(arrows.Count - 1);
                        Vector3 splashTempPosition = hero.transform.position;
                        GameObject splash = Instantiate(Effects[1], hero.transform.position + Vector3.up, Quaternion.identity);
                        Destroy(splash, 0.3f);
                        hero.Hurt((int)(casterHero.Damage * (PrimaryDamagePercent / 100f)));
                        lowerDef(hero, ReduceDefSeconds);
                        
                    }
                    else
                    { 
                        arrows.RemoveAt(arrows.Count - 1);
                    }
                   
                }
            }
        }
        tempHeroController.SetIsAttacking(false);
    }

    IEnumerator lowerDef(Hero hero , int time)
    {
        int tempDefence = hero.Defence;
        hero.Defence -= (int)(hero.BaseDefence * (ReduceDefPercent / 100f));
        if(hero.Defence < 0 )
        {
            hero.Defence = 0;
        }
        yield return new WaitForSeconds(time);
        hero.Defence = tempDefence;
    }
}
