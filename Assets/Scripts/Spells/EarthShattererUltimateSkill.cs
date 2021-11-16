using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EarthShattererUltimateSkill : Spell
{
    //With all his might they slam their weapon on the ground making it crumble, does 150% damage to all enemies in a cone and reducing their attack speed by 20%
    public float PrimaryDamageMultiplier;
    public float HealAmountMultiplier;
    public float DistanceOfCone;
    public float RadiusOfCone;
    public float AngleOfCone;
    HeroController tempHeroController;

    public override void CastWithDirection(GameObject caster, Vector3 direction)
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
        StartCoroutine(CastSpellLag(casterHero, caster , direction));
    }
    IEnumerator CastSpellLag(Hero casterHero, GameObject caster, Vector3 direction)
    {
        yield return new WaitForSeconds(CastTime);
        if (!tempHeroController.IsDead && !tempHeroController.Victory)
        {
            //RaycastHit[] sphereCastHits = Physics.SphereCastAll(caster.transform.position - new Vector3(0, 0, RadiusOfCone), RadiusOfCone, direction, DistanceOfCone);
            //List<RaycastHit> coneCastHitList = new List<RaycastHit>();

            //if (sphereCastHits.Length > 0)
            //{
            //    for (int i = 0; i < sphereCastHits.Length; i++)
            //    {
            //        Vector3 hitPoint = sphereCastHits[i].point;
            //        Vector3 directionToHit = hitPoint - caster.transform.position;
            //        float angleToHit = Vector3.Angle(direction, directionToHit);
            //        if (angleToHit < AngleOfCone)
            //        {
            //            Debug.DrawRay(caster.transform.position + Vector3.up, directionToHit, Color.red, 1);
            //            coneCastHitList.Add(sphereCastHits[i]);
            //        }
            //        else
            //        {
            //            Debug.DrawRay(caster.transform.position + Vector3.up, directionToHit, Color.white, 1);
            //        }
            //    }
            //}

            //Debug.Break();
            //RaycastHit[] coneCastHits = new RaycastHit[coneCastHitList.Count];
            //coneCastHits = coneCastHitList.ToArray();

            //foreach (var inCone in coneCastHits)
            //{
            //    if (inCone.transform.tag == "Team2")
            //    {
            //        Hero tempHero = inCone.transform.GetComponent<Hero>();
            //        GameObject splash = Instantiate(Effects[2], tempHero.transform.position + Vector3.up, Quaternion.identity);
            //        Destroy(splash, 0.3f);
            //        tempHero.Hurt((int)(casterHero.Damage * PrimaryDamageMultiplier));
            //        //TODO Reducing attack damage 
            //    }
            //}

            CasterAnimator.CrossFade("Attack", 0.1f);
        }
        tempHeroController.setIsAttacking(false);
    }

    
}
