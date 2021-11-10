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

            UIItemSlot[] slots = this.m_Container.gameObject.GetComponentsInChildren<UIItemSlot>();
            InventoryItem[] playerInventoryItems = UIInventoryItemDatabase.Instance.items;
            List<UIItemInfo> playerItemInfoItems = new();

            for (int i = 0; i < playerInventoryItems.Length; i++)
            {
                playerItemInfoItems.Add(InventoryItemConventer.Instance.Conventer(playerInventoryItems[i]));
            }

            if (slots.Length > 0 && playerItemInfoItems.Count > 0)
            {
                foreach (UIItemSlot slot in slots)
                    slot.Assign(playerItemInfoItems[Random.Range(0, playerItemInfoItems.Count)]);
            }

            this.Destruct();
        }

        private void Destruct()
        {
            DestroyImmediate(this);
        }
    }
}
