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
                m_Instance = Resources.Load("TestUI/Player") as UIInventoryItemDatabase;

            return m_Instance;
        }
    }

    public InventoryItem[] items;

    public InventoryItem Get(int index)
    {
        return (this.items[index]);
    }

    public InventoryItem GetByID(int ID)
    {
        for (int i = 0; i < this.items.Length; i++)
        {
            if (this.items[i].Id == ID)
                return this.items[i];
        }
        return null;
    }
}