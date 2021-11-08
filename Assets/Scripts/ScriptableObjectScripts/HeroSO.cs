using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 15 Sep 2020

[CreateAssetMenu(fileName = "New Hero", menuName = "Inventuna/Heroes Chained/Hero")]
public class HeroSO : ScriptableObject
{
    public int HeroID; 
    public string Name;
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
    public int MaxEnergy;
    public List<int> Skills;
    public AITypes AIType;
    public HeroTypes HeroType;
    public GameObject HeroSkin;
    
}
