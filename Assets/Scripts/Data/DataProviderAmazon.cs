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

    public override void GetUserRestricted(string userId, OnRestrictionDelegate onRestriction)
    {
        throw new System.NotImplementedException();
    }

    public override string Vendor()
    {
        return "Amazon AWS";
    }

    public override void GetNewMessageCount(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetMessageCount(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetMessageHeaders(string userId, int messageCount, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetSentMessageHeaders(string userId, int messageCount, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void SendMessage(string from, string senderName, string to, string title, string content)
    {
        throw new System.NotImplementedException();
    }

    public override void DeleteMessage(string sentTo, string messageId, OnCompletionDelegate onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetMessageBody(string sentTo, string messageId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void MarkMessageAsRead(string sentTo, string messageId)
    {
        throw new System.NotImplementedException();
    }

    public override void TestIfUserNameExists(string userName, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void SaveUserName(string userId, string userName, string mailAddress)
    {
        throw new System.NotImplementedException();
    }

    public override void SaveNickName(string userId, string nickName)
    {
        throw new System.NotImplementedException();
    }

    public override void AddResource(string userId, string name, int tier, int amount, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetResourceAmount(string userId, string name, int tier, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetResources(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetResourceAmount(string userId, string name, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void FetchLandResources(string landId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void CollectResource(string resourceId, string userId, string resourceType, int resourceTier, int amount, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void IfResourceCollected(string resourceId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void ResetResources()
    {
        throw new System.NotImplementedException();
    }

    public override void AddBeacon(Vector3 location, int index, string userId)
    {
        throw new System.NotImplementedException();
    }

    public override void GetUserBeacons(string userId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetLandInfo(string landId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void ResetBeacons(string userId)
    {
        throw new System.NotImplementedException();
    }

    public override void GetNearbyLands(Vector3 origin, float distance, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void GetLandDungeons(string landId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void IfDungeonCleaned(string landId, string dungeonId, string userId, OnCompletionDelegateWithParameter onComplete)
    {
        throw new System.NotImplementedException();
    }

    public override void VisitDungeon(int result, string landId, string dungeonId, string userId)
    {
        throw new System.NotImplementedException();
    }
}
