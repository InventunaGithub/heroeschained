using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BloodDrain : Spell
{
    public override void Cast(GameObject caster , GameObject target)
    {
        if(GetTarget() == null)
        {
            throw new Exception("No target is selected");
        }
        if (GetCaster() == null)
        {
            throw new Exception("No caster is selected");
        }
        Hero casterHero = caster.GetComponent<Hero>();
        Hero targetHero = target.GetComponent<Hero>();

        GameObject tempSpell = Instantiate(Effects[2], caster.transform.position, caster.transform.rotation);
        Destroy(tempSpell , CastTime);
        StartCoroutine(CastSpellLag(casterHero, targetHero, caster, target));
    }
    IEnumerator CastSpellLag(Hero casterHero, Hero targetHero ,GameObject caster , GameObject target)
    {
        yield return new WaitForSeconds(CastTime);
        targetHero.Hurt((int)(Math.Round(casterHero.Damage * 1.3f)));
        casterHero.Health += (int)(Math.Round(casterHero.Damage * 0.5f));
        casterHero.Normalise();
        GameObject tempEffect = Instantiate(Effects[0], caster.transform.position, caster.transform.rotation);
        Destroy(tempEffect, 1);
        GameObject tempEffect2 = Instantiate(Effects[1], target.transform.position, target.transform.rotation);
        Destroy(tempEffect2, 1);
    }
}
