using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EarthShattererUltimateSkill : Spell
{
    //With all his might they slam their weapon on the ground making it crumble, does 150% damage to all enemies in a cone and reducing their attack speed by 20%
    public float PrimaryDamageMultiplier;
    public float ReduceAttackSpeedTime;
    public float ReduceAttackSpeedPercent;
    [Range(0, 100)]
    HeroController tempHeroController;

    public override void CastWithDirection(GameObject caster, GameObject skillMesh)
    {
        tempHeroController = caster.GetComponent<HeroController>();
        tempHeroController.setIsAttacking(true);
        SetCaster(caster);
        if (GetCaster() == null)
        {
            throw new Exception("No caster is selected");
        }
        Hero casterHero = caster.GetComponent<Hero>();
        CasterAnimator = caster.transform.GetComponentInChildren<Animator>();
        CasterAnimator.CrossFade("Cast", 0.1f);
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        tempHeroController.HeroLock = true;
        tempHeroController.Agent.enabled = false;
        caster.transform.DOLookAt(skillMesh.transform.position + Vector3.forward, 0.1f);
        StartCoroutine(CastSpellLag(casterHero, caster , skillMesh));
    }
    IEnumerator CastSpellLag(Hero casterHero, GameObject caster, GameObject skillMesh)
    {
        yield return new WaitForSeconds(CastTime);
        if (!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            List<GameObject> inArea = skillMesh.GetComponentInChildren<CollisionObserver>().CollidedObjects;
            
            foreach (GameObject inCone in inArea)
            {
                if (inCone.transform.tag == "Team2")
                {
                    Hero tempHero = inCone.transform.GetComponent<Hero>();
                   
                    GameObject splash = Instantiate(Effects[1], tempHero.transform.position + Vector3.up, Quaternion.identity);
                    Destroy(splash, 0.3f);
                    tempHero.Hurt((int)(casterHero.Damage * PrimaryDamageMultiplier));
                    StartCoroutine(MakeTheGameObjectJump(inCone));
                    StartCoroutine(SlowAttackSpeed(inCone , ReduceAttackSpeedTime));
                }
            }

            CasterAnimator.CrossFade("Attack", 0.1f);
        }
        tempHeroController.setIsAttacking(false);
        tempHeroController.Agent.enabled = true;
        tempHeroController.HeroLock = false;
    }
    
    IEnumerator SlowAttackSpeed(GameObject targetGO , float duration)
    {
        HeroController targetHC = targetGO.GetComponent<HeroController>();
        Hero targetHero = targetGO.GetComponent<Hero>();
        targetHero.AttackSpeed -= targetHero.AttackSpeed / (ReduceAttackSpeedPercent / 100);
        targetHero.Normalise();
        targetHC.CalculateNormalAttackCooldown();
        yield return new WaitForSeconds(duration);
        targetHero.AttackSpeed += targetHero.AttackSpeed / (ReduceAttackSpeedPercent / 100);
        targetHero.Normalise();
        targetHC.CalculateNormalAttackCooldown();
    }
    IEnumerator MakeTheGameObjectJump(GameObject targetGO)
    {
        HeroController inConeHC = targetGO.transform.GetComponent<HeroController>();
        targetGO.transform.GetComponent<Rigidbody>().useGravity = false;
        inConeHC.HeroLock = true;
        inConeHC.Agent.enabled = false;
        Vector3 origin = targetGO.transform.position;
        yield return new WaitForSeconds(0.1f);
        targetGO.transform.DOMoveY(3, 0.3f);
        yield return new WaitForSeconds(0.4f);
        targetGO.transform.DOMove(origin, 0.3f);
        yield return new WaitForSeconds(0.3f);
        inConeHC.Agent.enabled = true;
        inConeHC.HeroLock = false;
        targetGO.transform.GetComponent<Rigidbody>().useGravity = true;
    }



}
