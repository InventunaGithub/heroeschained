using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 27 Oct 2020

public class GridController : MonoBehaviour
{
    [HideInInspector] public Renderer GridRenderer;
    BattlefieldManager BM;
    public GameObject HeroOnGrid;
    public int ID;
    void Start()
    {
        BM = GameObject.Find("Managers").GetComponent<BattlefieldManager>();
        GridRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        BM.GridCurrentlyOn = this.gameObject;
        if(HeroOnGrid == null)
        {
            HeroOnGrid = other.gameObject;
        }
        GridRenderer.material.color = Color.red;
    }
    void OnTriggerExit(Collider other)
    {
        if(HeroOnGrid == BM.HitGO)
        {
            HeroOnGrid = null;
            GridRenderer.material.color = Color.green;
        }
        BM.GridCurrentlyOn = null;
    }
}
