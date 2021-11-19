using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Author: Mert Karavural
//Date: 3.11.2021
public class BattlefieldManager : MonoBehaviour
{
    public List<Hero> Team1;//Hero Team
    [HideInInspector] public List<Hero> Team2; 
    public List<HeroSO> HeroesSO;
    public GameObject Characters;
    public GameObject HeroObjectPrefab;
    public List<GameObject> Team1GridAreas;
    public List<GameObject> Team2GridAreas;
    public bool GameStarted = false;

    private void Awake()
    {
        Characters = GameObject.Find("Characters");
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