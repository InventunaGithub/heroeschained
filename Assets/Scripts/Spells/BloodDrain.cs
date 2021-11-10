using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class BloodDrain : Spell
{
    Animator casterAnimator;
    Animator targetAnimator;
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
        CasterAnimator = caster.transform.GetComponentInChildren<Animator>();
        CasterAnimator.CrossFade("Idle", 0.1f);
        GameObject castingEffect = Instantiate(Effects[2], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero ,GameObject caster , GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        CasterAnimator.CrossFade("Attack", 0.1f);
        targetHero.Hurt((int)(Math.Round(casterHero.Damage * 1.3f)));
        casterHero.Health += (int)(Math.Round(casterHero.Damage * 0.5f));
        casterHero.Normalise();
        Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
        GameObject tempEffect = Instantiate(Effects[0], caster.transform.position, spawnRotation);
        Destroy(tempEffect, 3);
        GameObject tempEffect2 = Instantiate(Effects[1], target.transform.position + Vector3.up, target.transform.rotation);
        Destroy(tempEffect2, 3);
    }
}
