using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ryu
{
    /// <summary> UI 윈도우 드레그 이동 </summary>
    public class MoveWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField, Header("필수 설정")]
        private Camera UICamera;
        [SerializeField, Header("자동 설정")]
        private RectTransform MoveArea;
        [SerializeField, Header("자동 설정")]
        private RectTransform window;

        private Vector2 DownPosition;
        private Vector2 screenPoint;

        private void Awake()
        {
            // 속한 캠퍼스 찾기
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
            // 이동위의 같이 움직이는 프레임 지정
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
        // 드래그 : 마우스 커서 위치로 이동
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MoveArea, eventData.position, UICamera, out screenPoint);
            Vector2 offset = screenPoint - DownPosition;
            DownPosition = screenPoint;
            window.anchoredPosition += offset;
        }
    }
}