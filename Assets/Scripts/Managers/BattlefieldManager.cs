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
    public Dictionary<int, int> EnemyTeam3Formation = new Dictionary<int, int>();
    public List<Dictionary<int, int>> EnemyTeamFormations = new List<Dictionary<int, int>>();
    public List<HeroSO> HeroesSO;
    public GameObject Characters;
    public GameObject HeroObjectPrefab;
    private GameObject cameraGO;
    public List<int> EnemyTeam1IDs;
    public List<int> EnemyTeam1GridPositions;
    public List<int> EnemyTeam2IDs;
    public List<int> EnemyTeam2GridPositions;
    public List<int> EnemyTeam3IDs;
    public List<int> EnemyTeam3GridPositions;
    public List<GameObject> Team1GridAreas;
    public List<GameObject> Team2GridAreas;
    public bool GameStarted = false;
    public bool LastPhase = false;
    public bool Victory = false;
    public bool Lose = false;
    bool followPlayer = false;
    private int phase = 0;
    private BattlefieldUIManager battleFieldUIManager;

    private void Start()
    {
        Characters = GameObject.Find("Characters");
        cameraGO = GameObject.Find("CameraGO");
        battleFieldUIManager = GameObject.FindObjectOfType<BattlefieldUIManager>();
        for (int i = 0; i < EnemyTeam1IDs.Count; i++)
        {
            EnemyTeam1Formation.Add(EnemyTeam1GridPositions[i], EnemyTeam1IDs[i]);
        }

        for (int i = 0; i < EnemyTeam2IDs.Count; i++)
        {
            EnemyTeam2Formation.Add(EnemyTeam2GridPositions[i], EnemyTeam2IDs[i]);
        }

        for (int i = 0; i < EnemyTeam3IDs.Count; i++)
        {
            EnemyTeam3Formation.Add(EnemyTeam3GridPositions[i], EnemyTeam3IDs[i]);
        }
        EnemyTeamFormations.Add(EnemyTeam1Formation);
        EnemyTeamFormations.Add(EnemyTeam2Formation);
        EnemyTeamFormations.Add(EnemyTeam3Formation);

    }
    private void Update()
    {
        
        if (Team2.Count == 0 && GameStarted && !LastPhase)
        {
            phase += 1;
            StartCoroutine(StartMovingSequence());
        }
        if (LastPhase && Team2.Count == 0 && !Victory)
        {
            Victory = true;
            Debug.Log("You Win The Dungeon");
        }
        if (LastPhase && Team1.Count == 0 && !Lose)
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
            GameObject heroGO = Instantiate(HeroObjectPrefab, gridArea.transform.GetChild(gridID).transform.position, gridArea.transform.GetChild(gridID).transform.rotation, Characters.transform);
            heroGO.GetComponent<HeroController>().GridCurrentlyOn = gridArea.transform.GetChild(gridID).transform.GetComponent<GridController>();
            heroGO.GetComponent<Hero>().HeroObject = heroGO;
            heroGO.transform.tag = teamName;
            GameObject heroSkin = Instantiate(FindHero(heroID).HeroSkin, heroGO.transform.position, gridArea.transform.GetChild(gridID).transform.rotation , heroGO.transform);
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
        SetUpFormation(EnemyTeamFormations[phase], Team2GridAreas[phase], "Team2", Team2);
        yield return new WaitForSeconds(2f);
        
        foreach (Hero hero in Team1)
        {
            HeroController tempHeroController = hero.HeroObject.GetComponent<HeroController>();
            tempHeroController.Agent.enabled = true;
            tempHeroController.Agent.isStopped = false;
            tempHeroController.SetIsAttacking(false);
            tempHeroController.ResetAnimations();
            tempHeroController.RunningAnimation();
            tempHeroController.Agent.SetDestination(Team1GridAreas[phase].transform.GetChild(tempHeroController.GridCurrentlyOn.ID).transform.position);
            yield return new WaitForFixedUpdate();
        }
        int whileHandbreak = 0;
        followPlayer = false;
        while(true)
        {
            bool remainingDistFlag = true;
            if(!followPlayer)
            {
                followPlayer = true;
                StartCoroutine(FollowPlayerCoroutine());
            }
            
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
                else
                {
                    tempHeroController.IdleAnimation();
                }
            }

            if (remainingDistFlag)
            {
                break;
            }
            yield return new WaitForSeconds(1);
            whileHandbreak += 1;

            Debug.Log("Waiting For Navmesh");

            if (whileHandbreak > 30)
            {
                Debug.LogError("Navmesh Error");
                break;
            }
        }
        foreach (Hero hero in Team1)
        {
            HeroController tempHeroController = hero.HeroObject.GetComponent<HeroController>();
            tempHeroController.ResetAnimations();
            tempHeroController.IdleAnimation();
        }
        battleFieldUIManager.SetIngamePanel(true);
        followPlayer = false;
        if (EnemyTeamFormations.Count == phase + 1)
        {
            LastPhase = true;
        }
        yield return new WaitForSeconds(1);
        cameraGO.transform.DOMove(Team1GridAreas[phase].transform.position + (Team2GridAreas[phase].transform.position - Team1GridAreas[phase].transform.position) / 2, 0.5f);
        cameraGO.transform.DORotate(Team1GridAreas[phase].transform.rotation.eulerAngles, 0.5f);
        yield return new WaitForSeconds(0.6f);
        StartGame(true);
    }

    IEnumerator FollowPlayerCoroutine()
    {
        while(followPlayer)
        {
            yield return new WaitForFixedUpdate();
            cameraGO.transform.DOMove(Team1[0].transform.position, 0.5f);
            cameraGO.transform.DORotate(Team1[0].transform.rotation.eulerAngles, 0.5f);
        }
    }
}