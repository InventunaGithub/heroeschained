using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

//Author: Mert Karavural
//Date: 11.2021
public class EarthShattererUltimateSkill : Spell
{
    //With all his might they slam their weapon on the ground making it crumble, does 150% damage to all enemies in a cone and reducing their attack speed by 20%
    public float PrimaryDamagePercent;
    public float ReduceAttackSpeedTime;
    public float ReduceAttackSpeedPercent;
    [Range(0, 100)]
    HeroController tempHeroController;

    public override void CastWithDirection(GameObject caster, GameObject skillMesh)
    {
        tempHeroController = caster.GetComponent<HeroController>();
        tempHeroController.SetIsAttacking(true);
        SetCaster(caster);
        if (GetCaster() == null)
        {
            throw new Exception("No caster is selected");
        }
        Hero casterHero = caster.GetComponent<Hero>();
        tempHeroController.CastAnimation();
        GameObject castingEffect = Instantiate(Effects[0], caster.transform.position + Vector3.up, Quaternion.identity);
        Destroy(castingEffect, CastTime);
        tempHeroController.HeroLock = true;
        StartCoroutine(LookAtTarget(caster, skillMesh));
        StartCoroutine(CastSpellLag(casterHero, caster , skillMesh));
    }
    IEnumerator LookAtTarget(GameObject caster , GameObject skillMesh)
    {
        yield return new WaitForEndOfFrame();
        tempHeroController.Agent.enabled = false;
        caster.transform.DOLookAt(skillMesh.transform.position + Vector3.forward, 0.1f);
    }
    IEnumerator CastSpellLag(Hero casterHero, GameObject caster, GameObject skillMesh)
    {
        yield return new WaitForSeconds(CastTime);
        tempHeroController = casterHero.GetComponent<HeroController>();
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
                    tempHero.Hurt((int)(casterHero.Damage * (PrimaryDamagePercent / 100f)));
                    StartCoroutine(MakeTheGameObjectJump(inCone));
                    StartCoroutine(SlowAttackSpeed(inCone , ReduceAttackSpeedTime));
                }
            }

            tempHeroController.AttackAnimation();
        }
        tempHeroController.SetIsAttacking(false);
        tempHeroController.Agent.enabled = true;
        yield return new WaitForEndOfFrame();
        tempHeroController.HeroLock = false;
    }
    
    IEnumerator SlowAttackSpeed(GameObject targetGO , float duration)
    {
        HeroController targetHC = targetGO.GetComponent<HeroController>();
        Hero targetHero = targetGO.GetComponent<Hero>();
        targetHero.AttackSpeed -= targetHero.AttackSpeed / (ReduceAttackSpeedPercent / 100f);
        targetHero.Normalise();
        targetHC.CalculateNormalAttackCooldown();
        yield return new WaitForSeconds(duration);
        targetHero.AttackSpeed += targetHero.AttackSpeed / (ReduceAttackSpeedPercent / 100f);
        targetHero.Normalise();
        targetHC.CalculateNormalAttackCooldown();
    }
    IEnumerator MakeTheGameObjectJump(GameObject targetGO)
    {
        HeroController inConeHC = targetGO.transform.GetComponent<HeroController>();
        targetGO.transform.GetComponent<Rigidbody>().useGravity = false;
        inConeHC.HeroLock = true;
        yield return new WaitForEndOfFrame();
        inConeHC.Agent.enabled = false;
        Vector3 origin = targetGO.transform.position;
        yield return new WaitForSeconds(0.1f);
        targetGO.transform.DOMoveY(3, 0.3f);
        yield return new WaitForSeconds(0.4f);
        targetGO.transform.DOMove(origin, 0.3f);
        yield return new WaitForSeconds(0.35f);
        inConeHC.Agent.enabled = true;
        yield return new WaitForEndOfFrame();
        inConeHC.HeroLock = false;
        targetGO.transform.GetComponent<Rigidbody>().useGravity = true;
    }



}
