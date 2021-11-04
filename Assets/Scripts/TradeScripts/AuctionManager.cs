using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

//Date: 2.11.2021
//author: Deniz Afşar

public class AuctionManager : MonoBehaviour
{
    public List<Auction> AuctionList = new();
    public float CheckSec = 1.0f;

    private void Update()
    { 
        for (int i = 0; i < AuctionList.Count; i++)
        {
            if (AuctionList[i].GetActive() && AuctionList[i].GetEndsAt() < DateTime.Now)
            {
                AuctionList[i].DisActivate();
                AuctionList[i].OnAuctionEnded = AuctionList[i].CompleteAuction;
            }
            AuctionList[i].AuctionProcess();
        }
    }  
}

public class Bid
{
    ObscuredInt gold;
    string owner;
    int ownerId;
    ObscuredInt reserved1;
    ObscuredInt reserved2;
    ObscuredInt reserved3;
    float reserved4;
    string reserved5;
    DateTime givenAt;

    public Bid(ObscuredInt pGold, string pOwner, int pOwnerId)
    {
        gold = pGold;
        owner = pOwner;
        ownerId = pOwnerId;
        reserved1 = 0;
        reserved2 = 0;
        reserved3 = 0;
        reserved4 = 0;
        reserved5 = "0";
        givenAt = DateTime.Now;
    }

    public Bid(ObscuredInt pGold, string pOwner, ObscuredInt pReserved1, ObscuredInt pReserved2, ObscuredInt pReserved3, float pReserved4, string pReserved5, int pOwnerId)
    {
        gold = pGold;
        owner = pOwner;
        ownerId = pOwnerId;
        reserved1 = pReserved1;
        reserved2 = pReserved2;
        reserved3 = pReserved3;
        reserved4 = pReserved4;
        reserved5 = pReserved5;
        givenAt = DateTime.Now;
    }

    #region Gets
    public ObscuredInt GetGold()
    {
        return gold;
    }

    public string GetOwner()
    {
        return owner;
    }

    public int GetOwnerId()
    {
        return ownerId;
    }

    public ObscuredInt GetReserved1()
    {
        return reserved1;
    }

    public ObscuredInt GetReserved2()
    {
        return reserved2;
    }

    public ObscuredInt GetReserved3()
    {
        return reserved3;
    }

    public float GetReserved4()
    {
        return reserved4;
    }

    public string GetReserved5()
    {
        return reserved5;
    }

    public DateTime GetGivenAt()
    {
        return givenAt;
    }
    #endregion Gets
}

public class Auction
{
    private List<Bid> bidList = new();
    private InventoryItem item;
    private DateTime endsAt;
    private int highestBid;
    private string highestBidOwner;
    private int highestBidOwnerId;
    private int id;

    bool active = true;

    public delegate void OnAuctionEndedFunc(int auctionId, int winnerId, InventoryItem item);
    [HideInInspector]
    public OnAuctionEndedFunc OnAuctionEnded = null;

    public Auction(InventoryItem pItem, DateTime pDateTime, int id)
    {
        item = pItem;
        endsAt = pDateTime;
        highestBid = 0;
        this.id = id;
    }

    public bool AddBid(Bid pBid)
    {
        if(pBid.GetGold() > highestBid)
        {
            highestBid = pBid.GetGold();
            highestBidOwner = pBid.GetOwner();
            highestBidOwnerId = pBid.GetOwnerId();
            bidList.Add(pBid);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AuctionProcess()
    {
        OnAuctionEnded?.Invoke(id, highestBidOwnerId, item);
    }

    public void CompleteAuction(int pAuctionId, int pWinnderId, InventoryItem pItem)
    {
        Debug.Log("Auction " + pAuctionId + " is over!");
        Debug.Log("The winner id : " + pWinnderId);
        Debug.Log(pItem.ItemName + " sold");
        OnAuctionEnded = null;
    }

    #region Gets
    public List<Bid> GetBidList()
    {
        return bidList;
    }

    public InventoryItem GetItem()
    {
        return item;
    }

    public DateTime GetEndsAt()
    {
        return endsAt;
    }

    public int GetHighestBid()
    {
        return highestBid;
    }

    public string GetHighestBidOwner()
    {
        return highestBidOwner;
    }

    public int GetHighestBidOwnerId()
    {
        return highestBidOwnerId;
    }

    public int GetBidCount()
    {
        return bidList.Count;
    }

    public Bid GetBidAt(int index)
    {
        return bidList[index];
    }

    public bool GetActive()
    {
        return active;
    }

    public int GetId()
    {
        return id;
    }
    #endregion Gets

    #region Sets
    public void SetHighestBid(int pHighestBid, string pHighestBidOwner)
    {
        highestBid = pHighestBid;
        highestBidOwner = pHighestBidOwner;
    }

    public void DisActivate()
    {
        active = false;
    }
    #endregion Sets
}