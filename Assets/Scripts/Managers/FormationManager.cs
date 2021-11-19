using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class FormationManager : MonoBehaviour
{
    BattlefieldManager battleFieldManager;
    public Dictionary<int, int> CurrentFormation = new Dictionary<int, int>();
    public List<int> CurrentTeam;
    public List<int> CurrentTeamGridPossitions;
    public List<int> AllHeroes;
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
    public bool ClickedOnHeroCard = false;
    [HideInInspector] public bool PlacingOnFullGrid;
    public GameObject GridCurrentlyOn;
    private GameObject formationPanel;
    public GameObject ReadyButton;
    private GameObject createdHeroGO;
    private GameObject thumbnailArea;
    public GameObject thumbnailPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("HeroLayer"), LayerMask.NameToLayer("HeroLayer"), true);
        mainCam = Camera.main;
        ReadyButton.SetActive(true);
        heroLayer = LayerMask.GetMask("HeroLayer");
        formationPanel = GameObject.Find("formationPanel");
        thumbnailArea = GameObject.Find("HeroThumbnails");
        formationPanel.SetActive(true);
        battleFieldManager = GameObject.Find("Managers").GetComponent<BattlefieldManager>(); 
        FormationArea = GameObject.Find("FormationArea");
        
        for (int i = 0; i < CurrentTeam.Count; i++)
        {
            CurrentFormation.Add(CurrentTeamGridPossitions[i], CurrentTeam[i]);
        }

        if (CurrentFormation.Count > 0)
        {
            battleFieldManager.SetUpFormation(CurrentFormation, FormationArea, "Team1", battleFieldManager.Team1);
            ChangeGrid();
        }

        for (int i = 0; i < AllHeroes.Count; i++)
        {
            bool onTeam = false;
            
            foreach (var pair in CurrentFormation)
            {
                if(pair.Value == AllHeroes[i])
                {
                    GameObject tempCardGO = Instantiate(thumbnailPrefab, Vector3.zero, Quaternion.identity, thumbnailArea.transform);
                    HeroCard tempCard = tempCardGO.GetComponent<HeroCard>();
                    tempCard.HeroScriptableObjectID = pair.Value;
                    tempCard.OnTeam = true;
                    tempCard.ChangeColor(Color.red);
                    onTeam = true;
                    foreach (Hero hero in battleFieldManager.Team1) 
                    {
                        if(hero.ID == pair.Value)
                        {
                            tempCardGO.GetComponent<HeroCard>().ConnectedGameObject = hero.gameObject;
                        }
                    }
                }
            }
            if(!onTeam)
            {
                GameObject tempCardGO = Instantiate(thumbnailPrefab, Vector3.zero, Quaternion.identity, thumbnailArea.transform);
                tempCardGO.GetComponent<HeroCard>().HeroScriptableObjectID = AllHeroes[i];
                tempCardGO.GetComponent<HeroCard>().OnTeam = false;
                tempCardGO.GetComponent<HeroCard>().ChangeColor(Color.green);
            }

        }
        RealignThumbnails();

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
            GridCurrentlyOn = FormationArea.transform.GetChild(clickedHeroController.GridCurrentlyOn.ID).gameObject;
            //set last known grid position to the position we are lifting the hero up.
            LastGridPos = ClickedGO.transform.position;
        }
        if (Input.GetMouseButton(0) && ClickedOnHero)
        {
            //Change the GameObjects position that we hit with the ray.
            if (Physics.Raycast(ray, out hitData, 1000, ~heroLayer))
            {
                ClickedGO.transform.transform.DOMove(hitData.point, 0.1f);
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
                else //Swapping
                {
                    GameObject swappedGO = tempGC.HeroOnGrid;
                    //Swap positions of heroes
                    tempGC.HeroOnGrid.transform.DOMove(FormationArea.transform.GetChild(clickedHeroController.GridCurrentlyOn.ID).transform.position, 0.1f);
                    ClickedGO.transform.DOMove(GridCurrentlyOn.transform.position, 0.1f);
                    //Swap Formation Values
                    CurrentFormation[tempGC.ID] = clickedHero.ID;
                    CurrentFormation[clickedHeroController.GridCurrentlyOn.ID] = tempGC.HeroOnGrid.GetComponent<Hero>().ID;
                    //Swap Grids HeroOnGrid Values
                    tempGC.HeroOnGrid = ClickedGO;
                    FormationArea.transform.GetChild(clickedHeroController.GridCurrentlyOn.ID).GetComponent<GridController>().HeroOnGrid = swappedGO;
                    //Swapping Grid Currently On
                    swappedGO.GetComponent<HeroController>().GridCurrentlyOn = clickedHeroController.GridCurrentlyOn;
                    clickedHeroController.GridCurrentlyOn = tempGC;
                    LastGridPos = GridCurrentlyOn.transform.position;
                }

            }
            else
            {
                ClickedGO.transform.DOMove(FormationArea.transform.GetChild(clickedHeroController.GridCurrentlyOn.ID).transform.position , 0.1f);
            }
            ClickedOnHero = false;
            ClickedGO = null;
            ChangeGrid();
        }

        if (Input.GetMouseButton(0) && ClickedOnHeroCard)
        {
            if (Physics.Raycast(ray, out hitData, 1000, ~heroLayer))
            {
                createdHeroGO.transform.DOMove(hitData.point, 0.01f);
            }
        }

        if (Input.GetMouseButtonUp(0) && ClickedOnHeroCard)
        {
           if(GridCurrentlyOn != null)
            {
                //Bug on HeroonGrid thing. When placing new heroes on the board , it causes the wrong tiles have wrong hero on grid.
                GridController tempGC = GridCurrentlyOn.GetComponent<GridController>();
                if (tempGC.HeroOnGrid == createdHeroGO)
                {
                    Hero createdHero = createdHeroGO.GetComponent<Hero>();
                    battleFieldManager.Team1.Add(createdHero);
                    ClickedHeroCardGO.GetComponent<HeroCard>().ConnectedGameObject = ClickedGO;
                    createdHeroGO.transform.DOMove(GridCurrentlyOn.transform.position, 0.1f);
                    LastGridPos = GridCurrentlyOn.transform.position;
                    CurrentFormation.Add(tempGC.ID, createdHeroGO.GetComponent<Hero>().ID);
                    tempGC.HeroOnGrid = createdHeroGO;
                    createdHeroGO.GetComponent<HeroController>().GridCurrentlyOn = tempGC;
                    ClickedHeroCardGO.GetComponent<HeroCard>().OnTeam = true;
                    ClickedHeroCardGO.GetComponent<HeroCard>().ChangeColor(Color.red);
                }
                else
                {
                    //Swapping heroes out.
                    Destroy(createdHeroGO);
                    ClickedHeroCardGO = null;
                }
            }
            else
            {
                Destroy(createdHeroGO);
                ClickedHeroCardGO = null;
            }
            ChangeGrid();
            ClickedOnHeroCard = false; 
        }

    }
    public void RealignThumbnails()
    {
        int pos = 0;
        foreach (RectTransform child in thumbnailArea.transform)
        {
            child.anchoredPosition = new Vector3(-680 + (pos * 200), 0, 0);
            pos += 1;
        }
    }
    public void ChangeGrid()
    {
        for (int i = 0; i < FormationArea.transform.childCount; i++)
        {
            GridController tempGridController = FormationArea.transform.GetChild(i).GetComponent<GridController>();
            if (CurrentFormation.ContainsKey(i))
            {
                tempGridController.OnFormation();
                foreach (Hero hero in battleFieldManager.Team1)
                {
                    if(hero.ID == CurrentFormation[i])
                    {
                        tempGridController.HeroOnGrid = hero.HeroObject;
                    }
                }
            }
            else
            {
                tempGridController.NotOnFormation();
            }
        }
    }
    public void ClickedOnCard(HeroCard clickedCard)
    {
        if(!clickedCard.OnTeam)
        {
            GridCurrentlyOn = null;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData, 1000);
            ClickedOnHeroCard = true;
            ClickedHeroCardGO = clickedCard.gameObject;
            createdHeroGO = Instantiate(battleFieldManager.HeroObjectPrefab, hitData.point + Vector3.up, Quaternion.identity, battleFieldManager.Characters.transform);
            createdHeroGO.GetComponent<Hero>().HeroObject = createdHeroGO;
            createdHeroGO.transform.tag = "Team1";
            GameObject createdHeroSkin = Instantiate(battleFieldManager.FindHero(ClickedHeroCardGO.GetComponent<HeroCard>().HeroScriptableObjectID).HeroSkin, createdHeroGO.transform.position, Quaternion.identity, createdHeroGO.transform);
        }
    }
    //BM startgame
}
