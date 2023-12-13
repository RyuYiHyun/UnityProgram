using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryu
{
    [System.Serializable]
    public class SlotContainer
    {
        public Item item;
        public int amount;
        [HideInInspector]
        public Slot slot;
    }
}

