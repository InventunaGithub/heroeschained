using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    float saleTax = 0f;
    float auctionTax = 0f;
    float playerGold;
    GameCharacter playerGameChar;
    GameCharacter npcGameChar;

    private void Awake()
    {
        playerGameChar = GetComponent<GamePlayer>().PlayerGameCharacter;
        npcGameChar = GetComponent<GameNpc>().GameCharacterNpc;
        playerGold = GetComponent<GamePlayer>().Gold;
    }

    public bool Trade(GameCharacter Buyer, GameCharacter Seller, InventoryItem item, int index)
    {
        if((Buyer == playerGameChar) && (Seller == npcGameChar))
        {
            float tempValue = playerGameChar.GetSellValue(item, index);
            tempValue += tempValue * saleTax;
            if (playerGameChar.AddInventoryItem(item) && (playerGold >= tempValue))
            {
                playerGameChar.AddInventoryItem(item);
                playerGold -= tempValue;
                return true;
            }
            else
            {
                Debug.Log("Player hasn't any available item place or gold is insufficient.");
                return false;
            }
        }else if((Buyer == npcGameChar) && (Seller = playerGameChar))
        {
            float tempValue = playerGameChar.GetSellValue(item, index);
            if ((playerGameChar.RemoveInventoryItem(item)) && (playerGameChar.RemoveInventoryItemAt(index)))
            {
                playerGameChar.RemoveInventoryItemAt(index);
                playerGold += tempValue;
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
