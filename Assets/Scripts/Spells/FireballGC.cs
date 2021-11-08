using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballGC : Spell
{
    public float AOErange;
    public int Damage;
    public override void CastWithPosition(Vector3 positionToCast)
    {
        Collider[] inAOE = Physics.OverlapSphere(positionToCast, AOErange);
        foreach (Collider hero in inAOE)
        {
            hero.GetComponent<Renderer>().material.color = Color.red;
            if (hero.transform.tag == "Team2")
            {
                hero.GetComponent<Hero>().Hurt(Damage);
                Debug.Log("Got Hurt");
            }
        }
    }
}
