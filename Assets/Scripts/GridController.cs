using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 27 Oct 2020

public class GridController : MonoBehaviour
{
    FormationManager FM;
    Renderer gridRenderer;
    public GameObject HeroOnGrid;
    bool gridFull;
    public bool Dropped;
    void Start()
    {
        FM = GameObject.Find("Managers").GetComponent<FormationManager>();
        gridRenderer = GetComponent<Renderer>();
    }
    void OnTriggerStay(Collider other)
    {
        gridRenderer.material.color = Color.red;
    }
    private void OnTriggerEnter(Collider other)
    {   
        if(HeroOnGrid == null)
        {
            HeroOnGrid = other.gameObject;
            gridFull = true;
        }
        if(!gridFull || HeroOnGrid == FM.HitGO)
        {
            FM.LastGridPos = this.transform;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (!gridFull || HeroOnGrid == FM.HitGO)
        {
            if (FM.LastGridPos != this.transform)
            {
                FM.LastGridPos = this.transform;
            }
            gridRenderer.material.color = Color.green;
            HeroOnGrid = null;
            gridFull = false;
        }
        
    }
}
