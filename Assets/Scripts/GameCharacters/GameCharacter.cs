using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Deniz Afşar
//Date: 28.10.21

public class GameCharacter : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    public bool AddInventoryItem(InventoryItem item)
    {
        return inventory.Add(item);
    }

    public bool RemoveInventoryItem(InventoryItem item)
    {
        return inventory.RemoveItem(item);
    }

    public bool RemoveInventoryItemAt(int index)
    {
        return inventory.RemoveItemAt(index);
    }

    public int InventoryItemCount()
    {
        return inventory.ItemCount();
    }

    public InventoryItem GetInventoryItem(int index)
    {
        return inventory.GetItem(index);
    }

    public float GetSellValue(InventoryItem item)
    {
        return inventory.ItemSellValue(item);
    }

    public float GetBuyValue(InventoryItem item)
    {
        return inventory.ItemBuyValue(item);
    }
}
