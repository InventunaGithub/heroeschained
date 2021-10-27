using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Author: Mert Karavural
//Date: 28 Sep 2020

public class MockController : MonoBehaviour
{
    public Player King1;
    public Player King2;
    public List<HeroSO> Team1SO;
    public List<HeroSO> Team2SO;
    public List<Hero> Team1Heroes;
    public List<Hero> Team2Heroes;
    public GameObject Characters;
    public GameObject HeroPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        King1.PlayerID = 1;
        King2.PlayerID = 2;
        King1.Name = "King1";
        King2.Name = "King2";
        scriptableObjectToNormal();
        Characters = GameObject.Find("Characters");
        int onChildIndex = 0;
        foreach (Hero hero in Team1Heroes)
        {
            Transform gridPlace = GetComponent<FormationManager>().gridArea1.transform.GetChild(onChildIndex).transform;
            King1.Team.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.HeroSkin, hero.Skills, hero.AIType, hero.HeroType));
            GameObject heroObject = Instantiate(HeroPrefab, Vector3.zero, Quaternion.identity);
            heroObject.transform.tag = King1.Name;
            heroObject.transform.SetParent(Characters.transform, false);
            heroObject.GetComponent<HeroController>().ChildNo = King1.Team.IndexOf(King1.Team.Last());
            heroObject.transform.position = gridPlace.position;
            heroObject.transform.rotation = gridPlace.rotation;
            gridPlace.GetComponent<Renderer>().material.color = Color.red;
            heroObject.GetComponent<HeroController>().Owner = King1;
            heroObject.GetComponent<HeroController>().Enemy = King2;
            King1.Team.Last().HeroObject = heroObject;
            onChildIndex += 1;
        }
        onChildIndex = 0;
        foreach (Hero hero in Team2Heroes)
        {
            Transform gridPlace = GetComponent<FormationManager>().gridArea2.transform.GetChild(onChildIndex).transform;
            King2.Team.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.HeroSkin, hero.Skills, hero.AIType, hero.HeroType));
            GameObject heroObject = Instantiate(HeroPrefab, Vector3.zero, Quaternion.identity);
            heroObject.transform.SetParent(Characters.transform, false);
            heroObject.transform.tag = King2.Name;
            heroObject.GetComponent<HeroController>().ChildNo = King2.Team.IndexOf(King2.Team.Last());
            heroObject.transform.position = gridPlace.position ;
            heroObject.transform.rotation = gridPlace.rotation ;
            heroObject.GetComponent<HeroController>().Owner = King2;
            heroObject.GetComponent<HeroController>().Enemy = King1;
            King2.Team.Last().HeroObject = heroObject;
            onChildIndex += 1;
        }
    }

    void scriptableObjectToNormal()
    {
        foreach (var hero in Team1SO)
        {
            Team1Heroes.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.HeroSkin, hero.Skills, hero.AIType, hero.HeroType));
        }
        foreach (var hero in Team2SO)
        {
            Team2Heroes.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.HeroSkin, hero.Skills, hero.AIType, hero.HeroType));
        }
    }

    public void StartTheGame()
    {
        GameObject.Find("GridArea1").SetActive(false);
        GameObject.Find("GridArea2").SetActive(false);
        foreach (Transform child in GameObject.Find("Characters").transform)
        {
            child.GetComponent<HeroController>().Activate(true);
        }
    }
}
