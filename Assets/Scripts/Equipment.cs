using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Deniz Afşar
//Date: 20.10.2021

public class Equipment : MonoBehaviour
{
    public InventoryItem Head;
    public InventoryItem Chest;
    public InventoryItem Neck;
    public InventoryItem Shoulder;
    public InventoryItem Wrist;
    public InventoryItem Hand;
    public InventoryItem[] FingersLeft = new InventoryItem[2];
    public InventoryItem[] FingersRight = new InventoryItem[2];
    public InventoryItem Waist;
    public InventoryItem Feet;
}