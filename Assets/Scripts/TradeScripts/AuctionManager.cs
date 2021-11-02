using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


public class AuctionManager : MonoBehaviour
{
    public class Bid
    {
        ObscuredInt gold;
        string owner;
        ObscuredInt reserved1;
        ObscuredInt reserved2;
        ObscuredInt reserved3;
        float reserved4;
        string reserved5;
        DateTime givenAt;

        public Bid(ObscuredInt pGold, string pOwner)
        {
            gold = pGold;
            owner = pOwner;
            reserved1 = 0;
            reserved2 = 0;
            reserved3 = 0;
            reserved4 = 0;
            reserved5 = "0";
            givenAt = DateTime.UtcNow;
        }

        public Bid(ObscuredInt pGold, string pOwner, ObscuredInt pReserved1, ObscuredInt pReserved2, ObscuredInt pReserved3, float pReserved4, string pReserved5)
        {
            gold = pGold;
            owner = pOwner;
            reserved1 = pReserved1;
            reserved2 = pReserved2;
            reserved3 = pReserved3;
            reserved4 = pReserved4;
            reserved5 = pReserved5;
            givenAt = DateTime.UtcNow;
        }

        public ObscuredInt GetGold()
        {
            return gold;
        }

        public string GetOwner()
        {
            return owner;
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
    }

    public class Auction
    {
        private List<Bid> bidList;
        InventoryItem item;
        DateTime endsAt;
        int highestBid;
        string highestBidOwner;

        public Auction(InventoryItem pItem, int pHighestBid, string pHighestBidOwner)
        {
            item = pItem;
            endsAt = DateTime.UtcNow;
            highestBid = pHighestBid;
            highestBidOwner = pHighestBidOwner;
        }


    }
    

}


