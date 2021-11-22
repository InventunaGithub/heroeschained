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
    public int MaxTeamCount;
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
    private GameObject inGamePanel;
    public GameObject ReadyButton;
    private GameObject createdHeroGO;
    private GameObject thumbnailArea;
    public bool OnTrash;
    private bool battleSequence;
    public GameObject thumbnailPrefab;
    public GameObject Trash;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("HeroLayer"), LayerMask.NameToLayer("HeroLayer"), true);
        mainCam = Camera.main;
        ReadyButton.SetActive(true);
        heroLayer = LayerMask.GetMask("HeroLayer");
        formationPanel = GameObject.Find("formationPanel");
        inGamePanel = GameObject.Find("inGamePanel");
        inGamePanel.SetActive(false);
        thumbnailArea = GameObject.Find("Content");
        Trash = GameObject.Find("Trash");
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
            StartCoroutine(FirstChangeGrid());
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
                        if(hero.ID == pair.Value) //this creates a bug where they all get the same card.
                        {
                            tempCardGO.GetComponent<HeroCard>().ConnectedGameObject = hero.gameObject;
                            hero.gameObject.GetComponent<HeroController>().ConnectedHeroCard = tempCardGO;
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

        thumbnailArea.GetComponent<RectTransform>().sizeDelta = new Vector2(1080 , (Mathf.Ceil( AllHeroes.Count / 4 )+1 )* 400);
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
                ClickedGO.transform.transform.DOMove(hitData.point, 0.001f);
            }
        }

        if (Input.GetMouseButtonUp(0) && ClickedOnHero)
        {
            if(OnTrash)
            {
                CurrentFormation.Remove(clickedHeroController.GridCurrentlyOn.ID);
                clickedHeroController.GridCurrentlyOn = null;
                HeroCard swappedOutHeroCard = clickedHeroController.ConnectedHeroCard.GetComponent<HeroCard>();
                swappedOutHeroCard.ChangeColor(Color.green);
                swappedOutHeroCard.OnTeam = false;
                battleFieldManager.Team1.Remove(clickedHeroController.MainHero);
                Destroy(clickedHeroController.MainHero.HeroObject);
                Trash.GetComponent<TrashAreaObserver>().destroyed();
            }
            else
            {
                if (GridCurrentlyOn != null)
                {
                    GridController tempGC = GridCurrentlyOn.GetComponent<GridController>();
                    if (tempGC.HeroOnGrid == ClickedGO)
                    {
                        //set hitHero's position to new position and change the dictionary.
                        ClickedGO.transform.DOMove(GridCurrentlyOn.transform.position, 0.1f);
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
                    ClickedGO.transform.DOMove(FormationArea.transform.GetChild(clickedHeroController.GridCurrentlyOn.ID).transform.position, 0.1f);
                }
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
            if (OnTrash)
            {
                Destroy(createdHeroGO);
                Trash.GetComponent<TrashAreaObserver>().destroyed();
            }
            else
            {
                if (GridCurrentlyOn != null)
                {
                    GridController tempGridCtrlr = GridCurrentlyOn.GetComponent<GridController>();
                    if (tempGridCtrlr.HeroOnGrid == createdHeroGO)
                    {
                        //this happens when you drag a hero card to a empty grid , it makes the new hero join the team .
                        if(battleFieldManager.Team1.Count< MaxTeamCount)
                        {
                            Hero createdHero = createdHeroGO.GetComponent<Hero>();
                            battleFieldManager.Team1.Add(createdHero);
                            ClickedHeroCardGO.GetComponent<HeroCard>().ConnectedGameObject = createdHeroGO;
                            createdHero.GetComponent<HeroController>().ConnectedHeroCard = ClickedHeroCardGO;
                            createdHeroGO.transform.DOMove(GridCurrentlyOn.transform.position, 0.1f);
                            LastGridPos = GridCurrentlyOn.transform.position;
                            CurrentFormation.Add(tempGridCtrlr.ID, createdHeroGO.GetComponent<Hero>().ID);
                            tempGridCtrlr.HeroOnGrid = createdHeroGO;
                            createdHeroGO.GetComponent<HeroController>().GridCurrentlyOn = tempGridCtrlr;
                            ClickedHeroCardGO.GetComponent<HeroCard>().OnTeam = true;
                            ClickedHeroCardGO.GetComponent<HeroCard>().ChangeColor(Color.red);
                        }
                        else
                        {
                            Debug.Log("maxteamcount reached");
                            Destroy(createdHeroGO);
                        }
                        
                    }
                    else
                    {
                        //this happens when you drag a card to a full grid , it replaces the grid with the hero you are holding.
                        HeroController swappedOutHeroController = tempGridCtrlr.HeroOnGrid.GetComponent<HeroController>();
                        swappedOutHeroController.GridCurrentlyOn = null;
                        HeroCard swappedOutHeroCard = swappedOutHeroController.ConnectedHeroCard.GetComponent<HeroCard>();
                        swappedOutHeroCard.ChangeColor(Color.green);
                        swappedOutHeroCard.OnTeam = false;
                        battleFieldManager.Team1.Remove(swappedOutHeroController.MainHero);
                        Destroy(swappedOutHeroController.MainHero.HeroObject);

                        Hero createdHero = createdHeroGO.GetComponent<Hero>();
                        battleFieldManager.Team1.Add(createdHero);
                        ClickedHeroCardGO.GetComponent<HeroCard>().ConnectedGameObject = createdHeroGO;
                        createdHero.GetComponent<HeroController>().ConnectedHeroCard = ClickedHeroCardGO;
                        createdHeroGO.transform.DOMove(GridCurrentlyOn.transform.position, 0.1f);
                        LastGridPos = GridCurrentlyOn.transform.position;
                        CurrentFormation[tempGridCtrlr.ID] = createdHeroGO.GetComponent<Hero>().ID;
                        tempGridCtrlr.HeroOnGrid = createdHeroGO;
                        createdHeroGO.GetComponent<HeroController>().GridCurrentlyOn = tempGridCtrlr;
                        ClickedHeroCardGO.GetComponent<HeroCard>().OnTeam = true;
                        ClickedHeroCardGO.GetComponent<HeroCard>().ChangeColor(Color.red);
                        //Swapping heroes out.
                        ClickedHeroCardGO = null;
                    }
                }
                else
                {
                    //this happens when you drag a hero card to a non grid area.( go to the first free spot on grids)
                    if (battleFieldManager.Team1.Count < MaxTeamCount)
                    {
                        int freeGridIndex = -1;
                        for (int i = 0; i < 9; i++)
                        {
                            if (!CurrentFormation.ContainsKey(i))
                            {
                                if (freeGridIndex == -1)
                                {
                                    freeGridIndex = i;
                                }
                            }
                        }
                        GameObject tempGridGO = FormationArea.transform.GetChild(freeGridIndex).gameObject;
                        GridController tempGridCtrlr = FormationArea.transform.GetChild(freeGridIndex).GetComponent<GridController>();
                        Hero createdHero = createdHeroGO.GetComponent<Hero>();
                        battleFieldManager.Team1.Add(createdHero);
                        ClickedHeroCardGO.GetComponent<HeroCard>().ConnectedGameObject = createdHeroGO;
                        createdHero.GetComponent<HeroController>().ConnectedHeroCard = ClickedHeroCardGO;
                        createdHeroGO.transform.DOMove(tempGridGO.transform.position, 0.1f);
                        LastGridPos = tempGridGO.transform.position;
                        CurrentFormation.Add(tempGridCtrlr.ID, createdHeroGO.GetComponent<Hero>().ID);
                        tempGridCtrlr.HeroOnGrid = createdHeroGO;
                        createdHeroGO.GetComponent<HeroController>().GridCurrentlyOn = tempGridCtrlr;
                        ClickedHeroCardGO.GetComponent<HeroCard>().OnTeam = true;
                        ClickedHeroCardGO.GetComponent<HeroCard>().ChangeColor(Color.red);
                        ClickedHeroCardGO = null;
                    }
                    else
                    {
                        Debug.Log("Maxteamcount reached");
                        Destroy(createdHeroGO);
                    }
                }
            }

            ChangeGrid();
            ClickedOnHeroCard = false; 
        }

    }
    public void RealignThumbnails()
    {
        //int pos = 0;
        //foreach (RectTransform child in thumbnailArea.transform)
        //{
        //    child.anchoredPosition = new Vector3(-680 + (pos * 200), 0, 0);
        //    pos += 1;
        //}
    }

    IEnumerator FirstChangeGrid()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < FormationArea.transform.childCount; i++)
        {
            GridController tempGridController = FormationArea.transform.GetChild(i).GetComponent<GridController>();
            if (CurrentFormation.ContainsKey(i))
            {
                tempGridController.OnFormation();
            }
            else
            {
                tempGridController.NotOnFormation();
            }
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
    public void StartBattleStartSequence()
    {
        StartCoroutine(BattleStartSequence());
    }
    IEnumerator BattleStartSequence()
    {
        battleSequence = true;
        Trash.SetActive(false);
        inGamePanel.SetActive(true);
        formationPanel.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
    //BM startgame
}
