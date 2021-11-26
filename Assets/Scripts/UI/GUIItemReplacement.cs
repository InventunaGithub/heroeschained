using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuloGames.UI;

public class GUIItemReplacement : MonoBehaviour
{
    public GameObject ItemTemplate;
    public Transform PlayerContext;
    public Transform MarketContext;

    public void Awake()
    {
        if (MarketContext.gameObject.CompareTag("MarketItem"))
        {
            for (int i = 0; i < UIInventoryItemDatabase.Instance.MarketItems.Length; i++)
            {
                GameObject go = Instantiate(ItemTemplate, MarketContext);
                go.tag = "MarketItem";
                go.GetComponentInChildren<Text>().text = UIInventoryItemDatabase.Instance.MarketItems[i].ItemName;
                go.SetActive(true);
            }
        }
        if (PlayerContext.gameObject.CompareTag("PlayerItem"))
        {
            for (int i = 0; i < UIInventoryItemDatabase.Instance.PlayerItems.Length; i++)
            {
                GameObject go = Instantiate(ItemTemplate, PlayerContext);
                go.tag = "PlayerItem";
                go.GetComponentInChildren<Text>().text = UIInventoryItemDatabase.Instance.PlayerItems[i].ItemName;
                go.SetActive(true);
            }
        }
    }
}