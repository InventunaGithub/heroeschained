using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 15 Sep 2020

public enum AITypes { Closest, Lockon };
public enum HeroTypes { Mage, Archer, Warrior, Human };


[RequireComponent(typeof(Equipment))]
[RequireComponent(typeof(Inventory))]
public class Hero : MonoBehaviour
{
    private Equipment equipment;
    private Inventory inventory;
    public List<int> Skills;
    public int UltimateSkill;
    public int UltimateSkillCardID;
    public HeroSO RootSO;
    public int HeroID;
    public string Name;
    public GameObject HeroObject;
    public int BaseHealth;
    public int BaseDamage;
    public int BaseDefence;
    public int Armor;
    public int Damage;
    public int Defence;
    public int Health;
    public int Range;
    public int MaxEnergy;
    public int MaxUltimateEnergy;
    public int Energy;
    public int UltimateEnergy;
    public AITypes AIType;
    public HeroTypes HeroType;
    public GameObject HeroSkin;

    public void Init(HeroSO rootSO)
    {
        HeroID = rootSO.HeroID;
        Name = rootSO.Name;
        BaseHealth = rootSO.BaseHealth;
        Range = rootSO.Range;
        BaseDamage = rootSO.BaseDamage;
        Health = BaseHealth;
        Damage = BaseDamage;
        Armor = 10;
        HeroSkin = rootSO.HeroSkin;
        this.AIType = rootSO.AIType;
        HeroType = rootSO.HeroType;
        Skills = rootSO.Skills;
        MaxEnergy = rootSO.MaxEnergy;
        MaxUltimateEnergy = rootSO.MaxUltimateEnergy;
        UltimateSkill = rootSO.UltimateSkill;
        UltimateSkillCardID = rootSO.UltimateSkillCardID;
        UltimateEnergy = 0;
        Energy = 0;
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();
    }

    public void Hurt(int damage) //
    {
        GainEnergy((int)(damage * 0.1));
        damage -= Defence;
        if(damage < 0)
        {
            damage = 0;
        }
        Health -= damage;
        Normalise();
    }
    public void GainEnergy(int amount)
    {
        Energy += amount; 
        UltimateEnergy += amount;
        Normalise();
    }
    public void Normalise()
    {
        if (Defence < 0)
        {
            Defence = 0;
        }
        if (Health < 0)
        {
            Health = 0;
        }
        if (Damage < 0)
        {
            Damage = 0;
        }
        if (Health > BaseHealth)
        {
            Health = BaseHealth;
        }
        if (Energy >= MaxEnergy)
        {
            Energy = MaxEnergy;
        }
        if (Energy < 0)
        {
            Energy = 0;
        }
        if (UltimateEnergy >= MaxUltimateEnergy)
        {
            UltimateEnergy = MaxUltimateEnergy;
        }
        if (UltimateEnergy < 0)
        {
            UltimateEnergy = 0;
        }
    }

    #region Equipment
    public bool WearItem(InventoryItem item)
    {

        return equipment.WearItems(item);
    }

    public bool WearItem(InventoryItem item, int additional)
    {

        return equipment.WearItems(item, additional);
    }

    public bool UnWearItem(InventoryItem item)
    {
        return equipment.UnWearItems(item);
    }

    public bool UnWearItem(InventoryItem item, int additional)
    {
        return equipment.UnWearItems(item, additional);
    }
    #endregion Equipment
}
