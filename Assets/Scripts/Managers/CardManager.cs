using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int guildEnergy;
    public Slider GuildEnergyBar;

    // TODO : Placing cards and drawing them from the deck. Also deck mechanic.
    //DOING : Firing up a skill when its clicked.
    void Start()
    {
        mainCam = Camera.main;
        heroLayer = LayerMask.GetMask("HeroLayer");
        SM = GetComponent<SpellManager>();
        GuildEnergyBar.maxValue = 100;
        StartCoroutine(RestoreEnergy());
    }

    void Update()
    {
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 1000, ~heroLayer);
        GuildEnergyBar.value = guildEnergy;
        if (clickedOn)
        {
            if (Physics.Raycast(ray, out hitData, 1000 , ~heroLayer))
            {
                AOEIndicator.transform.position = hitData.point + (Vector3.up * 0.1f);
            }

        }
        if (Input.GetMouseButtonDown(0) && clickedOn)
        {
            SM.CastWithPosition(usingCard.SpellID, hitData.point + (Vector3.up * 0.1f));
            clickedOn = false;
            Debug.Log("Button Up");
            AOEIndicator.SetActive(false);
        }
    }
    
    public void useCard(GameObject usedCard)
    {
        usingCard = usedCard.GetComponent<Card>();
        if (guildEnergy >= SM.FindSpell(usingCard.SpellID).EnergyCost)
        {
            guildEnergy -= SM.FindSpell(usingCard.SpellID).EnergyCost;
            Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
            if (AOEIndicator == null)
            {
                AOEIndicator = Instantiate(AOEIndicatorPrefab, Vector3.zero, spawnRotation);

            }
            else if (!AOEIndicator.activeSelf)
            {
                AOEIndicator.SetActive(true);
                AOEIndicator.transform.position = usedCard.transform.position;
            }
            AOEIndicator.transform.localScale = Vector3.one * 0.4f * SM.FindSpell(usingCard.SpellID).AOERange;
            clickedOn = true;
        }
        else
        {
            Debug.Log("Not Enough Energy");
        }
    }

    IEnumerator RestoreEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            guildEnergy += 1;
            if(guildEnergy > 100)
            {
                guildEnergy = 100;
            }
        }

    }
}
