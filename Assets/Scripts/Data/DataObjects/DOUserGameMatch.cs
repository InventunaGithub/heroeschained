using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOUserGameMatch : DataObject
{
    // Same for all users and all games
    private string userId;
    private string gameId;

    public string UserId
    {
        get
        {
            return userId;
        }
    }

    public string GameId
    {
        get
        {
            return gameId;
        }
    }

    public string ConnectedWallet = "";
    public string RestrictionReason = "";
    public DateTime RestrictedAt = DateTime.MinValue;
    public DateTime FirstPlayedAt = DateTime.MinValue;
    public DateTime LastPlayedAt = DateTime.MinValue;
    public string WalletId = "";

    // specific to this game
    public bool FirstVisit = false;

    // heroes?

    public Inventory UserInventory = new Inventory();
    public Equipment UserEquipment = new Equipment();

    // standard consturctor
    public DOUserGameMatch(string userId, string gameId)
    {
        this.userId = userId;
        this.gameId = gameId;
    }

}
