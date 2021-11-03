using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test8006 : MonoBehaviour
{
    Bid testBid;
    Bid testBid2;
    Bid testBid3;
    Bid testBid4;
    Bid testBid5;

    [SerializeField]InventoryItem testItem1;
    [SerializeField]InventoryItem testItem2;

    Auction testAuction1;
    Auction testAuction2;

    public AuctionManager testManager;


    private void Start()
    {
        testBid = new(10, "deniz", 1);
        testBid2 = new(15, "tolga", 2);
        testBid3 = new(20, "aynalı", 3);
        testBid4 = new(25, "kemer", 4);
        testBid5 = new(30, "incebele", 5);

        testAuction1 = new(testItem1, DateTime.Now + new TimeSpan(0, 0, 2), 1);
        testAuction2 = new(testItem2, DateTime.Now + new TimeSpan(0, 0, 5), 2);

        
        testAuction1.AddBid(testBid);
        testAuction1.AddBid(testBid3);
        testAuction2.AddBid(testBid2);
        testAuction2.AddBid(testBid4);
        testAuction2.AddBid(testBid5);

        testManager.AuctionList.Add(testAuction1);
        testManager.AuctionList.Add(testAuction2);



    }
}