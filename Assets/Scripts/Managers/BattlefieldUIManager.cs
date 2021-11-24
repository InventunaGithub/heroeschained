using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldUIManager : MonoBehaviour
{
    GameObject inGamePanel;
    GameObject formationPanel;
    // Start is called before the first frame update
    void Awake()
    {
        inGamePanel = GameObject.Find("inGamePanel");
        formationPanel = GameObject.Find("formationPanel");
    }

    // Update is called once per frame
    void Update()
    {

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
