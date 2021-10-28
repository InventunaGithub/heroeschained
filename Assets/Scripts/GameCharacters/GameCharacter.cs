using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float GetSellValue(InventoryItem item, int index)
    {
        return inventory.ItemSellValue(item, index);
    }
}
