using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ryu
{
    public class PortionItem : StackableItem
    {
        public PortionItem(PortionItemData data, int amount = 1) : base(data, amount) { }

        public bool Use()
        {
            // 임시 : 개수 하나 감소
            Amount--;

            return true;
        }

        //protected override StackableItem Clone(int amount)
        //{
        //    return new PortionItem(stackableItemData as PortionItemData, amount);
        //}
    }
}

