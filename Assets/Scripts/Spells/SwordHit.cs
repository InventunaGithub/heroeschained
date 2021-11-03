using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : Spell
{
    Animator casterAnimator; 
    Animator targetAnimator;
    public Vector3 offset = new Vector3(0, 1, 0);
    public override void Cast(GameObject caster, GameObject target)
    {
        HeroController tempHeroController = caster.GetComponent<HeroController>();
        casterAnimator = caster.transform.GetChild(0).GetComponent<Animator>();
        casterAnimator.CrossFade("SSAttack", 0.1f);
        Hero casterHero = caster.GetComponent<Hero>();
        Hero targetHero = target.GetComponent<Hero>();
        tempHeroController.setIsAttacking(true);
        GameObject splashGO = Instantiate(Effects[0], target.transform.position + offset , target.transform.rotation);
        targetHero.Hurt(casterHero.Damage);
        Destroy(splashGO, 0.3f);
        tempHeroController.setIsAttacking(false);
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return casterAnimator.GetCurrentAnimatorStateInfo(0).length > casterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime && casterAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
