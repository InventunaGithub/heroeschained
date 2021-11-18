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
    public int BaseDefence;
    public int BaseDamage;
    public int Defence;
    public int Damage;
    public int Health;
    public int Range;
    public int MaxEnergy;
    public int MaxUltimateEnergy;
    public List<int> Skills;
    public int UltimateSkill;
    public int UltimateSkillCardID;
    public AITypes AIType;
    public HeroTypes HeroType;
    public GameObject HeroSkin;
    
}
