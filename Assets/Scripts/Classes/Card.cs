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
    public GameObject SkillMesh;
    private CardManager cardManager;
    private SpellManager spellManager;
    private Button cardButton;
    void Start()
    {
        cardManager = GameObject.Find("Managers").GetComponent<CardManager>();
        spellManager = GameObject.Find("Managers").GetComponent<SpellManager>();
        cardButton = GetComponent<Button>();
    }
    void Update()
    {
        if (cardManager.GuildEnergy >= spellManager.FindSpell(SpellID).EnergyCost)
        {
            cardButton.interactable = true;
        }
        else
        {
            cardButton.interactable = false;
        }
    }
}
