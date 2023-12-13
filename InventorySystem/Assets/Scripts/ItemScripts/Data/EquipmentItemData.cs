using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ryu
{
    public abstract class EquipmentItemData : ItemData
    {
        /// <summary> 최대 내구도 </summary>
        public EquipSlots EquipPart => _equipPart;
        public AbilityType Ability => _ability;

        public int Value => _value;

        [SerializeField] private EquipSlots _equipPart = EquipSlots.NoEquip;
        [SerializeField] private AbilityType _ability = AbilityType.NoAbility;
        [SerializeField] private int _value = 0;
    }
}

