using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class RainOfArrows : Spell
{
    public float AOErange;
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
        GameObject castingEffect = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero, GameObject caster, GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        Collider[] inAOE = Physics.OverlapSphere(targetHero.HeroObject.transform.position, AOErange);
        foreach (Collider hero in inAOE)
        {
            if(hero.transform.tag == targetHero.HeroObject.transform.tag)
            {
                if(hero != targetHero)
                {
                    hero.GetComponent<Hero>().Hurt((int)(casterHero.Damage * 0.6f));
                }

            }
        }
        targetHero.Hurt((int)(Math.Round(casterHero.Damage * 1.3f)));
        casterHero.Health += (int)(Math.Round(casterHero.Damage * 0.5f));
        casterHero.Normalise();
        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
        GameObject AOERing = Instantiate(Effects[3], target.transform.position, spawnRotation);
        AOERing.transform.DOScale(AOErange, 0.1f);
        Debug.Log(casterHero.name + " Used RainOfArrows");
        for (int i = 0; i <= 10; i++)
        {
            Vector3 rainStartPos = new Vector3(target.transform.position.x + UnityEngine.Random.Range(-AOErange, AOErange), 5, target.transform.position.z + UnityEngine.Random.Range(-AOErange, AOErange));
            Vector3 rainEndPos = new Vector3(rainStartPos.x, 0, rainStartPos.z);
            yield return new WaitForSeconds(0.05f);
            GameObject tempEffect2 = Instantiate(Effects[0], rainStartPos, target.transform.rotation);
            tempEffect2.transform.DOMove(rainEndPos, 0.3f);
            Destroy(tempEffect2, 0.2f);
            StartCoroutine(Splash(0.2f , rainEndPos , target.transform.rotation));
            if(i == 10)
            {
                Destroy(AOERing , 0.3f);
            }
        }

    }

    IEnumerator Splash(float seconds , Vector3 rainEndPos , Quaternion targetRot)
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 offset = new Vector3(0, 0.5f, 0);
        GameObject splash = Instantiate(Effects[1], rainEndPos + offset, targetRot);
        Destroy(splash, 0.2f);
    }

}
