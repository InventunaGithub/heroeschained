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

    public Card(int CardID, string Name, string Info, int Power, int Range)
    {
        this.CardID = CardID;
        this.Name = Name;
        this.Info = Info;
        this.Power = Power;
        this.Range = Range;
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


    public Hero(string Name, int HeroID, int BaseHealth, int BaseDamage, int Strength, int Dexterity, int Intelligence, int Vitality, int Range, List<Card> Skills = null , AITypes AIType = AITypes.Closest)
    {
        this.HeroID = HeroID;
        this.Name = Name;
        this.BaseHealth = BaseHealth;
        this.Strength = Strength;
        this.Dexterity = Dexterity;
        this.Intelligence = Intelligence;
        this.Vitality = Vitality;
        this.Range = Range;
        this.BaseDamage = BaseDamage;
        Health = this.BaseHealth + (this.Vitality * 2);
        MaxHealth = Health;
        Damage = BaseDamage + (this.Strength * 2);
        Armor = 10;
        this.Skills = Skills;
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

