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
        {// Ű ��� ������ return;
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
        if (m_keyCode == VirtualKeyCode.CAPITAL) // caps lock ��ư
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
        if (m_keyCode == VirtualKeyCode.SHIFT) // shift ��ư
        {
            KeyTable.Data.isShift = true;
        }
        if (m_keyCode >= VirtualKeyCode.VK_1 && m_keyCode <= VirtualKeyCode.VK_0)
        {// ����Ű 
            // ���� �����ϱ�
        }
        if (m_keyCode >= VirtualKeyCode.VK_A && m_keyCode <= VirtualKeyCode.VK_Z)
        {// ���� Ű 
            if (KeyTable.Data.isKorean)
            {// �ѱ�
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
            {// ����
                // ���� �����ϱ�
            }
        }
        // �ٸ� Ư�� Ű �����ϱ�
        
        
    }
}
