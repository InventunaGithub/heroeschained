using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FormationManager : MonoBehaviour
{
    BattlefieldManager battleFieldManager;
    public Dictionary<int, int> CurrentFormation = new Dictionary<int, int>();
    public List<int> CurrentTeam;
    public List<int> CurrentTeamGridPossitions;
    public List<int> Keys;
    public List<int> Values;
    private Ray ray;
    private LayerMask heroLayer;
    private Camera mainCam;
    private Hero clickedHero;
    private HeroController clickedHeroController;
    public GameObject FormationArea;
    [HideInInspector] public GameObject ClickedGO;
    [HideInInspector] public GameObject ClickedHeroCardGO;
    [HideInInspector] public Vector3 LastGridPos;
    [HideInInspector] public GridController GridCtrlr;
    [HideInInspector] public bool ClickedOnHero;
    public bool ClickedOnHeroCard;
    [HideInInspector] public bool PlacingOnFullGrid;
    public GameObject GridCurrentlyOn;
    private GameObject formationPanel;
    private GameObject inGamePanel;
    public GameObject ReadyButton;
    private GameObject createdHeroGO;
    private GameObject createdHeroSkin;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("HeroLayer"), LayerMask.NameToLayer("HeroLayer"), true);
        mainCam = Camera.main;
        ReadyButton.SetActive(true);
        heroLayer = LayerMask.GetMask("HeroLayer");
        formationPanel = GameObject.Find("formationPanel");
        formationPanel.SetActive(true);
        inGamePanel = GameObject.Find("inGamePanel");
        battleFieldManager = GameObject.Find("Managers").GetComponent<BattlefieldManager>(); 
        FormationArea = GameObject.Find("FormationArea");
        for (int i = 0; i < CurrentTeam.Count; i++)
        {
            CurrentFormation.Add(CurrentTeamGridPossitions[i], CurrentTeam[i]);
        }
        battleFieldManager.SetUpFormation(CurrentFormation, FormationArea, "Team1", battleFieldManager.Team1);
        ChangeGrid();
        //battleFieldManager.BattlefieldInitialize();
    }

    void Update()
    {
        //Shooting A Raycast from mouse position to scene , if it hits a GameObject that tagged Team1 , start the clicking procedure.
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 1000);
        if (Input.GetMouseButtonDown(0) && hitData.transform.tag == "Team1")
        {
            //Click Lock , also get who we are hitting with the ray
            ClickedOnHero = true;
            ClickedGO = hitData.transform.gameObject;
            clickedHero = ClickedGO.GetComponent<Hero>();
            clickedHeroController = ClickedGO.GetComponent<HeroController>();
            //set last known grid position to the position we are lifting the hero up.
            LastGridPos = ClickedGO.transform.position;
        }
        if (Input.GetMouseButton(0) && ClickedOnHero)
        {
            //Change the GameObjects position that we hit with the ray.
            if (Physics.Raycast(ray, out hitData, 1000, ~heroLayer))
            {
                ClickedGO.transform.position = hitData.point;
            }
        }

        if (Input.GetMouseButtonUp(0) && ClickedOnHero)
        {
            if (GridCurrentlyOn != null)
            {
                GridController tempGC = GridCurrentlyOn.GetComponent<GridController>();
                if (tempGC.HeroOnGrid == ClickedGO)
                {
                    //set hitHero's position to new position and change the dictionary.
                    ClickedGO.transform.DOMove(GridCurrentlyOn.transform.position , 0.1f);
                    LastGridPos = GridCurrentlyOn.transform.position;
                    CurrentFormation.Remove(clickedHeroController.GridCurrentlyOn.ID);
                    CurrentFormation.Add(tempGC.ID, clickedHero.ID);
                    ClickedGO.GetComponent<HeroController>().GridCurrentlyOn = tempGC;

                }

            }
            else
            {
                ClickedGO.transform.DOMove(FormationArea.transform.GetChild(ClickedGO.GetComponent<HeroController>().GridCurrentlyOn.ID).transform.position , 0.1f);
            }
            ClickedOnHero = false;
            ClickedGO = null;
            ChangeGrid();
        }

        if (Input.GetMouseButtonDown(0) && hitData.transform.tag == "HeroCard")
        {
            
        }
        if (Input.GetMouseButton(0) && ClickedOnHeroCard)
        {
            
        }

        if (Input.GetMouseButtonUp(0) && ClickedOnHeroCard)
        {
           
        }

    }

    public void ChangeGrid()
    {
        for (int i = 0; i < FormationArea.transform.childCount; i++)
        {
            if (CurrentFormation.ContainsKey(i))
            {
                FormationArea.transform.GetChild(i).GetComponent<GridController>().OnFormation();
            }
            else
            {
                FormationArea.transform.GetChild(i).GetComponent<GridController>().NotOnFormation();
            }
        }
    }
    public void ClickedOnCard(HeroCard clickedCard)
    {
        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 1000);
        ClickedOnHeroCard = true;
        ClickedHeroCardGO = hitData.transform.gameObject;
        ClickedHeroCardGO.SetActive(false);
        createdHeroGO = Instantiate(battleFieldManager.HeroObjectPrefab, hitData.transform.position, Quaternion.identity, battleFieldManager.Characters.transform);
        createdHeroGO.GetComponent<Hero>().HeroObject = createdHeroGO;
        createdHeroGO.transform.tag = "Team1";
        createdHeroSkin = Instantiate(battleFieldManager.FindHero(ClickedHeroCardGO.GetComponent<HeroCard>().HeroScriptableObjectID).HeroSkin, createdHeroSkin.transform.position, Quaternion.identity);
        createdHeroSkin.transform.SetParent(createdHeroGO.transform);

    }
    //BM startgame
}
