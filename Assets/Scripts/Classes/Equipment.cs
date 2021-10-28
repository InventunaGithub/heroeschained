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
    public InventoryItem HandRight;
    public InventoryItem HandLeft;
    public InventoryItem[] FingersLeft = new InventoryItem[2];
    public InventoryItem[] FingersRight = new InventoryItem[2];
    public InventoryItem Waist;
    public InventoryItem Feet;

    public const int RingLeft1 = 10;
    public const int RingLeft2 = 11;
    public const int RingRight1 = 20;
    public const int RingRight2 = 21;

    //public const int HandRight = 00;
    //public const int HandLeft = 01;

    public InventoryItem GetItem(EquipmentItemPlaces places)
    {
        switch (places)
        {
            case EquipmentItemPlaces.Head:
                return Head;
            case EquipmentItemPlaces.Chest:
                return Chest;
            case EquipmentItemPlaces.Neck:
                return Neck;
            case EquipmentItemPlaces.Shoulder:
                return Shoulder;
            case EquipmentItemPlaces.Wrist:
                return Wrist;
            case EquipmentItemPlaces.HandLeft:
                return HandLeft;
            case EquipmentItemPlaces.HandRight:
                return HandRight;
            case EquipmentItemPlaces.FingerLeft1:
                return FingersLeft[0];
            case EquipmentItemPlaces.FingerLeft2:
                return FingersLeft[1];
            case EquipmentItemPlaces.FingerRight1:
                return FingersRight[0];
            case EquipmentItemPlaces.FingerRight2:
                return FingersRight[1];
            case EquipmentItemPlaces.Waist:
                return Waist;
            case EquipmentItemPlaces.Feet:
                return Feet;
        }
        return null;
    }

    public bool WearItems(InventoryItem item)
    {
        switch (item.ItemType)
        {
            case ItemTypes.Helm:
                if (GetItem(EquipmentItemPlaces.Head) != null)
                {
                    return false;
                }
                else
                {
                    Head = item;
                    return true;
                }
            case ItemTypes.Weapon:
                if (GetItem(EquipmentItemPlaces.HandRight) != null)
                {
                    return false;
                }
                else
                {
                    HandRight = item;
                    return true;
                }
            case ItemTypes.Weapon2Handed:
                if (GetItem(EquipmentItemPlaces.HandRight) != null && GetItem(EquipmentItemPlaces.HandLeft) != null)
                {
                    return false;
                }
                else
                {
                    HandRight = item;
                    return true;
                }
            case ItemTypes.Shield:
                if (GetItem(EquipmentItemPlaces.HandLeft) != null)
                {
                    return false;
                }
                else
                {
                    HandLeft = item;
                    return true;
                }
            case ItemTypes.Bracelet:
                if (GetItem(EquipmentItemPlaces.Wrist) != null)
                {
                    return false;
                }
                else
                {
                    Wrist = item;
                    return true;
                }
            case ItemTypes.Shoulder:
                if (GetItem(EquipmentItemPlaces.Shoulder) != null)
                {
                    return false;
                }
                else
                {
                    Shoulder = item;
                    return true;
                }
            case ItemTypes.Armor:
                if (GetItem(EquipmentItemPlaces.Chest) != null)
                {
                    return false;
                }
                else
                {
                    Chest = item;
                    return true;
                }
            case ItemTypes.Belt:
                if (GetItem(EquipmentItemPlaces.Waist) != null)
                {
                    return false;
                }
                else
                {
                    Waist = item;
                    return true;
                }
            case ItemTypes.Boot:
                if (GetItem(EquipmentItemPlaces.Feet) != null)
                {
                    return false;
                }
                else
                {
                    Feet = item;
                    return true;
                }
            case ItemTypes.Necklace:
                if (GetItem(EquipmentItemPlaces.Neck) != null)
                {
                    return false;
                }
                else
                {
                    Neck = item;
                    return true;
                }
        }
        return false;
    }

    public bool WearItems(InventoryItem item, int additional)
    {
        switch (additional)
        {
            case RingLeft1:
                if (GetItem(EquipmentItemPlaces.FingerLeft1) != null)
                {
                    return false;
                }
                else
                {
                    FingersLeft[0] = item;
                    return true;
                }
            case RingLeft2:
                if (GetItem(EquipmentItemPlaces.FingerLeft2) != null)
                {
                    return false;
                }
                else
                {
                    FingersLeft[1] = item;
                    return true;
                }
            case RingRight1:
                if (GetItem(EquipmentItemPlaces.FingerRight1) != null)
                {
                    return false;
                }
                else
                {
                    FingersRight[0] = item;
                    return true;
                }
            case RingRight2:
                if (GetItem(EquipmentItemPlaces.FingerRight2) != null)
                {
                    return false;
                }
                else
                {
                    FingersRight[1] = item;
                    return true;
                }
        }
        return false;
    }

    public bool UnWearItems(InventoryItem item)
    {
        switch (item.ItemType)
        {
            case ItemTypes.Helm:
                if (GetItem(EquipmentItemPlaces.Head) == null)
                {
                    return false;
                }
                else
                {
                    Head = null;
                    return true;
                }
            case ItemTypes.Weapon:
                if (GetItem(EquipmentItemPlaces.HandRight) == null)
                {
                    return false;
                }
                else
                {
                    HandRight = null;
                    return true;
                }
            case ItemTypes.Weapon2Handed:
                if (GetItem(EquipmentItemPlaces.HandRight) == null)
                {
                    return false;
                }
                else
                {
                    HandRight = null;
                    return true;
                }
            case ItemTypes.Shield:
                if (GetItem(EquipmentItemPlaces.HandLeft) == null)
                {
                    return false;
                }
                else
                {
                    HandLeft = null;
                    return true;
                }
            case ItemTypes.Bracelet:
                if (GetItem(EquipmentItemPlaces.Wrist) == null)
                {
                    return false;
                }
                else
                {
                    Wrist = null;
                    return true;
                }
            case ItemTypes.Shoulder:
                if (GetItem(EquipmentItemPlaces.Shoulder) == null)
                {
                    return false;
                }
                else
                {
                    Shoulder = null;
                    return true;
                }
            case ItemTypes.Armor:
                if (GetItem(EquipmentItemPlaces.Chest) == null)
                {
                    return false;
                }
                else
                {
                    Chest = null;
                    return true;
                }
            case ItemTypes.Belt:
                if (GetItem(EquipmentItemPlaces.Waist) == null)
                {
                    return false;
                }
                else
                {
                    Waist = null;
                    return true;
                }
            case ItemTypes.Boot:
                if (GetItem(EquipmentItemPlaces.Feet) == null)
                {
                    return false;
                }
                else
                {
                    Feet = null;
                    return true;
                }
            case ItemTypes.Necklace:
                if (GetItem(EquipmentItemPlaces.Neck) == null)
                {
                    return false;
                }
                else
                {
                    Neck = null;
                    return true;
                }
        }
        return false;
    }

    public bool UnWearItems(InventoryItem item, int additional)
    {
        switch (additional)
        {
            case Equipment.RingLeft1:
                if (GetItem(EquipmentItemPlaces.FingerLeft1) != null)
                {
                    FingersLeft[0] = null;
                    return true;
                }
                else
                {
                    return false;
                }
            case Equipment.RingLeft2:
                if (GetItem(EquipmentItemPlaces.FingerLeft2) != null)
                {
                    FingersLeft[1] = null;
                    return true;
                }
                else
                {
                    return false;
                }
            case Equipment.RingRight1:
                if (GetItem(EquipmentItemPlaces.FingerRight1) != null)
                {
                    FingersRight[0] = null;
                    return true;
                }
                else
                {
                    return false;
                }
            case Equipment.RingRight2:
                if (GetItem(EquipmentItemPlaces.FingerRight2) != null)
                {
                    FingersRight[1] = null;
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }
}