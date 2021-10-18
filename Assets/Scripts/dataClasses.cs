using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int PlayerID;
    public string Name;
    public List<Hero> Team;
    public List<Card> Deck;
    public List<Card> Hand = new List<Card>();
    public List<Card> Graveyard;
    public int Energy;
    public int maxEnergy = 100;

    public void ResetEnergy(int ResetAmount)
    {
        Energy += ResetAmount;
        if(Energy > maxEnergy)
        {
            Energy = maxEnergy;
        }
    }

    public void EndTurn()
    {
        ResetEnergy(maxEnergy);
        Realign();
    }

    public void Realign()
    {
        Hero temp = Team[0];
        for (int j = 0; j <= Team.Count - 2; j++)
        {
            for (int i = 0; i <= Team.Count - 2; i++)
            {
                if (Team[i].Dexterity < Team[i + 1].Dexterity)
                {
                    temp= Team[i + 1];
                    Team[i + 1] = Team[i];
                    Team[i] = temp;
                }
            }
        }

    }

    public void pullCard()
    {
        Hand.Add(Deck[0]);
        Deck.RemoveAt(0);
    }

    public void shuffleDeck()
    {
        var count = Deck.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = Deck[i];
            Deck[i] = Deck[r];
            Deck[r] = tmp;
        }
    }

}
[System.Serializable]
public class Item
{
    public int ItemID;
    public string Name;
    public string ItemType;
    public int Power;
    public string BuffType;

    public Item(int SkillID, string name, int power, string itemType, string buffType)
    {
        Name = name;
        Power = power;
        ItemType = itemType;
        BuffType = buffType;
    }
}

public enum SkillTypes { buff, debuff, heal, damage, magicDamage, interrupt, piercing, shake, format, stabilise, bloodritual, godlightning};
public enum SkillTargets { arena, singleTarget, enemies, allies, card, self, king };
public enum StatusEffect { burn, heal, damage, atk, dex, armor, freeze, sleep, exhaust, daze, electrified, wet, bleed, energy, none };
public enum AITypes {Closest , Lockon};

[System.Serializable]
public class Card
{
    //Base class for cards FOR NOW THIS IS USED BOTH CARDS AND SPELLS , BECAUSE OF THAT WE EVEN DO NORMAL ATTACKS BY "NORMAL ATTACK" CARD. it is for simpilicty.
    public int CardID;
    public string Name;
    public string Info;
    public int Power;
    public int Energy;
    public int RemainingTurn;
    public int Range;
    public bool used;
    public Sprite artwork;
    public SkillTypes skillType;
    public SkillTargets skillTarget;
    public StatusEffect statusEffect; // can be use to finish the fight mechanic by interrupt 

    public Card(int CardID, string Name, string Info, int Power, int Energy, int RemainingTurn, int Range, SkillTypes skillType, SkillTargets skillTarget, StatusEffect statusEffect, Sprite artwork = null)
    {
        this.CardID = CardID;
        this.Name = Name;
        this.Info = Info;
        this.Power = Power;
        this.Energy = Energy;
        this.Range = Range;
        this.RemainingTurn = RemainingTurn;
        this.skillType = skillType;
        this.skillTarget = skillTarget;
        this.statusEffect = statusEffect;
        if(artwork != null)
        {
            this.artwork = artwork;
        }
    }
}

[System.Serializable]
public class Hero
{
    public int HeroID; //Uniq id ?
    public string Name;
    //public Item[] Items;
    public List<Card> HeroSkills;
    public List<Card> HeroStatuses; // this should be skills or card ?
    public GameObject heroObject;
    public int BaseHealth;
    public int BaseDamage;
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Vitality;
    public int Armor;
    public int Damage;
    public int Health;
    public int maxHealth;
    public int Range;
    public AITypes AIType;
    public bool isFrozen;
    public bool isAsleep;
    public bool isDazed;
    public bool isExhausted;
    public bool isBleeding;
    public bool isBurning;
    public bool isWet;
    bool isElectirified;


    public Hero(string name, int heroID, int baseHealth, int baseDamage, int strength, int dexterity, int intelligence, int vitality, int Range, List<Card> HeroSkills = null , AITypes AIType = AITypes.Closest)
    {
        HeroID = heroID;
        Name = name;
        BaseHealth = baseHealth;
        Strength = strength;
        Dexterity = dexterity;
        Intelligence = intelligence;
        Vitality = vitality;
        this.Range = Range;
        this.BaseDamage = baseDamage;
        Health = baseHealth + (Vitality * 2);
        maxHealth = Health;
        Damage = BaseDamage + (strength * 2);
        Armor = 10;
        HeroStatuses = new List<Card>();
        if(HeroSkills == null)
        {
            HeroSkills = new List<Card>();
        }
        this.HeroSkills = HeroSkills;
        this.AIType = AIType; 
    }

