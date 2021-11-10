using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 5.11.2021
public class SpellManager : MonoBehaviour
{
    public List<GameObject> Spells;
    
    public Spell FindSpell(int spellID)
    {
        foreach (GameObject spell in Spells)
        {
            if (spell.GetComponent<Spell>().ID == spellID)
            {
                return spell.GetComponent<Spell>();
            }
        }
        return null;
    }
    public void Cast(int spellID , GameObject caster , GameObject target)
    {
        foreach (GameObject spell in Spells)
        {
            if(spell.GetComponent<Spell>().ID == spellID)
            {
                spell.GetComponent<Spell>().Cast(caster, target);
            }
        }
    }
    public void CastWithPosition(int spellID, Vector3 position)
    {
        foreach (GameObject spell in Spells)
        {
            if (spell.GetComponent<Spell>().ID == spellID)
            {
                spell.GetComponent<Spell>().CastWithPosition(position);
            }
        }
    }
}
