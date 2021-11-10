using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using UnityEngine;

public class InventoryItemConventer : MonoBehaviour
{
    private static InventoryItemConventer m_Instance;
    public static InventoryItemConventer Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new();

            return m_Instance;
        }
    }
    public UIItemInfo Conventer(InventoryItem item)
    {
        UIItemInfo tempItem = new();
        tempItem.ID = item.Id;
        tempItem.Name = item.ItemName;
        tempItem.Icon = item.Icon;
        tempItem.Description = item.ItemDescription;
        tempItem.Quality = (UIItemQuality)item.ItemQuality;
        tempItem.EquipType = UIEquipmentType.None;
        tempItem.ItemType = -1;
        tempItem.Type = null;
        tempItem.Subtype = null;
        tempItem.Damage = -1;
        tempItem.AttackSpeed = -1;
        tempItem.Block = -1;
        tempItem.Armor = -1;
        tempItem.Stamina = -1;
        tempItem.Strength = -1;
        tempItem.Durability = -1;
        tempItem.RequiredLevel = -1;
        return tempItem;
    }
}
