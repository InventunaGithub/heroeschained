using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Deniz Afşar
//Date: 28.10.21

public class TradeManager : MonoBehaviour
{
    public float SaleTax = 0f;
    public float AuctionTax = 0f;

    public TradeResponse Trade(GameCharacter buyer, GameCharacter seller, InventoryItem item)
    {
        if((buyer is GamePlayer) && (seller is GameNpc))
        {
            if (((GamePlayer)buyer).Gold >= buyer.GetBuyValue(item))
            {
                if (buyer.AddInventoryItem(item))
                {
                    ((GamePlayer)buyer).Gold -= buyer.GetBuyValue(item);
                    return TradeResponse.OK;
                }
                else
                {
                    Debug.Log("Player hasn't any available item place.");
                    return TradeResponse.InsufficientInventorySpace;
                }
            }
            else
            {
                Debug.Log("Player's gold is insufficient.");
                return TradeResponse.InsufficientFunds;
            }
        }else if((buyer is GameNpc) && (seller is GamePlayer))
        {
            if (seller.RemoveInventoryItem(item))
            {
                ((GamePlayer)seller).Gold += buyer.GetSellValue(item);
                return TradeResponse.OK;
            }
            else
            {
                Debug.Log("Player hasn't got this item.");
                return TradeResponse.InexistentItem;
            }
        }
        Debug.Log("Trade is not happened");
        return TradeResponse.UnexpectedTradePair;
    }
}

