using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Ryu
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField]
        public int TapIndex;
        public TabGroup tabGroup;
        public Image background;
        public Text text;
        public UnityEvent onTabSelected;
        public UnityEvent onTabDeSelected;
        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this, TapIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        private void Start()
        {
            background = GetComponent<Image>();
            text = transform.GetChild(0).GetComponent<Text>();
            tabGroup.Subscribe(this);
            if (TapIndex == 0)
            {
                tabGroup.OnTabSelected(this, TapIndex);
            }
        }


        public void Select()
        {
            if (onTabSelected != null)
            {
                onTabDeSelected.Invoke();
            }
        }

        public void DeSelect()
        {
            if (onTabDeSelected != null)
            {
                onTabDeSelected.Invoke();
            }
        }
    }
}