    public bool CanPlay()// Returns if the hero can play this turn.
    {
        if (isFrozen || isAsleep || isDazed)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Card usedSkill(int skillNumber)
    {  
        Card TempCard = new Card(HeroSkills[skillNumber].CardID, HeroSkills[skillNumber].Name, HeroSkills[skillNumber].Info, HeroSkills[skillNumber].Power, HeroSkills[skillNumber].Energy, HeroSkills[skillNumber].RemainingTurn, HeroSkills[skillNumber].Range, HeroSkills[skillNumber].skillType, HeroSkills[skillNumber].skillTarget, HeroSkills[skillNumber].statusEffect, HeroSkills[skillNumber].artwork);
        if(TempCard.skillType == SkillTypes.damage)
        {
            TempCard.Power += Damage;
        }
        if (TempCard.skillType == SkillTypes.magicDamage)
        {
            TempCard.Power += (Intelligence * 2);
        }
        if (TempCard.skillType == SkillTypes.heal)
        {
            TempCard.Power += (Intelligence * 2);
        }
        return TempCard;
    }
    
    public void effectedBy(Card card) //This is used for most situations. Normal attacks and such still counts as cards.
    {
        
        if (card.skillType == SkillTypes.godlightning)
        {
            Health -= card.Power;
            Damage += card.Power;
        }
        if (card.skillType == SkillTypes.bloodritual)
        {
            Health -= card.Power;
            Damage += card.Power;
        }

        if (card.skillType == SkillTypes.format)
        {
            foreach (var status in HeroStatuses)
            {
                if (status.skillType == SkillTypes.buff)
                {
                    status.RemainingTurn = 0;
                }
            }
        }

        if (card.skillType == SkillTypes.stabilise)
        {
            foreach (var status in HeroStatuses)
            {
                if (status.skillType == SkillTypes.debuff)
                {
                    status.RemainingTurn = 0;
                }
            }
        }
        if (card.skillType == SkillTypes.shake)
        {
            hurt(2); //deals 2 damage then deletes all effects
            isAsleep = false;
            isDazed = false;
            isFrozen = false;
            foreach (var status in HeroStatuses)
            {
                if(status.statusEffect == StatusEffect.daze || status.statusEffect == StatusEffect.sleep || status.statusEffect == StatusEffect.freeze)
                {
                    status.RemainingTurn = 0; 
                }
            }
        }
        if (card.skillType == SkillTypes.piercing)
        {
            Health -= card.Power;
        }
        if (card.skillType == SkillTypes.heal)
        {
            Health += card.Power;
        }
        if (card.skillType == SkillTypes.damage || card.skillType == SkillTypes.magicDamage)
        {
            hurt(card.Power);
        }

        if (card.statusEffect == StatusEffect.atk)
        {
            if (card.skillType == SkillTypes.buff)
            {
                Damage += card.Power;
            }
            if (card.skillType == SkillTypes.debuff)
            {
                Damage -= card.Power;
            }
        }
        if (card.statusEffect == StatusEffect.dex)
        {
            if (card.skillType == SkillTypes.buff)
            {
                Dexterity += card.Power;
            }
            if (card.skillType == SkillTypes.debuff)
            {
                Dexterity -= card.Power;
            }
        }
        if (card.statusEffect == StatusEffect.armor)
        {
            if (card.skillType == SkillTypes.buff)
            {
                Armor += card.Power;
            }
            if (card.skillType == SkillTypes.debuff)
            {
                Armor -= card.Power;
            }
        }
        if (card.statusEffect == StatusEffect.freeze)
        {
            isFrozen = true;
        }
        if (card.statusEffect == StatusEffect.burn)
        {
            isBurning = true;
        }
        if (card.statusEffect == StatusEffect.bleed)
        {
            isBleeding = true;
        }
        if (card.statusEffect == StatusEffect.wet)
        {
            isWet = true;
        }
        if (card.statusEffect == StatusEffect.sleep)
        {
            isAsleep = true;
        }
        if (card.statusEffect == StatusEffect.daze)
        {
            isDazed = true;
        }
        if(card.statusEffect != StatusEffect.none)
        {
            HeroStatuses.Add(new Card(card.CardID, card.Name, card.Info, card.Power, card.Energy, card.RemainingTurn, card.Range, card.skillType, card.skillTarget, card.statusEffect, card.artwork));
        }
        Normalise();
    }
    
    void hurt(int damage) // this is used for physical attacks , attacks that must pierce armor first.
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
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }

    }

    public string PreviewStats()//Returns hero's base stats as a string.
    {
        string preview = Name;
        preview += " stats ;";
        preview += "\n Health : " + Health.ToString();
        preview += "\n Armor : " + Armor.ToString();
        preview += "\n Str : " + Strength.ToString();
        preview += "\n Dex : " + Dexterity.ToString();
        preview += "\n Vit : " + Vitality.ToString();
        preview += "\n Int : " + Intelligence.ToString();
        preview += "\n Damage : " + Damage.ToString();
        foreach (var status in HeroStatuses)
        {
            preview += "\n Status Effect : " + status.statusEffect.ToString() + " " + status.skillType.ToString() + " " + status.RemainingTurn ;
        }
        return preview;
    }
}

