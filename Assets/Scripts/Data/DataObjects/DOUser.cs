using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOUser : DataObject
{
    private string id;

    public string ID
    {
        get
        {
            return id;
        }
    }
    public string NickName;
    public string UserName;
    public DateTime RestrictedUntil = DateTime.MinValue;
    public string RestrictionReason = "";
    public DateTime LastLogon = DateTime.MinValue;
    public string CustodialWalletId;

    public string ReservedS1;
    public string ReservedS2;
    public int Reserved1;
    public int Reserved2;
    public int Reserved3;
    public float Reserved4;
    public float Reserved5;

    public bool CityTavernOpen = false;
    public bool CityArenaOpen = false;
    public bool CityMarketOpen = false;
    public bool CityRoyalPalaceOpen = false;
    public bool CitySlumsOpen = false;
    public bool CityGateOpen = false;
    public bool GuildTavernOpen = false;
    public bool GuildCourtOpen = false;
    public bool GuildGarageOpen = false;
    public bool GuildHealingOpen = false;
    public bool GuildPetOpen = false;
    public bool GuildScoutOpen = false;
    public bool GuildSmithOpen = false;
    public bool GuildTrainingGroundsOpen = false;

    public List<DOUserGameMatch> Games = new List<DOUserGameMatch>();

    public DOUser(string id)
    {
        this.id = id;
    }

    public DOUser(string id, string name)
    {
        this.id = id;
        this.NickName = name;
    }
}
