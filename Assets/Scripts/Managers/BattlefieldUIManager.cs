using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldUIManager : MonoBehaviour
{
    GameObject inGamePanel;
    GameObject formationPanel;
    void Awake()
    {
        inGamePanel = GameObject.Find("inGamePanel");
        formationPanel = GameObject.Find("formationPanel");
    }

    public void SetIngamePanel(bool set)
    {
        inGamePanel.SetActive(set);
    }
    public void SetFormationPanel(bool set)
    {
        formationPanel.SetActive(set);
    }

    //Lose - Win - Pause states
}
