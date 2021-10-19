using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 15 Sep 2020

[CreateAssetMenu(fileName = "New Hero", menuName = "Inventuna/HeroesChained/Hero")]
public class HeroSO : ScriptableObject
{
    public int HeroID; //Uniq id ?
    public string Name;
    public List<Card> Skills;
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
    public AITypes AIType;
    public HeroTypes HeroType;
    
}
