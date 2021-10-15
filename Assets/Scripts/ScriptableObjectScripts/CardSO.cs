using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Inventuna/HeroesChained/Card")]
public class CardSO : ScriptableObject
{
    //Base class for cards FOR NOW THIS IS USED BOTH CARDS AND SPELLS , BECAUSE OF THAT WE EVEN DO NORMAL ATTACKS BY "NORMAL ATTACK" CARD. it is for simpilicty.
    public int CardID;
    public string Name;
    public string Info;
    public int Power;
    public int Energy;
    public int RemainingTurn;
    public int Range;
    public SkillTypes skillType;
    public SkillTargets skillTarget;
    public StatusEffect statusEffect; // can be use to finish the fight mechanic by interrupt 
    public Sprite artwork;
}
