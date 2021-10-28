using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldManager : MonoBehaviour
{
    public List<Hero> Team1;
    public List<Hero> Team2;
    public List<HeroSO> Team1SO;
    public List<HeroSO> Team2SO;
    public GameObject Characters;
    // Start is called before the first frame update
    void Start()
    {
        Characters = GameObject.Find("Characters");

        //Instantiate GameObjects , assign them correct hero and add them to the team
    }

    // Update is called once per frame
    void Update()
    {
     
    }

}
