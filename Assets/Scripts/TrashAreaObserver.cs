using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashAreaObserver : MonoBehaviour
{
    // Start is called before the first frame update
    FormationManager formationManager;
    [HideInInspector] public Renderer GridRenderer;
    GameObject hero;
    void Awake()
    {
        formationManager = GameObject.Find("Managers").GetComponent<FormationManager>();
        GridRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        formationManager.OnTrash = true;
        formationManager.Trash = this.gameObject;
        GridRenderer.material.color = Color.red;
        hero = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        formationManager.OnTrash = false;
        GridRenderer.material.color = Color.cyan;
    }
    public void destroyed()
    {
        formationManager.OnTrash = false;
        GridRenderer.material.color = Color.cyan;
    } 
}
