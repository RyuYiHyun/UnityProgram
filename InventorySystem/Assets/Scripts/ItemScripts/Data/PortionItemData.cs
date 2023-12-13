using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ryu
{
    [CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/PortionCreate", order = 2), System.Serializable]
    public class PortionItemData : StackableItemData
    {
        /// <summary> ȿ����(ȸ���� ��) </summary>
        public float Value => _value;
        [SerializeField] private float _value;
        public override Item CreateItem()
        {
            return new PortionItem(this);
        }
    }
}

