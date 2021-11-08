using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tolga Karanlıkoğlu
// Nov 2021

public class QuestManager : MonoBehaviour
{
    public Quest[] QuestList;
    public GUITownTemporary GUI;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ActivateQuest(int id)
    {
        for (int i = 0; i < QuestList.Length; i++)
        {
            if(QuestList[i].ID == id)
            {
                if(QuestList[i].State == Quest.QuestState.NotAvailable || QuestList[i].State == Quest.QuestState.Available)
                {
                    QuestList[i].State = Quest.QuestState.Attempted;
                }

                // TEMPORARY PART - START
                if (id == 101)
                {
                    GUI.MakeGuildBuildingAvailable(GUITownTemporary.GuildBuildings.Tavern);
                }
                // TEMPORARY PART - END

                return;
            }
        }
    }

    public void MakeQuestAvailable(int id)
    {
        for (int i = 0; i < QuestList.Length; i++)
        {
            if (QuestList[i].ID == id)
            {
                if (QuestList[i].State == Quest.QuestState.NotAvailable)
                {
                    QuestList[i].State = Quest.QuestState.Available;
                }

                return;
            }
        }
    }
}
