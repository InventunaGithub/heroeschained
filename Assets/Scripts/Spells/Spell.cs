using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public int ID;
    GameObject caster;
    GameObject target;
    public List<GameObject> Effects;
    public float CastTime;
    public float AOERange;
    public int EnergyCost;
    public virtual void Cast(GameObject caster, GameObject target)
    {

    }

    public virtual void CastWithPosition(Vector3 position)
    {

    }
    public virtual void SetCaster(GameObject caster)
    {
        this.caster = caster;
    }
    public virtual void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public virtual GameObject GetCaster()
    {
        return caster;
    }
    public virtual GameObject GetTarget()
    {
        return target;
    }

}
