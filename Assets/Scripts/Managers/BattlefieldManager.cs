using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 3.11.2021
public class BattlefieldManager : MonoBehaviour
{
    public List<Hero> Team1;//Hero Team
    public List<Hero> Team2;
    public Dictionary<int, int> EnemyTeam1Formation = new Dictionary<int, int>();
    public Dictionary<int, int> EnemyTeam2Formation = new Dictionary<int, int>();
    public List<Dictionary<int, int>> EnemyTeamFormations = new List<Dictionary<int, int>>();
    public List<HeroSO> HeroesSO;
    public GameObject Characters;
    public GameObject HeroObjectPrefab;
    public List<int> EnemyTeamIDs;
    public List<int> EnemyTeamGridPositions;
    public List<GameObject> Team1GridAreas;
    public List<GameObject> Team2GridAreas;
    public bool GameStarted = false;
    public bool LastPhase = false;
    public bool Victory = false;
    public bool Lose = false;
    private int phase = 0;
    private FormationManager formationManager;
    private BattlefieldUIManager battleFieldUIManager;

    private void Start()
    {
        Characters = GameObject.Find("Characters");
        formationManager = GameObject.Find("Managers").GetComponent<FormationManager>();
        battleFieldUIManager = GameObject.Find("Managers").GetComponent<BattlefieldUIManager>();
        for (int i = 0; i < EnemyTeamIDs.Count; i++)
        {
            EnemyTeam1Formation.Add(EnemyTeamGridPositions[i], EnemyTeamIDs[i]);
            EnemyTeam2Formation.Add(EnemyTeamGridPositions[i], EnemyTeamIDs[i]);
        }
        EnemyTeamFormations.Add(EnemyTeam1Formation);
        EnemyTeamFormations.Add(EnemyTeam2Formation);

    }
    private void Update()
    {
        if(Team2.Count == 0 && GameStarted && !LastPhase)
        {
            phase += 1;
            Debug.Log("Moving to next phase");
            StartCoroutine(StartMovingSequence());
        }
        if(EnemyTeamFormations.Count == phase + 1)
        {
            LastPhase = true;
        }
        if(LastPhase && Team2.Count == 0)
        {
            Victory = true;
            Debug.Log("You Win The Dungeon");
        }
        if (LastPhase && Team1.Count == 0)
        {
            Lose = true;
            Debug.Log("You Lost The Dungeon");
        }
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
            GameObject heroGO = Instantiate(HeroObjectPrefab, gridArea.transform.GetChild(gridID).transform.position, Quaternion.identity, Characters.transform);
            heroGO.GetComponent<HeroController>().GridCurrentlyOn = gridArea.transform.GetChild(gridID).transform.GetComponent<GridController>();
            heroGO.GetComponent<Hero>().HeroObject = heroGO;
            heroGO.transform.tag = teamName;
            GameObject heroSkin = Instantiate(FindHero(heroID).HeroSkin, heroGO.transform.position, Quaternion.identity, heroGO.transform);
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
    public IEnumerator StartMovingSequence()
    {
        StartGame(false);
        battleFieldUIManager.SetIngamePanel(false);
        Camera.main.transform.DOMoveZ(14 * (phase +1) , 3f);
        SetUpFormation(EnemyTeamFormations[phase], Team2GridAreas[phase], "Team2", Team2);
        foreach (Hero hero in Team1)
        {
            HeroController tempHeroController = hero.HeroObject.GetComponent<HeroController>();
            tempHeroController.Agent.enabled = true;
            tempHeroController.Agent.isStopped = false;
            tempHeroController.RunningAnimation();
            tempHeroController.Agent.SetDestination(Team1GridAreas[phase].transform.GetChild(tempHeroController.GridCurrentlyOn.ID).transform.position);
        }
        int whileHandbreak = 0;
        while(true)
        {
            bool remainingDistFlag = true;
            foreach (Hero hero in Team1)
            {
                HeroController tempHeroController = hero.HeroObject.GetComponent<HeroController>();
                if (tempHeroController.Agent.remainingDistance == 0 || tempHeroController.Agent.remainingDistance > 1.2f)
                {
                    if (remainingDistFlag)
                    {
                        remainingDistFlag = false;
                    }
                }
            }
            if (remainingDistFlag)
            {
                break;
            }
            yield return new WaitForSeconds(1);
            whileHandbreak += 1;

            if(whileHandbreak > 10)
            {
                Debug.Log("Navmesh Error");
                break;
            }
        }
        foreach (Hero hero in Team1)
        {
            HeroController tempHeroController = hero.HeroObject.GetComponent<HeroController>();
            tempHeroController.IdleAnimation();
        }
        yield return new WaitForSeconds(0.1f);
        battleFieldUIManager.SetIngamePanel(true);
        StartGame(true);
    }
}