using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Mert Karavural
//Date: 11.2021
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
    private BattlefieldManager BM;
    [HideInInspector] public float GuildEnergy;
    public float GuildEnergyRefreshPerSecond;
    public Slider GuildEnergyBar;
    [HideInInspector] public GameObject CardArea;
    public float MaxGuildEnergy;
    void Start()
    {
        mainCam = Camera.main;
        heroLayer = LayerMask.GetMask("HeroLayer");
        CardArea = GameObject.Find("CardArea");
        SM = GetComponent<SpellManager>();
        BM = GetComponent<BattlefieldManager>();
        GuildEnergyBar.maxValue = MaxGuildEnergy;
        StartCoroutine(RestoreEnergy());
        for (int i = 0; i < 4; i++)
        {
            Hand.Add(Deck[Deck.Count - 1]);
            GameObject tempCardGO = Instantiate(FindCard(Deck[Deck.Count - 1]), Vector3.zero, Quaternion.identity);
            tempCardGO.transform.SetParent(CardArea.transform);
            Deck.RemoveAt(Deck.Count - 1);
        }
        RealignCards();
    }
    void Update()
    {
        RealignCards();
        if (BM.GameStarted)
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData, 1000, ~heroLayer);
            GuildEnergyBar.value = GuildEnergy;
            if (clickedOn)
            {
                if (Physics.Raycast(ray, out hitData, 1000 , ~heroLayer))
                {
                    AOEIndicator.transform.position = hitData.point + (Vector3.up * 0.1f);
                }

            }
            if (Input.GetMouseButtonUp(0) && clickedOn)
            {
                SM.CastWithPosition(usingCard.SpellID, hitData.point + (Vector3.up * 0.1f));
                GuildEnergy -= SM.FindSpell(usingCard.SpellID).EnergyCost;
                clickedOn = false;
                AOEIndicator.SetActive(false);
                StartCoroutine(PullCardFromDeck());
            }
            if (Input.GetMouseButtonDown(1) && clickedOn)
            {
                clickedOn = false;
                AOEIndicator.SetActive(false);
            }
        }
    }
    
    public void UseCard(GameObject usedCard)
    {
        usingCardGO = usedCard;
        usingCard = usedCard.GetComponent<Card>();
        if (GuildEnergy >= SM.FindSpell(usingCard.SpellID).EnergyCost)
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
            GuildEnergy += GuildEnergyRefreshPerSecond / 100;
            if(GuildEnergy > MaxGuildEnergy)
            {
                GuildEnergy = MaxGuildEnergy;
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
        tempCardGO.transform.SetParent(CardArea.transform);
        Deck.RemoveAt(Deck.Count - 1);
        Deck.Add(usingCard.ID);
        RealignCards();
        Deck = FisherYatesCardDeckShuffle(Deck);
    }
    public static List<int> FisherYatesCardDeckShuffle(List<int> aList)
    {
        int myInt;
        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(Random.Range(0,100000) * (n - i));
            r = r % n;
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

    public GameObject PullUltimateSkillCard(GameObject hero)
    {
        GameObject tempCardGO = Instantiate(FindCard(hero.GetComponent<Hero>().UltimateSkillCardID), Vector3.zero, Quaternion.identity);
        tempCardGO.transform.SetParent(CardArea.transform);
        tempCardGO.GetComponent<Button>().onClick.AddListener(delegate { hero.GetComponent<HeroController>().CastUltimateSkill(tempCardGO); });
        RealignCards();
        return tempCardGO;
    }

    public IEnumerator DestroyCardRitual(GameObject cardToBeDestroyed)
    {
        Destroy(cardToBeDestroyed);
        yield return new WaitForSeconds(0.1f);
        RealignCards();
    }

}
