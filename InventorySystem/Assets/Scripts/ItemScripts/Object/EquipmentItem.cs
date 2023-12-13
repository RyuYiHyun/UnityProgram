using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryu
{
    public  class EquipmentItem : Item
    {
        public EquipmentItemData EquipmentData { get; private set; }

        /* 장비에만 따로 관리해야하는 값 넣기 */

        /* ================================*/


        public EquipmentItem(EquipmentItemData data) : base(data)
        {
            EquipmentData = data;
            /* 장비에만 따로 관리해야하는 값 초기화 */

            /* ================================*/
        }
    }
}

