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
    public GameObject HeroPrefab;
    private GameObject gridArea1;
    private GameObject gridArea2;
    private Ray ray;
    public bool ClickedOnHero;
    public GameObject HitGO;
    public Transform LastGridPos;
    private LayerMask heroLayer;
    public bool PlacingOnFullGrid;
    public GridController GC;
    void Awake()
    {
        Characters = GameObject.Find("Characters");
        gridArea1 = GameObject.Find("GridArea1");
        gridArea2 = GameObject.Find("GridArea2");
        heroLayer = LayerMask.GetMask("HeroLayer");
        Physics.IgnoreLayerCollision(9, 9, true);
        int index = 0;
        foreach (var Hero in Team1SO)
        {
            GameObject heroGO = Instantiate(HeroPrefab, gridArea1.transform.GetChild(index).transform.position , gridArea1.transform.GetChild(index).transform.rotation);
            heroGO.transform.SetParent(Characters.transform);
            heroGO.transform.tag = "Team1";
            GameObject heroSkin = Instantiate(Hero.HeroSkin , heroGO.transform.position, Quaternion.identity);
            heroSkin.transform.SetParent(heroGO.transform);
            heroGO.GetComponent<Hero>().Init(Hero);
            Team1.Add(heroGO.GetComponent<Hero>());
            index += 1;
        }
        index = 0;
        foreach (var Hero in Team2SO)
        {
            GameObject heroGO = Instantiate(HeroPrefab, gridArea2.transform.GetChild(index).transform.position, gridArea1.transform.GetChild(index).transform.rotation);
            heroGO.transform.SetParent(Characters.transform);
            heroGO.transform.tag = "Team2";
            GameObject heroSkin = Instantiate(Hero.HeroSkin, heroGO.transform.position, Quaternion.identity);
            heroSkin.transform.SetParent(heroGO.transform);
            heroGO.GetComponent<Hero>().Init(Hero);
            Team2.Add(heroGO.GetComponent<Hero>());
            index += 1;
        }
        
    }

    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 10000000);
        if (Input.GetMouseButtonDown(0) && hitData.transform.tag == "Team1")
        {
            ClickedOnHero = true;
            HitGO = hitData.transform.gameObject;
        }

        if (Input.GetMouseButton(0) && ClickedOnHero)
        {
            if (Physics.Raycast(ray, out hitData, 1000, ~heroLayer))
            {
                HitGO.transform.position = hitData.point;
            }

        }

        if (Input.GetMouseButtonUp(0) && ClickedOnHero)
        {
            SetPos(LastGridPos);
            ClickedOnHero = false;
            HitGO = null;
        }

    }
    public void SetPos(Transform posRef)
    {
        HitGO.transform.position = posRef.position;
    }


}
