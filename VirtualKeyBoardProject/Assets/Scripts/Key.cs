using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Button m_key;
    public VirtualKeyCode m_keyCode;

    private void Start()
    {
        m_key = GetComponent<Button>();
        m_key.onClick.AddListener(KeyClick);
    }

    public void KeyClick()
    {
        if (!KeyTable.Data.DicKeyData.ContainsKey(m_keyCode))
        {// 키 등록 없으면 return;
            return;
        }
        KeyData data = KeyTable.Data.DicKeyData[m_keyCode];
        var key = data.DefaultKey;

        if(m_keyCode == VirtualKeyCode.SPACE)// space
        {
            KeyTable.Data.SpaceData();
        }
        if (m_keyCode == VirtualKeyCode.BACK) // back space
        {
            KeyTable.Data.DeleteData();
        }
        if (m_keyCode == VirtualKeyCode.CAPITAL) // caps lock 버튼
        {
            if (KeyTable.Data.isCaps)
            {
                KeyTable.Data.isShift = false;
                KeyTable.Data.isCaps = false;
            }
            else
            {
                KeyTable.Data.isShift = true;
                KeyTable.Data.isCaps = true;
            }
        }
        if (m_keyCode == VirtualKeyCode.SHIFT) // shift 버튼
        {
            KeyTable.Data.isShift = true;
        }
        if (m_keyCode >= VirtualKeyCode.VK_1 && m_keyCode <= VirtualKeyCode.VK_0)
        {// 숫자키 
            // 숫자 구현하기
        }
        if (m_keyCode >= VirtualKeyCode.VK_A && m_keyCode <= VirtualKeyCode.VK_Z)
        {// 문자 키 
            if (KeyTable.Data.isKorean)
            {// 한글
                key = data.KorKey;
                if (KeyTable.Data.isShift && (m_keyCode == VirtualKeyCode.VK_Q || m_keyCode == VirtualKeyCode.VK_W || m_keyCode == VirtualKeyCode.VK_E ||
                        m_keyCode == VirtualKeyCode.VK_R || m_keyCode == VirtualKeyCode.VK_T || m_keyCode == VirtualKeyCode.VK_O ||
                        m_keyCode == VirtualKeyCode.VK_P))
                {
                    key = data.KorShiftKey;
                }
                KeyTable.Data.InputKRData(key);
                if (!KeyTable.Data.isCaps)
                {
                    KeyTable.Data.isShift = false;
                }
                return;
            }
            else
            {// 영어
                // 영어 구현하기
            }
        }
        // 다른 특수 키 구현하기
        
        
    }
}
