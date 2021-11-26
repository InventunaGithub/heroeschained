using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI
{
    public class Test_UIItemSlot_AssignAll : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] private Transform m_Container;
        #pragma warning restore 0649

        void Start()
        {
            if (this.m_Container == null || UIInventoryItemDatabase.Instance == null)
            {
                this.Destruct();
                return;
            }      

            if (this.gameObject.CompareTag("PlayerItem"))
            {
                UIItemSlot[] inventorySlots = this.m_Container.gameObject.GetComponentsInChildren<UIItemSlot>();
                List<UIItemInfo> inventoryItemInfo = new();
                InventoryItem[] inventoryInventoryItem = UIInventoryItemDatabase.Instance.PlayerItems;
            
                for (int i = 0; i < inventoryInventoryItem.Length; i++)
                {
                    inventoryItemInfo.Add(InventoryItemConventer.Instance.Conventer(inventoryInventoryItem[i]));
                }

                if (inventorySlots.Length > 0 && inventoryItemInfo.Count > 0)
                {
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        UIItemSlot slot = inventorySlots[i];
                        slot.Assign(inventoryItemInfo[i]);
                    }
                }
            }
            else if (this.gameObject.CompareTag("MarketItem"))
            {
                UIItemSlot[] marketSlots = this.m_Container.gameObject.GetComponentsInChildren<UIItemSlot>();
                List<UIItemInfo> marketItemInfo = new();
                InventoryItem[] marketInventoryItem = UIInventoryItemDatabase.Instance.MarketItems;

                for (int i = 0; i < marketInventoryItem.Length; i++)
                {
                    marketItemInfo.Add(InventoryItemConventer.Instance.Conventer(marketInventoryItem[i]));
                }

                if (marketSlots.Length > 0 && marketItemInfo.Count > 0)
                {
                    for (int i = 0; i < marketSlots.Length; i++)
                    {
                        UIItemSlot slot = marketSlots[i];
                        slot.Assign(marketItemInfo[i]);
                    }
                }
            }
            this.Destruct();
        }

        private void Destruct()
        {
            DestroyImmediate(this);
        }
    }
}