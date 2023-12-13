using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryu
{
    [System.Serializable]
    public class Item
    {
        public ItemData Data;
        public Item(ItemData data) => Data = data;
    }
}

