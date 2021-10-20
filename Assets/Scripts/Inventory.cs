using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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