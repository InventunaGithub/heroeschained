using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuloGames.UI;

//Author: Deniz Afşar
//Date: 28.10.21

public class TradeManager : MonoBehaviour
{
    public DataSource Data;
    public GUITownTemporary TownTemporary;

    private void Start()
    {
        //Data.GetProvider().AddResource(VariableManager.Instance.GetVariable("userid").ToString(), "gold", 1, 0, null);
    }

    public void TradeDialog(GameObject go)
    {
        if (go.CompareTag("MarketItem"))
        {
            TownTemporary.Confirm("Are you sure for buying this item?", "Yes"
                , "No", () => BuyTrade(null), () => { });
        }
        else if (go.CompareTag("PlayerItem"))
        {
            TownTemporary.Confirm("Are you sure for selling this item?", "Yes"
                , "No", () => SellTrade(null), () => { });
        }
    }

    public TradeResponse SellTrade(InventoryItem item)
    {
        //if (((GamePlayer)buyer).Gold >= buyer.GetBuyValue(item))
        //{
        //    if (buyer.AddInventoryItem(item))
        //    {
        //        ((GamePlayer)buyer).Gold -= buyer.GetBuyValue(item);
        //        return TradeResponse.OK;
        //    }
        //    else
        //    {
        //        Debug.Log("Player hasn't any available item place.");
        //        return TradeResponse.InsufficientInventorySpace;
        //    }
        //}
        //else
        //{
        //    Debug.Log("Player's gold is insufficient.");
        Debug.Log("Sold");
            return TradeResponse.InsufficientFunds;
        //}
    }
    public TradeResponse BuyTrade(InventoryItem item)
    {
        //    if (seller.RemoveInventoryItem(item))
        //    {
        //        ((GamePlayer)seller).Gold += buyer.GetSellValue(item);
        //        return TradeResponse.OK;
        //    }
        //    else
        //    {
        //    Debug.Log("Player hasn't got this item.");
        //    return TradeResponse.InexistentItem;
        //    }

        //Debug.Log("Trade is not happened");
        Debug.Log("Buyed");
        return TradeResponse.UnexpectedTradePair;
    }  
}