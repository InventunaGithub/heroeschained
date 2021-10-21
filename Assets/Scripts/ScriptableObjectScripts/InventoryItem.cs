using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Deniz Afşar
//Date: 19.10.2021

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventuna/HeroesChained/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public int Id;
    public int MinLevel;
    public int MaxLevel;
    public int SpecificToClass = 0;
    public int ItemLevel = 1;

    public string ItemName;
    public string ItemDescription;

    public ItemTypes ItemType = ItemTypes.Misc;
    public ItemQualities ItemQuality = ItemQualities.Common;

    public bool Consumable = false;
    public bool AccountBound = false;

    public float SellValue;

    public enum ItemTypes
    {
        Weapon, WeaponTwoHanded, Shield, Bracelet, Shoulder,
        Armor, Helm, Belt, Boot, Ring, Amulet, Gem, Necklace,
        Gold, Potion, Misc, Other
    }

    public enum ItemQualities
    {
        Common, Uncommon, Rare, Unique
    }

    public enum Other
    {
        Gem, Scroll, Card, CardConsumable, Useless
    }
}