using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 15 Sep 2020


[System.Serializable]
public class Player
{
    public int PlayerID;
    public string Name;
    public List<Hero> Team;

}

public enum AITypes {Closest , Lockon};
public enum ItemTypes
{
    Weapon, WeaponTwoHanded, Shield,
    Armor, Helm, Belt, Boot, Ring, Amulet, Gem,
    Gold, Scroll, Potion, Misc
}
public enum ItemQualities
{
    Common, Uncommon, Rare, Unique
}


[System.Serializable]
public class Card
{
    //Base class for cards FOR NOW THIS IS USED BOTH CARDS AND SPELLS , BECAUSE OF THAT WE EVEN DO NORMAL ATTACKS BY "NORMAL ATTACK" CARD. it is for simpilicty.
    public int CardID;
    public string Name;
    public string Info;
    public int Power;
    public int Range;
    public bool Used;

    public Card(int cardID, string name, string info, int power, int range)
    {
        CardID = cardID;
        Name = name;
        Info = info;
        Power = power;
        Range = range;
    }
}

[System.Serializable]
public class Hero
{
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


    public Hero(string name, int heroID, int baseHealth, int baseDamage, int strength, int dexterity, int intelligence, int vitality, int range, List<Card> skills = null , AITypes AIType = AITypes.Closest)
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
        if(Armor < 0)
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

//Author: Deniz Afşar
//Date: 20.10.2021

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
    [SerializeField] private int capacity = 10;

    void Add(InventoryItem item)
    {
        if (!(items.Count >= capacity))
        {
            items.Add(item);
        }
        else
        {
            Debug.Log("Achieve the capacity");
        }
    }

    void RemoveItem(InventoryItem item)
    {
        items.RemoveAt(items.IndexOf(item));
    }

    void RemoveItemAt(int index)
    {
        items.RemoveAt(index);
    }

    int ItemCount()
    {
        return items.Count;
    }

    InventoryItem GetItem(int index)
    {
        return items[index];
    }
}

public class Equipment : MonoBehaviour
{
    public InventoryItem Head;
    public InventoryItem Chest;
    public InventoryItem Neck;
    public InventoryItem Shoulder;
    public InventoryItem Wrist;
    public InventoryItem Hand;
    public InventoryItem[] FingersLeft = new InventoryItem[2];
    public InventoryItem[] FingersRight = new InventoryItem[2];
    public InventoryItem Waist;
    public InventoryItem Feet;
}

