using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 5.11.2021
public class SwordHit : Spell
{
    public int GainEnergyAmount;
    public Vector3 offset = new Vector3(0, 1, 0);
    public override void Cast(GameObject caster, GameObject target)
    {
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        if(!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            tempHeroController.AttackAnimation();
            Hero casterHero = caster.GetComponent<Hero>();
            Hero targetHero = target.GetComponent<Hero>();
            tempHeroController.SetIsAttacking(true);
            GameObject splashGO = Instantiate(Effects[0], target.transform.position + offset, target.transform.rotation);
            targetHero.Hurt(casterHero.Damage);
            casterHero.GainEnergy(GainEnergyAmount);
            Destroy(splashGO, 0.3f);
            tempHeroController.SetIsAttacking(false);
        }
    }


}
