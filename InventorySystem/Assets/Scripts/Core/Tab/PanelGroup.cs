using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ryu
{
    public class PanelGroup : MonoBehaviour
    {
        public GameObject[] panels;

        //public TabGroup tabGroup;

        public int panelIndex;

        
        void ShowCurrentPanel()
        {
            for(int i =0; i< panels.Length; i++)
            {
                if(i == panelIndex)
                {
                    panels[i].gameObject.SetActive(true);
                    scrollRect.content = panels[i].GetComponent<RectTransform>();
                }
                else
                {
                    panels[i].gameObject.SetActive(false);
                }
            }
        }

        public void SetPageIndex(int index)
        {
            panelIndex = index;
            ShowCurrentPanel();
        }

        // 스크롤 뷰 때문에 적용
        private ScrollRect scrollRect;
        private void Awake()
        {
            scrollRect = this.GetComponent<ScrollRect>();
        }
    }
}

