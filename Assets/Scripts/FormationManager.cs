using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 27 Oct 2020

public class FormationManager : MonoBehaviour
{
    public GameObject gridArea1;
    public GameObject gridArea2;
    private Ray ray;
    public bool ClickedOnHero;
    public  GameObject HitGO;
    private GameObject gridGO;
    public Transform LastGridPos;
    private LayerMask heroLayer;
    public bool PlacingOnFullGrid;
    public GridController GC;

    void Awake()
    {
        gridArea1 = GameObject.Find("GridArea1");
        gridArea2 = GameObject.Find("GridArea2");
        heroLayer = LayerMask.GetMask("HeroLayer");
        Physics.IgnoreLayerCollision(9, 9, true);
    }

    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 10000000);        
        if (Input.GetMouseButtonDown(0) && hitData.transform.tag == "King1")
        {
            ClickedOnHero = true;
            HitGO = hitData.transform.gameObject;
        }

        if (Input.GetMouseButton(0) && ClickedOnHero)
        {
            if (Physics.Raycast(ray, out hitData, 1000 , ~heroLayer))
            {
                HitGO.transform.position = hitData.point;
            }
            
        }

        if (Input.GetMouseButtonUp(0) && ClickedOnHero)
        {
            SetPos(LastGridPos);
            ClickedOnHero = false;
            HitGO = null;
        }

    }

    public void SetPos(Transform posRef)
    {
        HitGO.transform.position = posRef.position;
    }

}
