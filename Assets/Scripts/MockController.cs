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
    public List<HeroSO> MockHeroSo;
    public List<Hero> MockHeroes;
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
        for (int i = 0; i < Characters.transform.childCount; i++)
        {
            
            if(i >= (Characters.transform.childCount / 2))
            {

                Hero hero = MockHeroes[i % MockHeroes.Count];
                King2.Team.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.Skills , hero.AIType));
                Characters.transform.GetChild(i).GetComponent<HeroController>().ChildNo = King2.Team.IndexOf(King2.Team.Last());
                King2.Team.Last().HeroObject = Characters.transform.GetChild(i).gameObject;
                Characters.transform.GetChild(i).GetComponent<HeroController>().Owner = King2;
                Characters.transform.GetChild(i).GetComponent<HeroController>().Enemy = King1;
            }
            else
            {
                Hero hero = MockHeroes[i % MockHeroes.Count];
                King1.Team.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.Skills, hero.AIType));
                Characters.transform.GetChild(i).GetComponent<HeroController>().ChildNo = King1.Team.IndexOf(King1.Team.Last());
                King1.Team.Last().HeroObject = Characters.transform.GetChild(i).gameObject;
                Characters.transform.GetChild(i).GetComponent<HeroController>().Owner = King1;
                Characters.transform.GetChild(i).GetComponent<HeroController>().Enemy = King2;
            }
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void scriptableObjectToNormal()
    {
        foreach (var hero in MockHeroSo)
        {
            MockHeroes.Add(new Hero(hero.Name, hero.HeroID, hero.BaseHealth, hero.BaseDamage, hero.Strength, hero.Dexterity, hero.Intelligence, hero.Vitality, hero.Range, hero.Skills, hero.AIType));
        }
    }
}
