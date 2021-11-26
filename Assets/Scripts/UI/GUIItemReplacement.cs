using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIItemReplacement : MonoBehaviour
{
    public GameObject ItemTemplate;

    public void Awake()
    {
        if (this.gameObject.CompareTag("MarketItem"))
        {
            for (int i = 0; i < UIInventoryItemDatabase.Instance.MarketItems.Length; i++)
            {
                GameObject go = Instantiate(ItemTemplate,this.gameObject.transform);
                go.SetActive(true);
                //Debug.Log("111");
            }
        }
        else if (this.gameObject.CompareTag("PlayerItem"))
        {
            for (int i = 0; i < UIInventoryItemDatabase.Instance.PlayerItems.Length; i++)
            {
                GameObject go = Instantiate(ItemTemplate, this.gameObject.transform);
                go.SetActive(true);
                //Debug.Log("111");
            }
        }
        
    }
}