using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 3.11.2021
public class BattlefieldManager : MonoBehaviour
{
    public Dictionary<int, int> Team1Formation = new Dictionary<int, int>();
    public Dictionary<int, int> Team2Formation = new Dictionary<int, int>();
    public List<int> Team1IDs;
    public List<int> Team2IDs;
    public List<int> Team1GridPositions;
    public List<int> Team2GridPositions;
    [HideInInspector] public List<Hero> Team1;
    [HideInInspector] public List<Hero> Team2;
    public List<HeroSO> HeroesSO;
    private GameObject characters;
    public GameObject HeroObjectPrefab;
    private GameObject gridArea1;
    private GameObject gridArea2;
    private GameObject inGamePanel;
    private GameObject formationPanel;
    public GameObject ReadyButton;
    [HideInInspector] public GameObject HitGO;
    private Ray ray;
    [HideInInspector] public Vector3 LastGridPos;
    private LayerMask heroLayer;
    [HideInInspector] public GridController GridCtrlr;
    [HideInInspector] public bool ClickedOnHero;
    [HideInInspector] public bool PlacingOnFullGrid;
    public bool GameStarted = false;
    private bool lockOn = false;
    private Camera mainCam;
    public GameObject GridCurrentlyOn;
    private Hero hitHero;
    private HeroController hitHeroController;
    void Awake()
    {
        
        characters = GameObject.Find("Characters");
        gridArea1 = GameObject.Find("GridArea1");
        gridArea2 = GameObject.Find("GridArea2");
        heroLayer = LayerMask.GetMask("HeroLayer");
        inGamePanel = GameObject.Find("inGamePanel");
        formationPanel = GameObject.Find("formationPanel");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("HeroLayer"), LayerMask.NameToLayer("HeroLayer"), true);
        int gridArea1Children = gridArea1.transform.childCount;
        for (int i = 0; i < gridArea1Children; ++i)
        {
            gridArea1.transform.GetChild(i).GetComponent<GridController>().ID = i;
        }
        for (int i = 0; i < Team1IDs.Count; i++)
        {
            Team1Formation.Add(Team1GridPositions[i], Team1IDs[i]);
        }
        for (int i = 0; i < Team2IDs.Count; i++)
        {
            Team2Formation.Add(Team2GridPositions[i], Team2IDs[i]);
        }

        SetUpFormation(Team1Formation, gridArea1, "Team1", Team1);
        inGamePanel.SetActive(false);
        formationPanel.SetActive(true);
        mainCam = Camera.main;
        
    }

    void Update()
    {
        if(GameStarted && !lockOn)
        {
            gridArea1.SetActive(false);
            gridArea2.SetActive(false);
            ReadyButton.SetActive(false);
            inGamePanel.SetActive(true);
            formationPanel.SetActive(false);
            lockOn = true;
            SetUpFormation(Team2Formation, gridArea2, "Team2", Team2);
        }
        if (!lockOn)
        {
            //Shooting A Raycast from mouse position to scene , if it hits a GameObject that tagged Team1 , start the clicking procedure.
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData, 1000);
            if (Input.GetMouseButtonDown(0) && hitData.transform.tag == "Team1")
            {
                //Click Lock , also get who we are hitting with the ray
                ClickedOnHero = true;
                HitGO = hitData.transform.gameObject;
                hitHero = HitGO.GetComponent<Hero>();
                hitHeroController = HitGO.GetComponent<HeroController>();
                //set last known grid position to the position we are lifting the hero up.
                LastGridPos = HitGO.transform.position;
            }
            if (Input.GetMouseButton(0) && ClickedOnHero)
            {
                //Change the GameObjects position that we hit with the ray.
                if (Physics.Raycast(ray, out hitData, 1000, ~heroLayer))
                {
                    HitGO.transform.position = hitData.point;
                }
            }

            if (Input.GetMouseButtonUp(0) && ClickedOnHero)
            {
                if(GridCurrentlyOn != null)
                {
                    GridController tempGC = GridCurrentlyOn.GetComponent<GridController>();
                    if (tempGC.HeroOnGrid == HitGO)
                    {
                        //set hitHero's position to new position and change the dictionary.
                        SetPos(GridCurrentlyOn.transform.position);
                        LastGridPos = GridCurrentlyOn.transform.position;
                        Team1Formation.Remove(hitHeroController.GridCurrentlyOn.ID);
                        HitGO.GetComponent<HeroController>().GridCurrentlyOn.ID = tempGC.ID;
                        Team1Formation.Add(tempGC.ID, hitHero.ID);
                    }
                    
                }
                else
                {
                    SetPos(gridArea1.transform.GetChild(HitGO.GetComponent<HeroController>().GridCurrentlyOn.ID).transform.position);
                }
                ClickedOnHero = false;
                HitGO = null;
            }
        }

    }
    public void SetPos(Vector3 posRef)
    {
        HitGO.transform.DOMove(posRef , 0.1f);
    }

    public void StartGame(bool game)
    {
        GameStarted = game;
    }

    public void SetUpFormation(Dictionary<int, int> formation , GameObject gridArea , string teamName , List<Hero> team)
    {
        foreach (var heroPair in formation)
        {
            //Caching key and value
            int gridID = heroPair.Key;
            int heroID = heroPair.Value;
            //Instantiatin the heroObject , placing them in the correct grid
            //writing what grid hero is on , giving hero the team tag , adding hero to the team.
            GameObject heroGO = Instantiate(HeroObjectPrefab, gridArea.transform.GetChild(gridID).transform.position, gridArea.transform.GetChild(gridID).transform.rotation, characters.transform);
            heroGO.GetComponent<HeroController>().GridCurrentlyOn = gridArea.transform.GetChild(gridID).transform.GetComponent<GridController>();
            heroGO.GetComponent<Hero>().HeroObject = heroGO;
            heroGO.transform.tag = teamName;
            GameObject heroSkin = Instantiate(FindHero(heroID).HeroSkin, heroGO.transform.position, Quaternion.identity);
            heroSkin.transform.SetParent(heroGO.transform);
            heroGO.GetComponent<Hero>().Init(FindHero(heroID));
            team.Add(heroGO.GetComponent<Hero>());
        }  
    }
    public HeroSO FindHero(int heroID)
    {
        //Finds Hero in the Heroes list with the given ID
        foreach(HeroSO hero in HeroesSO)
        {
            if(hero.HeroID == heroID)
            {
                return hero;
            }
        }

        return null;
    }
}
