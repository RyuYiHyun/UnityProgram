using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryu
{
    [CreateAssetMenu(fileName = "Item_Armor_", menuName = "Inventory System/ArmorCreate", order = 1), System.Serializable]
    public class ArmorItemData : EquipmentItemData
    {
        /// <summary> ¹æ¾î·Â </summary>
        public int Weight => _weight;
        [SerializeField] private int _weight = 1;
        public override Item CreateItem()
        {
            return new ArmorItem(this);
        }
    }
}
