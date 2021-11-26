using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Deniz Afşar
//Date: 20.10.2021

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
    [SerializeField] private int capacity = 10;


    public bool Add(InventoryItem item)
    {
        if (!(items.Count >= capacity))
        {
            items.Add(item);
            return true;
        }
        else
        {
            Debug.Log("Achieve the capacity");
            return false;
        }
    }

    public bool RemoveItem(InventoryItem item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            return true;
        }
        else
        {
            Debug.Log("This item is not in inventory");
            return false;
        }
    }

    public bool RemoveItemAt(int index)
    {
        if (items.Count > index && items[index] != null)
        {
            items.RemoveAt(index);
            return true;
        }
        else
        {
            Debug.Log("Index is out of capacity or item is not found.");
            return false;
        }
    }

    public int ItemCount()
    {
        return items.Count;
    }

    public InventoryItem GetItem(int index)
    {
        return items[index];
    }

    public float ItemSellValue(InventoryItem item)
    {
        return item.SellValue;
    }

    public float ItemBuyValue(InventoryItem item)
    {
        return item.BuyValue;
    }
}