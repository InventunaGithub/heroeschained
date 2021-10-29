using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Deniz Afşar
//Date: 28.10.21

public class TradeManager : MonoBehaviour
{
    public float SaleTax = 0f;
    public float AuctionTax = 0f;
    public int MinArbPer = 10;
    public int MaxArbPer = 20;

    public TradeResponse Trade(GameCharacter buyer, GameCharacter seller, InventoryItem item)
    {
        if((buyer is GamePlayer) && (seller is GameNpc))
        {
            float tempSellValue = buyer.GetSellValue(item);
            if (((GamePlayer)buyer).Gold >= tempSellValue)
            {
                if (buyer.AddInventoryItem(item))
                {
                    ((GamePlayer)buyer).Gold -= tempSellValue;
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
            float tempSellValue = ((GamePlayer)seller).GetSellValue(item);
            int randomArb = Random.Range(MinArbPer, MaxArbPer);
            tempSellValue -= tempSellValue * randomArb;
            if (seller.RemoveInventoryItem(item))
            {
                ((GamePlayer)seller).Gold += tempSellValue;
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

