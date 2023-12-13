using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ryu
{
    [System.Flags]
    public enum EquipSlots
    {
        NoEquip =0,
        Primary = 1 << 0,
        Secondary = 1 << 1,
        Head = 1 << 2,
        Body = 1 << 3,
        Shoes = 1 << 4,
    }

    [System.Flags]
    public enum AbilityType
    {
        NoAbility,
        Attack = 1 << 0,
        Defense = 1 << 1,
        HP = 1 << 2,
        Speed = 1 << 3,
    }
}