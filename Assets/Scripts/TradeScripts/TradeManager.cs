using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    float saleTax = 0f;
    float auctionTax = 0f;

    public bool Trade(GameCharacter buyer, GameCharacter seller, InventoryItem item)
    {
        if((buyer is GamePlayer) && (seller is GameNpc))
        {
            float tempSellValue = buyer.GetSellValue(item);
            tempSellValue += tempSellValue * saleTax;
            if (((GamePlayer)buyer).Gold >= tempSellValue)
            {
                if (buyer.AddInventoryItem(item))
                {
                    ((GamePlayer)buyer).Gold -= tempSellValue;
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
        }else if((buyer is GameNpc) && (seller is GamePlayer))
        {
            float tempSellValue = ((GamePlayer)seller).GetSellValue(item);
            if (seller.RemoveInventoryItem(item))
            {
                ((GamePlayer)seller).Gold += tempSellValue;
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
