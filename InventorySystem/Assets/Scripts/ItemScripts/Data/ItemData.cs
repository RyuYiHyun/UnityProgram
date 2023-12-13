using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryu
{
    public enum ItemType
    {
        Basic,
        Armor,
        Portion,
    }
    //[CreateAssetMenu(fileName = "Item_", menuName = "Inventory System/ItemCreate", order = 1), System.Serializable]
    // 베이스
    public abstract class ItemData : ScriptableObject
    {
        public int ID => _id;
        public string Name => _name;
        public string Tooltip => _tooltip;
        public bool Stackable => stackable;
        public Sprite IconSprite => _iconSprite;

        public ItemType itemType => _itemType;

        [SerializeField] private int _id;
        [SerializeField] private string _name;    // 아이템 이름
        [Multiline]
        [SerializeField] private string _tooltip; // 아이템 설명
        [SerializeField] private bool stackable;
        [SerializeField] private Sprite _iconSprite; // 아이템 아이콘
        [SerializeField] private GameObject _dropItemPrefab; // 바닥에 떨어질 때 생성할 프리팹
        [SerializeField] private ItemType _itemType;

        /// <summary> 타입에 맞는 새로운 아이템 생성 </summary>
        public abstract Item CreateItem();
    }
}