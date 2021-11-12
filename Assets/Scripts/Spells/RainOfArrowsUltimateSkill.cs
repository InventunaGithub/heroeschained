using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RainOfArrowsUltimateSkill : Spell
{
    //Shoots 3 arrows of light into the sky that multiply and hit all enemies 3 times for 100% damage while reduce their def by 10% per hit for (10)secs
    public float PrimaryDamageMultiplier;
    public float ReduceDefMultiplier;
    public int HitEachEnemyWithAmount;
    public int ReduceDefSeconds;
    HeroController tempHeroController;
    List<GameObject> arrows = new List<GameObject>();
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
            CasterAnimator.CrossFade("Attack", 0.1f);
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
                        arrows[arrows.Count - 1].transform.DOMove(hero.transform.position, 0.3f);
                        yield return new WaitForSeconds(0.3f);
                        Destroy(arrows[arrows.Count - 1]);
                        arrows.RemoveAt(arrows.Count - 1);
                        GameObject splash = Instantiate(Effects[1], hero.transform.position + Vector3.up, Quaternion.identity);
                        Destroy(splash, 0.3f);
                        hero.Hurt(casterHero.Damage);
                        lowerDef(hero, ReduceDefSeconds);
                    }
                    else
                    { 
                        arrows.RemoveAt(arrows.Count - 1);
                    }
                   
                }
            }
        }
        tempHeroController.setIsAttacking(false);
    }

    IEnumerator lowerDef(Hero hero , int time)
    {
        int tempDefence = hero.Defence;
        hero.Defence -= (int)(hero.BaseDefence * ReduceDefMultiplier);
        if(hero.Defence < 0 )
        {
            hero.Defence = 0;
        }
        yield return new WaitForSeconds(time);
        hero.Defence = tempDefence;
    }
}
