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
            // �ӽ� : ���� �ϳ� ����
            Amount--;

            return true;
        }

        //protected override StackableItem Clone(int amount)
        //{
        //    return new PortionItem(stackableItemData as PortionItemData, amount);
        //}
    }
}

