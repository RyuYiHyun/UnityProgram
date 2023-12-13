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
    // ���̽�
    public abstract class ItemData : ScriptableObject
    {
        public int ID => _id;
        public string Name => _name;
        public string Tooltip => _tooltip;
        public bool Stackable => stackable;
        public Sprite IconSprite => _iconSprite;

        public ItemType itemType => _itemType;

        [SerializeField] private int _id;
        [SerializeField] private string _name;    // ������ �̸�
        [Multiline]
        [SerializeField] private string _tooltip; // ������ ����
        [SerializeField] private bool stackable;
        [SerializeField] private Sprite _iconSprite; // ������ ������
        [SerializeField] private GameObject _dropItemPrefab; // �ٴڿ� ������ �� ������ ������
        [SerializeField] private ItemType _itemType;

        /// <summary> Ÿ�Կ� �´� ���ο� ������ ���� </summary>
        public abstract Item CreateItem();
    }
}