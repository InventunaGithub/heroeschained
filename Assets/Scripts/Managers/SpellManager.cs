using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public List<GameObject> Spells;
    
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
}
