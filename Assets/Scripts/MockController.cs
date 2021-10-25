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
            King1.Team.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.HeroSkin, hero.Skills, hero.AIType, hero.HeroType));
            Characters.transform.GetChild(onChildIndex).GetComponent<HeroController>().ChildNo = King1.Team.IndexOf(King1.Team.Last());
            King1.Team.Last().HeroObject = Characters.transform.GetChild(onChildIndex).gameObject;
            Characters.transform.GetChild(onChildIndex).GetComponent<HeroController>().Owner = King1;
            Characters.transform.GetChild(onChildIndex).GetComponent<HeroController>().Enemy = King2;
            onChildIndex += 1;
        }
        foreach (Hero hero in Team2Heroes)
        {
            King2.Team.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.HeroSkin, hero.Skills, hero.AIType, hero.HeroType));
            Characters.transform.GetChild(onChildIndex).GetComponent<HeroController>().ChildNo = King2.Team.IndexOf(King2.Team.Last());
            King2.Team.Last().HeroObject = Characters.transform.GetChild(onChildIndex).gameObject;
            Characters.transform.GetChild(onChildIndex).GetComponent<HeroController>().Owner = King2;
            Characters.transform.GetChild(onChildIndex).GetComponent<HeroController>().Enemy = King1;
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
}
