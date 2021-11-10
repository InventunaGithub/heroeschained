using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Mert Karavural
//Date: 8.11.2021
public class CardManager : MonoBehaviour
{
    public List<int> Hand;
    public List<int> Deck;
    public List<GameObject> Cards;
    private Ray ray;
    private Camera mainCam;
    private bool clickedOn;
    private LayerMask heroLayer;
    public GameObject AOEIndicatorPrefab;
    private GameObject AOEIndicator;
    private Card usingCard;
    private GameObject usingCardGO;
    private SpellManager SM;
    private float guildEnergy;
    public float GuildEnergyRefreshPerSecond;
    public Slider GuildEnergyBar;
    public GameObject CardArea;
    public float maxGuildEnergy;

    //TODO : Placing cards and drawing them from the deck. Also deck mechanic.
    //DOING : Firing up a skill when its clicked.
    void Start()
    {
        mainCam = Camera.main;
        heroLayer = LayerMask.GetMask("HeroLayer");
        SM = GetComponent<SpellManager>();
        GuildEnergyBar.maxValue = maxGuildEnergy;
        StartCoroutine(RestoreEnergy());
        for (int i = 0; i < 4; i++)
        {
            Hand.Add(Deck[Deck.Count - 1]);
            GameObject tempCardGO = Instantiate(FindCard(Deck[Deck.Count - 1]), Vector3.zero, Quaternion.identity);
            tempCardGO.transform.SetParent(GameObject.Find("CardArea").transform);
            Deck.RemoveAt(Deck.Count - 1);
        }
        RealignCards();
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
            guildEnergy -= SM.FindSpell(usingCard.SpellID).EnergyCost;
            clickedOn = false;
            Debug.Log("Activated Card");
            AOEIndicator.SetActive(false);
            //UI Change Here ! used skill
            StartCoroutine(PullCardFromDeck());
        }
        if (Input.GetMouseButtonDown(1) && clickedOn)
        {
            clickedOn = false;
            AOEIndicator.SetActive(false);
            Debug.Log("Deactivated Card");
        }
    }
    
    public void UseCard(GameObject usedCard)
    {
        usingCardGO = usedCard;
        usingCard = usedCard.GetComponent<Card>();
        if (guildEnergy >= SM.FindSpell(usingCard.SpellID).EnergyCost)
        {
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
            yield return new WaitForSeconds(0.01f);
            guildEnergy += GuildEnergyRefreshPerSecond / 100;
            if(guildEnergy > maxGuildEnergy)
            {
                guildEnergy = maxGuildEnergy;
            }
        }

    }

    public GameObject FindCard(int ID)
    {
        foreach (GameObject cardGO in Cards)
        {
            if(cardGO.GetComponent<Card>().ID == ID)
            {
                return cardGO;
            }
        }

        return null;
    }

    public IEnumerator PullCardFromDeck()
    {
        Destroy(usingCardGO);
        Hand.Remove(usingCard.ID);
        yield return new WaitForEndOfFrame();
        Hand.Add(Deck[Deck.Count-1]);
        GameObject tempCardGO = Instantiate(FindCard(Deck[Deck.Count - 1]) , Vector3.zero , Quaternion.identity);
        tempCardGO.transform.SetParent(GameObject.Find("CardArea").transform);
        RealignCards();
        Deck.RemoveAt(Deck.Count - 1);
        Deck.Add(usingCard.ID);
        Deck = Fisher_Yates_CardDeck_Shuffle(Deck);
    }
    public static List<int> Fisher_Yates_CardDeck_Shuffle(List<int> aList)
    {

        System.Random _random = new System.Random();

        int myInt;

        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(_random.NextDouble() * (n - i));
            myInt = aList[r];
            aList[r] = aList[i];
            aList[i] = myInt;
        }

        return aList;
    }
    public void RealignCards()
    {
        int pos = 0;
        foreach (RectTransform child in CardArea.transform)
        {
            child.anchoredPosition = new Vector3(-680 + (pos * 200), 0 , 0);
            pos += 1;
        }
    }
}
