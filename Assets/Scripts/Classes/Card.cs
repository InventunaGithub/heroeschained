using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Mert Karavural
//Date: 8.11.2021
public enum CardTypes {GuildCard ,DraggableUlti , PickADirectionUlti, CastUlti}
public class Card : MonoBehaviour
{
    public int ID;
    public int SpellID;
    public string Name;
    public string Description;
    public CardTypes CardType;
    [HideInInspector]public GameObject UsingHero;
    private CardManager CM;
    private SpellManager SM;
    private Button cardButton;
    void Start()
    {
        CM = GameObject.Find("Managers").GetComponent<CardManager>();
        SM = GameObject.Find("Managers").GetComponent<SpellManager>();
        cardButton = GetComponent<Button>();
    }
    void Update()
    {
        if (CM.GuildEnergy >= SM.FindSpell(SpellID).EnergyCost)
        {
            cardButton.interactable = true;
        }
        else
        {
            cardButton.interactable = false;
        }
    }
}
