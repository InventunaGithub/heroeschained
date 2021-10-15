using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO MAKE THE EFFECTEDBYCLASS , MAKE HERO SKILLS.

[CreateAssetMenu(fileName = "New Hero", menuName = "Inventuna/HeroesChained/Hero")]
public class HeroSO : ScriptableObject
{
    public int HeroID; //Uniq id ?
    public string Name;
    //public Item[] Items;
    public List<Card> HeroSkills;
    public List<Card> StatusEffects; // this should be skills or card ?
    public int BaseHealth;
    public int BaseDamage = 5;
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Vitality;
    public int Armor;
    public int Damage;
    public int Health;
    public int Range;
    
}
