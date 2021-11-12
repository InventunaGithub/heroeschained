using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI InventoryItem Database", menuName = "Inventuna/Heroes Chained/UI InventoryItem Database")]
public class UIInventoryItemDatabase : ScriptableObject
{
    private static UIInventoryItemDatabase m_Instance;
    public static UIInventoryItemDatabase Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = Resources.Load("TestUI/Test") as UIInventoryItemDatabase;
            }  
            return m_Instance;
        }
    }
    
    public InventoryItem[] PlayerItems;
    public InventoryItem[] MarketItems;

    public InventoryItem GetPlayerItems(int index)
    {
        return (this.PlayerItems[index]);
    }

    public InventoryItem GetPlayerItemsById(int ID)
    {
        for (int i = 0; i < this.PlayerItems.Length; i++)
        {
            if (this.PlayerItems[i].Id == ID)
                return this.PlayerItems[i];
        }
        return null;
    }

    public InventoryItem GetMarketItems(int index)
    {
        return (this.MarketItems[index]);
    }

    public InventoryItem GetMarketItemsById(int ID)
    {
        for (int i = 0; i < this.MarketItems.Length; i++)
        {
            if (this.MarketItems[i].Id == ID)
                return this.MarketItems[i];
        }
        return null;
    }
}