using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Tolga KAranlıkoğlu
//Date: 7 Nov 2020

[CreateAssetMenu(fileName = "New Quest", menuName = "Inventuna/Heroes Chained/Quest")]
public class Quest : ScriptableObject
{
    public enum QuestState { NotAvailable, Available, Attempted, Completed, Failed }
    public QuestState State = QuestState.NotAvailable;

    public int ID;
    public string Title;
    public string TitleLong;
    public string Description;

    public int[] RequirementsListRequiredAmount;
    public int[] RequirementsListSatisfiedAmount;
    public string[] RequirementsListRight;
    public string[] RewardsList;
    public Image[] RewardsImages;
}
