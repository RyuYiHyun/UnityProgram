using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ryu
{
    /// <summary> UI ������ �巹�� �̵� </summary>
    public class MoveWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField, Header("�ʼ� ����")]
        private Camera UICamera;
        [SerializeField, Header("�ڵ� ����")]
        private RectTransform MoveArea;
        [SerializeField, Header("�ڵ� ����")]
        private RectTransform window;

        private Vector2 DownPosition;
        private Vector2 screenPoint;

        private void Awake()
        {
            // ���� ķ�۽� ã��
            if (MoveArea == null)
            {
                Transform isCanvas;
                isCanvas = transform.parent;
                while (MoveArea == null)
                {
                    if (null != isCanvas.GetComponent<Canvas>())
                    {
                        MoveArea = isCanvas.GetComponent<RectTransform>();
                    }
                    else
                    {
                        isCanvas = isCanvas.parent;
                        if (isCanvas == null)
                        {
                            break;
                        }
                    }
                }
            }
            // �̵����� ���� �����̴� ������ ����
            if (window == null)
            {
                window = transform.parent.GetComponent<RectTransform>();
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MoveArea, eventData.position, UICamera, out screenPoint);
            DownPosition = screenPoint;
        }
        // �巡�� : ���콺 Ŀ�� ��ġ�� �̵�
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MoveArea, eventData.position, UICamera, out screenPoint);
            Vector2 offset = screenPoint - DownPosition;
            DownPosition = screenPoint;
            window.anchoredPosition += offset;
        }
    }
}