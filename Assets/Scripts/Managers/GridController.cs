using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 27 Oct 2020

public class GridController : MonoBehaviour
{
    [HideInInspector] public Renderer GridRenderer;
    FormationManager formationManager;
    public GameObject HeroOnGrid;
    public int ID;
    void Start()
    {
        formationManager = GameObject.Find("Managers").GetComponent<FormationManager>();
        GridRenderer = GetComponent<Renderer>();
    }

    public void OnFormation()
    {
        GridRenderer.material.color = Color.white;
    }
    public void NotOnFormation()
    {
        GridRenderer.material.color = Color.green;
        HeroOnGrid = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (HeroOnGrid == null)
        {
            HeroOnGrid = other.gameObject;
        }
        formationManager.GridCurrentlyOn = this.gameObject;
        if (formationManager.ClickedOnHero || formationManager.ClickedOnHeroCard)
        {
            if (GridRenderer.material.color != Color.white)
            {
                GridRenderer.material.color = Color.red;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (formationManager.ClickedOnHero || formationManager.ClickedOnHeroCard)
        {
            if (GridRenderer.material.color != Color.white)
            {
                GridRenderer.material.color = Color.green;
            }
            if (HeroOnGrid == formationManager.ClickedGO)
            {
                HeroOnGrid = null;
            }
        }
        formationManager.GridCurrentlyOn = null;
    }
}
