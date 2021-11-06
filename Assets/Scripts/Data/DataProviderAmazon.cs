using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProviderAmazon : DataProvider
{
    public override bool Connect(string gameId)
    {
        this.gameId = gameId;

        return true;
    }

    public override bool Connect(string gameId, string[] args)
    {
        return Connect(gameId);
    }

    public override bool Disonnect()
    {
        return true;
    }

    public override void CreateUser(DOUser user, OnCompletionDelegate onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetUser(string userId, OnQueryUserCompletionDelegate onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void SetUserFirstVisitDate(string userId)
    {
        throw new System.NotImplementedException();
    }

    public override void SetUserFirstVisitProperty(string userId)
    {
        throw new System.NotImplementedException();
    }

    public override void SetUserLastVisitDate(string userId)
    {
        throw new System.NotImplementedException();
    }

    public override string Vendor()
    {
        return "Amazon";
    }
}
