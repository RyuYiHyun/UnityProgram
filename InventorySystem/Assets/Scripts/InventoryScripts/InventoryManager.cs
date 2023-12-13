using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Ryu
{
    public class InventoryManager : MonoBehaviour
    {
        #region �̱��� ����
        static InventoryManager m_instance = null;
        public static InventoryManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<InventoryManager>();
                    if (m_instance == null)
                    {
                        GameObject obj = new GameObject("InventoryManager", typeof(InventoryManager));
                        m_instance = obj.GetComponent<InventoryManager>();
                    }
                }
                return m_instance;
            }
        }
        #endregion



        public Canvas m_canvas;
        private GraphicRaycaster _gr;
        private PointerEventData _ped;
        private List<RaycastResult> _rrList;
        private void Awake()
        {
            _gr = m_canvas.GetComponent<GraphicRaycaster>();
            _ped = new PointerEventData(null);
            _rrList = new List<RaycastResult>(10);
        }
        private T RaycastAndGetFirstComponent<T>() where T : Component
        {
            _rrList.Clear();
            _ped.position = Input.mousePosition;
            _gr.Raycast(_ped, _rrList);

            if (_rrList.Count == 0)
                return null;

            return _rrList[0].gameObject.transform.GetComponent<T>();
        }







        public Slot slotPrefab;// ���� ���纻
        public List<Item> ItemDB;
        public Inventory[] inventoryList;

        private void Start()
        {
            for(int i = 0; i< inventoryList.Length; i++ )
            {
                InitializeSlotTable(inventoryList[i].GetComponent<RectTransform>(), slotPrefab, inventoryList[i].inventorySlots, i);
                UpdateAllSlot(inventoryList[i].inventorySlots);
            }
        }


        void InitializeSlotTable(RectTransform container, Slot slotPrefab, SlotContainer[] slots, int groupNum)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    slots[i] = new SlotContainer();// ��������� �Ҵ�
                }
                GameObject newSlot = Instantiate(slotPrefab.gameObject, container);// �����̳ʿ� �ڽ����� ���� ����
                Vector3 pos = newSlot.GetComponent<RectTransform>().localPosition;
                pos.z = 0;
                newSlot.GetComponent<RectTransform>().localPosition = pos;
                newSlot.GetComponent<Slot>().contain = slots[i];
                slots[i].slot = newSlot.GetComponent<Slot>();
                slots[i].slot.gameObject.SetActive(true);
                slots[i].slot.GroupNum = groupNum;
                slots[i].slot.Index = i;
            }
        }

        public Item FindItem(int itemId)// ������ �˻�
        {
            for (int i = 0; i < ItemDB.Count; i++)
            {
                if (ItemDB[i].Data.ID == itemId)
                {
                    return ItemDB[i];
                }
            }
            return null;
        }
        public Item FindItem(Sprite sprite)// ������ �˻�
        {
            if (!sprite)
            {
                return null;
            }
            for (int i = 0; i < ItemDB.Count; i++)
            {
                if (ItemDB[i].Data.IconSprite == sprite)
                {
                    return ItemDB[i];
                }
            }
            return null;
        }
        public Item FindItem(string name)// ������ �˻�
        {
            for (int i = 0; i < ItemDB.Count; i++)
            {
                if (ItemDB[i].Data.Name == name)
                {
                    return ItemDB[i];
                }
            }
            return null;
        }

        // index ������
        public void UpdateManySlot(SlotContainer[] slots, params int[] indices)
        {
            foreach (var i in indices)
            {
                UpdateSlot(slots, i);
            }
        }
        public void UpdateSlot(SlotContainer[] slots, int index)
        {
            if(slots[index].item == null || slots[index].item.Data == null)
            {
                slots[index].slot.RemoveItem();
                return;
            }
            Item slotItem = FindItem(slots[index].item.Data.IconSprite);// �������� �ִ��� �˻�
            if (slotItem != null)
            {
                if (!slotItem.Data.Stackable) // �������� ������ �Ұ����ѵ� ī��Ʈ��������1�� ���̱�
                {
                    slots[index].amount = 1;
                }
                slots[index].slot.SetIcon(slotItem.Data.IconSprite);
                slots[index].slot.SetAmount(slots[index].amount);
            }
            else    // ������ �˻��Ǵ� ���� ������ ��Ȱ��ȭ
            {
                slots[index].slot.RemoveItem();
            }
        }
        public void UpdateAllSlot(SlotContainer[] slots)
        {
            for (int index = 0; index < slots.Length; index++)
            {
                if (slots[index].item == null || slots[index].item.Data == null)
                {
                    slots[index].slot.RemoveItem();
                    continue;
                }
                Item slotItem = FindItem(slots[index].item.Data.IconSprite);// �������� �ִ��� �˻�
                if (slotItem != null)
                {
                    if (!slotItem.Data.Stackable) // �������� ������ �Ұ����ѵ� ī��Ʈ��������1�� ���̱�
                    {
                        slots[index].amount = 1;
                    }
                    slots[index].slot.SetIcon(slotItem.Data.IconSprite);
                    slots[index].slot.SetAmount(slots[index].amount);
                }
                else    // ������ �˻��Ǵ� ���� ������ ��Ȱ��ȭ
                {
                    slots[index].slot.RemoveItem();
                }
            }
        }


        [SerializeField, Header("�ʼ� ����")]
        private Camera UICamera; // ī�޶�
        [SerializeField, Header("�ʼ� ����")]
        private RectTransform MoveArea;// ĵ����

        private Vector2 DownPosition;
        private Vector2 screenPoint;

        private Slot startSlot;

        public Slot MouseSlot;
        public int DragGroup;
        public void OnPointerDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("DOwn Mouse2");
                startSlot = RaycastAndGetFirstComponent<Slot>();
                if (startSlot != null && startSlot.HaveItem)
                {
                    Debug.Log("DOwn Mouse3");
                    DragGroup = startSlot.GroupNum;
                    startSlot.Giveto(MouseSlot);
                    startSlot.contain.item = null;
                    startSlot.contain.amount = 0;

                    MouseSlot.UpdateUI();
                    UpdateSlot(inventoryList[startSlot.GroupNum].inventorySlots, startSlot.Index);

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(MoveArea, startSlot.GetComponent<RectTransform>().anchoredPosition, UICamera, out screenPoint);
                    DownPosition = screenPoint;
                    MouseSlot.GetComponent<RectTransform>().anchoredPosition = screenPoint;
                    MouseSlot.GetComponent<RectTransform>().gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log("DOwn Mouse4");
                    startSlot = null;
                }
            }
        }
        // �巡�� : ���콺 Ŀ�� ��ġ�� �̵�
        void OnDrag()
        {
            if (startSlot == null)
            {
                return;
            }
            if (Input.GetMouseButton(0))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(MoveArea, Input.mousePosition, UICamera, out screenPoint);
                Vector2 offset = screenPoint - DownPosition;
                DownPosition = screenPoint;
                MouseSlot.GetComponent<RectTransform>().anchoredPosition += offset;
            }
        }
        public void OnPointerUp()
        {
            if (startSlot == null)
            {
                return;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("UP Mouse");
                EndDrag();
                startSlot = null;

                MouseSlot.contain.item = null;
                MouseSlot.contain.amount = 0;
                MouseSlot.GroupNum = -1;
                MouseSlot.UpdateUI();
                DragGroup = -1;
                MouseSlot.GetComponent<RectTransform>().gameObject.SetActive(false);
            }
        }
        public void EndDrag()
        {
            Slot endDragSlot = RaycastAndGetFirstComponent<Slot>();
            if (endDragSlot != null)
            {
                if(startSlot == endDragSlot)
                {
                    startSlot.contain.item = MouseSlot.contain.item;
                    startSlot.contain.amount = MouseSlot.contain.amount;
                    startSlot.UpdateUI();
                }
                if (startSlot != endDragSlot)
                {
                    startSlot.contain.item = endDragSlot.contain.item;
                    startSlot.contain.amount = endDragSlot.contain.amount;

                    endDragSlot.contain.item = MouseSlot.contain.item;
                    endDragSlot.contain.amount = MouseSlot.contain.amount;
                    startSlot.UpdateUI();
                    endDragSlot.UpdateUI();
                }
            }
            else
            {
                // ������ �Ǵ� ���ڸ�
                //1. ���ڸ�
                startSlot.contain.item = MouseSlot.contain.item;
                startSlot.contain.amount = MouseSlot.contain.amount;
                startSlot.UpdateUI();
            }
        }


        private void Update()
        {
            OnPointerDown();
            OnDrag();
            OnPointerUp();
        }
    }


}
