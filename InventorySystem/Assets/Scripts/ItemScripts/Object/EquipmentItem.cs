using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryu
{
    public  class EquipmentItem : Item
    {
        public EquipmentItemData EquipmentData { get; private set; }

        /* ��񿡸� ���� �����ؾ��ϴ� �� �ֱ� */

        /* ================================*/


        public EquipmentItem(EquipmentItemData data) : base(data)
        {
            EquipmentData = data;
            /* ��񿡸� ���� �����ؾ��ϴ� �� �ʱ�ȭ */

            /* ================================*/
        }
    }
}

