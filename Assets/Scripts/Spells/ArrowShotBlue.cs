using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 5.11.2021
public class ArrowShotBlue : Spell
{
    Animator casterAnimator;
    Animator targetAnimator;
    HeroController tempHeroController;
    GameObject projectileGO;
    public Vector3 offset = new Vector3(0, 1, 0);
    private float travelTime;

    public override void Cast(GameObject caster, GameObject target)
    {
        travelTime = Vector3.Distance(caster.transform.position, target.transform.position) * 0.05f;
        SetCaster(caster);
        SetTarget(target);
        tempHeroController = caster.GetComponent<HeroController>();
        casterAnimator = caster.transform.GetChild(0).GetComponent<Animator>();
        casterAnimator.CrossFade("Attack", 0.1f);
        tempHeroController.setIsAttacking(true);
        projectileGO = Instantiate(Effects[0], caster.transform.position + offset, caster.transform.rotation);
        projectileGO.transform.DOMove(target.transform.position + offset, travelTime);
        
        StartCoroutine(TravelTime());
    }
    IEnumerator TravelTime()
    {
        Hero casterHero = GetCaster().GetComponent<Hero>();
        Hero targetHero = GetTarget().GetComponent<Hero>();
        yield return new WaitForSeconds(travelTime);
        Destroy(projectileGO);
        GameObject splashGO = Instantiate(Effects[1], GetTarget().transform.position + offset, GetTarget().transform.rotation);
        targetHero.Hurt(casterHero.Damage);
        casterHero.GainEnergy(casterHero.Intelligence + 5);
        Destroy(splashGO, 0.3f);
        tempHeroController.setIsAttacking(false);
    }
    bool AnimatorIsPlaying(string stateName)
    {
        return casterAnimator.GetCurrentAnimatorStateInfo(0).length > casterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime && casterAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
