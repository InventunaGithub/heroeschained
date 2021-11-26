using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static MessagingManager;

public abstract class DataProvider : MonoBehaviour
{
    // standard members, which can exist in all games and vendors
    protected string gameId;

    public delegate void OnCreateRecordDelegate(string userId);
    public delegate void OnCompletionDelegate();
    public delegate void OnQueryUserCompletionDelegate(DOUser user);
    public delegate void OnRestrictionDelegate(DateTime restrictedUntil, string restrictionResult);
    public delegate void OnCompletionDelegateWithParameter(object result);

    public string GameId
    {
        get
        {
            return gameId;
        }
    }

    public abstract bool Connect(string gameId);
    public abstract bool Connect(string gameId, string[] args);
    public abstract bool Disonnect();

    public abstract string Vendor();

    // Fields specific to this game

    // User related commands
    public abstract void CreateUser(DOUser user, OnCompletionDelegate onComplete);
    public abstract void GetUser(string userId, OnQueryUserCompletionDelegate onComplete);
    public abstract void SetUserFirstVisitProperty(string userId);
    public abstract void SetUserFirstVisitDate(string userId);
    public abstract void SetUserLastVisitDate(string userId);
    public abstract void GetUserRestricted(string userId, OnRestrictionDelegate onRestriction);

    // Messaging related commands
    public abstract void GetNewMessageCount(string userId, OnCompletionDelegateWithParameter onComplete);
    public abstract void GetMessageCount(string userId, OnCompletionDelegateWithParameter onComplete);
    public abstract void GetMessageHeaders(string userId, int messageCount, OnCompletionDelegateWithParameter onComplete);
    public abstract void GetSentMessageHeaders(string userId, int messageCount, OnCompletionDelegateWithParameter onComplete);
    public abstract void SendMessage(string from, string senderName, string to, string title, string content);
    public abstract void DeleteMessage(string sentTo, string messageId, OnCompletionDelegate onComplete);
    public abstract void GetMessageBody(string sentTo, string messageId, OnCompletionDelegateWithParameter onComplete);
    public abstract void MarkMessageAsRead(string sentTo, string messageId);
    public abstract void TestIfUserNameExists(string userName, OnCompletionDelegateWithParameter onComplete);
    public abstract void SaveUserName(string userId, string userName, string mailAddress);
    public abstract void SaveNickName(string userId, string nickName);

    // Resource related commands
    public abstract void AddResource(string userId, string name, int tier, int amount, OnCompletionDelegateWithParameter onComplete);
    public abstract void GetResourceAmount(string userId, string name, int tier, OnCompletionDelegateWithParameter onComplete);
    public abstract void GetResourceAmount(string userId, string name, OnCompletionDelegateWithParameter onComplete);
    public abstract void GetResources(string userId, OnCompletionDelegateWithParameter onComplete);
    public abstract void CollectResource(string resourceId, string userId, string resourceType, int resourceTier, int amount, OnCompletionDelegateWithParameter onComplete);
    public abstract void IfResourceCollected(string resourceId, string userId, OnCompletionDelegateWithParameter onComplete);

    // Land related commands
    public abstract void FetchLandResources(string landId, string userId, OnCompletionDelegateWithParameter onComplete);
    public abstract void GetLandInfo(string landId, OnCompletionDelegateWithParameter onComplete);

    // Beacon related commands
    public abstract void AddBeacon(Vector3 location, int index, string userId);
    public abstract void GetUserBeacons(string userId, OnCompletionDelegateWithParameter onComplete);
    public abstract void ResetBeacons(string userId);
    public abstract void GetNearbyLands(Vector3 origin, float distance, OnCompletionDelegateWithParameter onComplete);

    // Dungeon related commands
    public abstract void GetLandDungeons(string landId, OnCompletionDelegateWithParameter onComplete);
    public abstract void IfDungeonCleaned(string landId, string dungeonId, string userId, OnCompletionDelegateWithParameter onComplete);
    public abstract void VisitDungeon(int result, string landId, string dungeonId, string userId);

    // Misc. Commands
    public abstract void ResetResources();
}