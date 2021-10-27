using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mert Karavural
//Date: 27 Oct 2020

public class GridController : MonoBehaviour
{
    FormationManager fm;
    Renderer gridRenderer;
    bool triggered;
    void Start()
    {
        fm = GameObject.Find("Managers").GetComponent<FormationManager>();
        gridRenderer = GetComponent<Renderer>();
    }
    void OnTriggerStay(Collider other)
    {
        gridRenderer.material.color = Color.red;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            fm.LastGridPos = this.transform;
            triggered = true;
        }
        else
        {
            triggered = false;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (!triggered)
        {
            fm.LastGridPos = this.transform;
            triggered = true;
        }
        else
        {
            triggered = false;
        }

        fm.LastGridPos = this.transform;
        gridRenderer.material.color = Color.green;
    }
}
