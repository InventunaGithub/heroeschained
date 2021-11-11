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
                UIItemSlot[] playerSlots = this.m_Container.gameObject.GetComponentsInChildren<UIItemSlot>();
                List<UIItemInfo> playerItemInfo = new();
                InventoryItem[] playerInventoryItem = UIInventoryItemDatabase.Instance.PlayerItems;

                for (int i = 0; i < playerInventoryItem.Length; i++)
                {
                    playerItemInfo.Add(InventoryItemConventer.Instance.Conventer(playerInventoryItem[i]));
                }

                if (playerSlots.Length > 0 && playerItemInfo.Count > 0)
                {
                    foreach (UIItemSlot slot in playerSlots)
                        slot.Assign(playerItemInfo[Random.Range(0, playerItemInfo.Count)]);
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
                    foreach (UIItemSlot slot in marketSlots)
                        slot.Assign(marketItemInfo[Random.Range(0, marketItemInfo.Count)]);
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
