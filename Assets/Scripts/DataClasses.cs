using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 15 Sep 2020


[System.Serializable]
public class Player
{
    public int PlayerID;
    public string Name;
    public List<Hero> Team;

}

public enum AITypes { Closest, Lockon };



[System.Serializable]
public class Card
{
    //Base class for cards FOR NOW THIS IS USED BOTH CARDS AND SPELLS , BECAUSE OF THAT WE EVEN DO NORMAL ATTACKS BY "NORMAL ATTACK" CARD. it is for simpilicty.
    public int CardID;
    public string Name;
    public string Info;
    public int Power;
    public int Range;
    public bool Used;

    public Card(int cardID, string name, string info, int power, int range)
    {
        CardID = cardID;
        Name = name;
        Info = info;
        Power = power;
        Range = range;
    }
}