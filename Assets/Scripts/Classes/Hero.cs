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
    public HeroSO RootSO;
    public int HeroID;
    public string Name;
    public GameObject HeroObject;
    public int BaseHealth;
    public int BaseDamage;
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Vitality;
    public int Armor;
    public int Damage;
    public int Health;
    public int MaxHealth;
    public int Range;
    public AITypes AIType;
    public HeroTypes HeroType;
    public GameObject HeroSkin;

    private void Start()
    {
        Init(RootSO);
    }

    public void Init(HeroSO rootSO)
    {
        HeroID = rootSO.HeroID;
        Name = rootSO.Name;
        BaseHealth = rootSO.BaseHealth;
        Strength = rootSO.Strength;
        Dexterity = rootSO.Dexterity;
        Intelligence = rootSO.Intelligence;
        Vitality = rootSO.Vitality;
        Range = rootSO.Range;
        BaseDamage = rootSO.BaseDamage;
        Health = this.BaseHealth + (this.Vitality * 2);
        MaxHealth = Health;
        Damage = BaseDamage + (this.Strength * 2);
        Armor = 10;
        HeroSkin = rootSO.HeroSkin;
        this.AIType = rootSO.AIType;
        HeroType = rootSO.HeroType;
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();
    }

    public void Hurt(int damage) // this is used for physical attacks , attacks that must pierce armor first.
    {
        if (Armor >= damage)
        {
            Armor -= damage;
        }
        else
        {
            damage -= Armor;
            Armor = 0;
            Health -= damage;
        }
        Normalise();
    }

    void Normalise()
    {
        if (Armor < 0)
        {
            Armor = 0;
        }
        if (Health < 0)
        {
            Health = 0;
        }
        if (Dexterity < 0)
        {
            Dexterity = 0;
        }
        if (Damage < 0)
        {
            Damage = 0;
        }
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
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
