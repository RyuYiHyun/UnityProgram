using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//InventoryManager
namespace Ryu
{
    public class TabGroup : MonoBehaviour
    {
        public bool isTabActve;
        public int activeIndex = 0;
        public List<TabButton> tabButtons;

        public Color tabIdle;
        public Color tabHover;
        public Color tabActive;

        public Color textIdle;
        public Color textHover;
        public Color textActive;

        public TabButton selectedTab;
        
        public PanelGroup panelGroup;
        


        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }
            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                button.background.color = tabHover;
                button.text.color = textHover;
            }
        }
        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }
        public void OnTabSelected(TabButton button, int index)
        {
            if (selectedTab != null)
            {
                selectedTab.DeSelect();
            }
            selectedTab = button;

            selectedTab.Select();

            ResetTabs();
            button.background.color = tabActive;
            button.text.color = textActive;
            activeIndex = index;

            if (panelGroup != null)
            {
                panelGroup.SetPageIndex(activeIndex);
            }
        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                if (selectedTab != null && button == selectedTab) { continue; }
                button.background.color = tabIdle;
                button.text.color = textIdle;
            }
        }

        private void Update()
        {
            if (isTabActve)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("input Q");
                    activeIndex--;
                    if (activeIndex < 0)
                    {
                        activeIndex = 0;
                    }
                    OnTabSelected(tabButtons[activeIndex], activeIndex);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("input E");
                    activeIndex++;
                    if (activeIndex > (tabButtons.Count - 1))
                    {
                        activeIndex = tabButtons.Count - 1;
                    }
                    OnTabSelected(tabButtons[activeIndex], activeIndex);
                }
            }
        }
    }
}

