using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 15 Sep 2020

public enum HeroTypes { Mage, Archer, Warrior, Human };


[RequireComponent(typeof(Equipment))]
[RequireComponent(typeof(Inventory))]
public class Hero : MonoBehaviour
{
    private Equipment equipment;

    public int HeroID; //Uniq id ?
    public string Name;
    //public Item[] Items;
    public List<Card> Skills;
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
        Hero2("Deniz");
    }

    public void Hero2(string name, int heroID = 0, int baseHealth = 0,
        int baseDamage = 0, int strength = 0, int dexterity = 0,
        int intelligence = 0, int vitality = 0, int range = 0,
        GameObject heroSkin = null, List<Card> skills = null,
        AITypes AIType = AITypes.Closest, HeroTypes heroType = HeroTypes.Human)
    {
        HeroID = heroID;
        Name = name;
        BaseHealth = baseHealth;
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Vitality = vitality;
        Range = range;
        BaseDamage = baseDamage;
        Health = this.BaseHealth + (this.Vitality * 2);
        MaxHealth = Health;
        Damage = BaseDamage + (this.Strength * 2);
        Armor = 10;
        Skills = skills;
        HeroSkin = heroSkin;
        this.AIType = AIType;
        HeroType = heroType;
        equipment = GetComponent<Equipment>();
    }

    public Card UsedSkill(Card usedCard)
    {
        Card TempCard = new Card(usedCard.CardID, usedCard.Name, usedCard.Info, usedCard.Power, usedCard.Range);

        TempCard.Power += Damage;

        return TempCard;
    }

    public void EffectedBy(Card card) //This is used for most situations. Normal attacks and such still counts as cards.
    {
        Hurt(card.Power);

        Normalise();
    }

    void Hurt(int damage) // this is used for physical attacks , attacks that must pierce armor first.
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