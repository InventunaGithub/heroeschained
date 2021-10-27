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
    private bool clickedOnHero;
    private GameObject hitGO;
    private GameObject gridGO;
    public Transform LastGridPos;
    private LayerMask heroLayer;

    void Awake()
    {
        gridArea1 = GameObject.Find("GridArea1");
        gridArea2 = GameObject.Find("GridArea2");
        heroLayer = LayerMask.GetMask("HeroLayer");
    }

    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 10000000);        
        if (Input.GetMouseButtonDown(0) && hitData.transform.tag == "King1")
        {
            clickedOnHero = true;
            hitGO = hitData.transform.gameObject;
            hitGO.GetComponent<CapsuleCollider>().enabled = true;
        }

        if (Input.GetMouseButton(0) && clickedOnHero)
        {
            if (Physics.Raycast(ray, out hitData, 1000 , ~heroLayer))
            {
                hitGO.transform.position = hitData.point;
            }
            
        }

        if (Input.GetMouseButtonUp(0) && clickedOnHero)
        {
            SetPos(LastGridPos);
            clickedOnHero = false;
            hitGO = null;
            hitGO.GetComponent<CapsuleCollider>().enabled = false;
        }

    }

    public void SetPos(Transform posRef)
    {
        hitGO.transform.position = posRef.position;
    }

}
