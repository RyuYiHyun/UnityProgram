using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ryu
{
    /// <summary> �� �� �ִ� ������ ������ </summary>
    public abstract class StackableItemData : ItemData
    {
        public int MaxAmount => _maxAmount;
        [SerializeField] private int _maxAmount = 99;
    }
}

