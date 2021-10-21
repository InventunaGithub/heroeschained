using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 15 Sep 2020

//Author: Deniz Afşar
//Date: 21 Sep 2021
public class Hero : MonoBehaviour
{
    private Equipment equipment;
    private Inventory inventory;

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

    public Hero(string name, int heroID, int baseHealth, int baseDamage, int strength, int dexterity, int intelligence, int vitality, int range, List<Card> skills = null, AITypes AIType = AITypes.Closest)
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
        this.AIType = AIType;
        inventory = new Inventory();
        equipment = new Equipment();
    }

    #region Inventory
    public int GetInventoryItemCount()
    {
        return inventory.ItemCount();
    }

    public InventoryItem GetInventoryItem(int index)
    {
        return inventory.GetItem(index);
    }

    public bool AddItemToInventory(InventoryItem item)
    {
        return inventory.Add(item);
    }

    public bool RemoveFromInventory(InventoryItem item)
    {
        return inventory.RemoveItem(item);
    }

    public bool RemoveFromInventoryAt(int index)
    {
        return inventory.RemoveItemAt(index);
    }
    #endregion Inventory




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
}