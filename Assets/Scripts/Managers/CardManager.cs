using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<GameObject> Hand;
    private Ray ray;
    private Camera mainCam;
    bool clickedOn;
    private LayerMask heroLayer;
    public GameObject AOEIndicatorPrefab;
    GameObject AOEIndicator;
    Card usingCard;
    SpellManager SM;

    // TODO : Placing cards and drawing them from the deck. Also deck mechanic.
    //DOING : Firing up a skill when its clicked.
    void Start()
    {
        mainCam = Camera.main;
        heroLayer = LayerMask.GetMask("HeroLayer");
        SM = GetComponent<SpellManager>();
    }

    void Update()
    {
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 1000, ~heroLayer);
            
        if (clickedOn)
        {
            if (Physics.Raycast(ray, out hitData, 1000 , ~heroLayer))
            {
                AOEIndicator.transform.position = hitData.point;
            }

        }
        if (Input.GetMouseButtonDown(0) && clickedOn)
        {
            SM.CastWithPosition(usingCard.ID, hitData.point);
            clickedOn = false;
            Debug.Log("Button Up");
            AOEIndicator.SetActive(false);
        }
    }
    public void useCard(GameObject usedCard)
    {
        usingCard = usedCard.GetComponent<Card>();
        Debug.Log("onClick Worked");
        if(AOEIndicator == null)
        {
            AOEIndicator = Instantiate(AOEIndicatorPrefab, Vector3.zero, Quaternion.identity);
            AOEIndicator.transform.localScale = Vector3.one * usingCard.SpellRange;
        }
        else if(!AOEIndicator.activeSelf)
        {
            AOEIndicator.SetActive(true);
            AOEIndicator.transform.position = usedCard.transform.position;
        }
        clickedOn = true;
    }
}
