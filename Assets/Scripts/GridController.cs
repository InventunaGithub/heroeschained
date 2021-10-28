using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 27 Oct 2020

public class GridController : MonoBehaviour
{
    BattlefieldManager BM;
    Renderer gridRenderer;
    public GameObject HeroOnGrid;
    bool gridFull;
    public bool Dropped;
    void Start()
    {
        BM = GameObject.Find("Managers").GetComponent<BattlefieldManager>();
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
        if(!gridFull || HeroOnGrid == BM.HitGO)
        {
            BM.LastGridPos = this.transform;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (!gridFull || HeroOnGrid == BM.HitGO)
        {
            if (BM.LastGridPos != this.transform)
            {
                BM.LastGridPos = this.transform;
            }
            gridRenderer.material.color = Color.green;
            HeroOnGrid = null;
            gridFull = false;
        }
        
    }
}
