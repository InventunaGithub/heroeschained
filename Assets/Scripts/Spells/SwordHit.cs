using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 5.11.2021
public class SwordHit : Spell
{
    Animator casterAnimator; 
    Animator targetAnimator;
    public Vector3 offset = new Vector3(0, 1, 0);
    public override void Cast(GameObject caster, GameObject target)
    {
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        casterAnimator = caster.transform.GetChild(0).GetComponent<Animator>();
        casterAnimator.CrossFade("Attack", 0.1f);
        Hero casterHero = caster.GetComponent<Hero>();
        Hero targetHero = target.GetComponent<Hero>();
        tempHeroController.setIsAttacking(true);
        GameObject splashGO = Instantiate(Effects[0], target.transform.position + offset , target.transform.rotation);
        targetHero.Hurt(casterHero.Damage);
        casterHero.GainEnergy(5);
        Destroy(splashGO, 0.3f);
        tempHeroController.setIsAttacking(false);
    }

}
