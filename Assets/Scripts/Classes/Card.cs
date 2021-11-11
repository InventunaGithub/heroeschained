using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Mert Karavural
//Date: 8.11.2021
public class Card : MonoBehaviour
{
    public int ID;
    public int Energy;
    public int SpellID;
    public string Name;
    public string Description;
    private CardManager CM;
    private Button cardButton;
    void Start()
    {
        CM = GameObject.Find("Managers").GetComponent<CardManager>();
        cardButton = GetComponent<Button>();
    }
    void Update()
    {
        if(CM.GuildEnergy >= Energy)
        {
            cardButton.interactable = true;
        }
        else
        {
            cardButton.interactable = false;
        }
    }
}
