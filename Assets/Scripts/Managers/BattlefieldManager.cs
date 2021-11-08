using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public List<Hero> Team1;
    public List<Hero> Team2;
    public List<HeroSO> Team1SO;
    public List<HeroSO> Team2SO;
    private GameObject characters;
    public GameObject HeroPrefab;
    private GameObject gridArea1;
    private GameObject gridArea2;
    public GameObject ReadyButton;
    public GameObject HitGO;
    private Ray ray;
    public Transform LastGridPos;
    private LayerMask heroLayer;
    public GridController GC;
    public bool ClickedOnHero;
    public bool PlacingOnFullGrid;
    public bool GameStarted = false;
    private Camera mainCam;
    void Awake()
    {
        characters = GameObject.Find("Characters");
        gridArea1 = GameObject.Find("GridArea1");
        gridArea2 = GameObject.Find("GridArea2");
        heroLayer = LayerMask.GetMask("HeroLayer");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("HeroLayer"), LayerMask.NameToLayer("HeroLayer"), true);
        int index = 0;
        foreach (var Hero in Team1SO)
        {
            GameObject heroGO = Instantiate(HeroPrefab, gridArea1.transform.GetChild(index).transform.position , gridArea1.transform.GetChild(index).transform.rotation);
            heroGO.transform.SetParent(characters.transform);
            heroGO.transform.tag = "Team1";
            GameObject heroSkin = Instantiate(Hero.HeroSkin , heroGO.transform.position, Quaternion.identity);
            heroSkin.transform.SetParent(heroGO.transform);
            heroGO.GetComponent<Hero>().Init(Hero);
            Team1.Add(heroGO.GetComponent<Hero>());
            index += 1;
        }
        index = 0;
        foreach (var Hero in Team2SO)
        {
            GameObject heroGO = Instantiate(HeroPrefab, gridArea2.transform.GetChild(index).transform.position, gridArea1.transform.GetChild(index).transform.rotation);
            heroGO.transform.SetParent(characters.transform);
            heroGO.transform.tag = "Team2";
            GameObject heroSkin = Instantiate(Hero.HeroSkin, heroGO.transform.position, Quaternion.identity);
            heroSkin.transform.SetParent(heroGO.transform);
            heroGO.GetComponent<Hero>().Init(Hero);
            Team2.Add(heroGO.GetComponent<Hero>());
            index += 1;
        }

        mainCam = Camera.main;
        
    }

    void Update()
    {
        if(GameStarted)
        {
            gridArea1.SetActive(false);
            gridArea2.SetActive(false);
            ReadyButton.SetActive(false);
            
        }
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 10000000);
        if (Input.GetMouseButtonDown(0) && hitData.transform.tag == "Team1")
        {
            ClickedOnHero = true;
            HitGO = hitData.transform.gameObject;
        }

        if (Input.GetMouseButton(0) && ClickedOnHero)
        {
            if (Physics.Raycast(ray, out hitData, 1000, ~heroLayer))
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

    public void StartGame(bool game)
    {
        GameStarted = game;
    }
}
