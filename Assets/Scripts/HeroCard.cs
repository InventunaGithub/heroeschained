using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour
{
    public int HeroScriptableObjectID;
    public GameObject ConnectedGameObject;
    public bool OnTeam = false;
    public Image image;
    private Color givenColor = Color.green;
    private void Update()
    {
        image.color = givenColor;
    }

    public void ChangeColor(Color givenColor)
    {
        this.givenColor = givenColor;
    }
}
