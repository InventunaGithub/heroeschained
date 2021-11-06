using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataProvider : MonoBehaviour
{
    // standard members, which can exist in all games and vendors
    protected string gameId;

    public delegate void OnCreateRecordDelegate(string userId);
    public delegate void OnCompletionDelegate();
    public delegate void OnQueryUserCompletionDelegate(DOUser user);

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

    // fields specific to this game
    public abstract void CreateUser(DOUser user, OnCompletionDelegate onComplete);
    public abstract void GetUser(string userId, OnQueryUserCompletionDelegate onComplete);
    public abstract void SetUserFirstVisitProperty(string userId);
    public abstract void SetUserFirstVisitDate(string userId);
    public abstract void SetUserLastVisitDate(string userId);

}
