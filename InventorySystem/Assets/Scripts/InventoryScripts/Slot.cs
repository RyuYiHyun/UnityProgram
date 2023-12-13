using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ryu
{
    [System.Serializable]
    public class Slot : MonoBehaviour
    {
        public SlotContainer contain;

        public Image icon;  // 아이템 아이콘
        public Text text;  // 수량
        [HideInInspector]
        public int Index;
        [HideInInspector]
        public int GroupNum;
        public bool HaveItem;

        public void UpdateUI()
        {
            if (contain.item == null || contain.item.Data == null)
            {
                RemoveItem();
                return;
            }
            Item slotItem = InventoryManager.Instance.FindItem(contain.item.Data.IconSprite);// 아이템이 있는지 검색
            if (slotItem != null)
            {
                if (!slotItem.Data.Stackable) // 아이템이 스택이 불가능한데 카운트가있으면1로 줄이기
                {
                    contain.amount = 1;
                }
                SetIcon(slotItem.Data.IconSprite);
                SetAmount(contain.amount);
            }
            else    // 아이템 검색되는 것이 없으면 비활성화
            {
                RemoveItem();
            }
        }
        public void AddItem(Item _item, int _amount = 1)
        {
            contain.item = _item;
            contain.amount = _amount;
            SetIcon(contain.item.Data.IconSprite);
            SetAmount(_amount);
            HaveItem = true;
        }

        public void SetGroupNum(int groupNum) => GroupNum = groupNum;
        public void RemoveItem()
        {
            contain.item = null;
            contain.amount = 0;
            HaveItem = false;
            icon.sprite = null;
            HideIcon();
            HideText();
        }
        public void SetIcon(Sprite sprite)
        {
            if (sprite != null)
            {
                HaveItem = true;
                icon.sprite = sprite;
                ShowIcon();
            }
        }
        public void SetAmount(int amount)
        {
            if (amount > 1)
            {
                text.text = amount.ToString();
                ShowText();
            }
            else
            {
                HideText();
            }
        }

        public void HideIcon() => icon.gameObject.SetActive(false);
        public void ShowIcon() => icon.gameObject.SetActive(true);

        public void HideText() => text.gameObject.SetActive(false);
        public void ShowText() => text.gameObject.SetActive(true);

        public void Giveto(Slot taker)
        {
            taker.contain.item = contain.item;
            taker.contain.amount = contain.amount;
        }

        
    }


}

