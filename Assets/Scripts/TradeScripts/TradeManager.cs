using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    float saleTax = 0f;
    float auctionTax = 0f;

    public bool Trade(GameCharacter Buyer, GameCharacter Seller, InventoryItem item)
    {
        if((Buyer is GamePlayer) && (Seller is GameNpc))
        {
            float tempValue = Buyer.GetSellValue(item);
            tempValue += tempValue * saleTax;
            if (((GamePlayer)Buyer).Gold >= tempValue)
            {
                if (Buyer.AddInventoryItem(item))
                {
                    ((GamePlayer)Buyer).Gold -= tempValue;
                    return true;
                }
                else
                {
                    Debug.Log("Player hasn't any available item place.");
                    return false;
                }
            }
            else
            {
                Debug.Log("Player's gold is insufficient.");
                return false;
            }
        }else if((Buyer is GameNpc) && (Seller is GamePlayer))
        {
            float tempValue = ((GamePlayer)Seller).GetSellValue(item);
            if (Seller.RemoveInventoryItem(item))
            {
                ((GamePlayer)Seller).Gold += tempValue;
                return true;
            }
            else
            {
                Debug.Log("Player hasn't got this item.");
                return false;
            }
        }
        Debug.Log("Trade is not happened");
        return false;
    }
}
