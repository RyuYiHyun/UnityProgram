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
        char key = data.DefaultKey;
        if(m_keyCode == VirtualKeyCode.HANGUL)//�ѿ�
        {
            KeyTable.Data.isKorean = !KeyTable.Data.isKorean;
            KeyTable.Data.TransData();
            return;
        }
        else if(m_keyCode == VirtualKeyCode.SPACE)// space
        {
            KeyTable.Data.SpaceData();
            return;
        }
        else if (m_keyCode == VirtualKeyCode.BACK) // back space
        {
            KeyTable.Data.DeleteData();
            return;
        }
        else if (m_keyCode == VirtualKeyCode.CAPITAL) // caps lock ��ư
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
            return;
        }
        else if (m_keyCode == VirtualKeyCode.SHIFT) // shift ��ư
        {
            KeyTable.Data.isShift = true;
            return;
        }
        else if (m_keyCode >= VirtualKeyCode.VK_1 && m_keyCode <= VirtualKeyCode.VK_0)
        {// ����Ű 
            if(KeyTable.Data.isShift)
            {
                key = string.IsNullOrWhiteSpace(data.ShiftKey.ToString()) ? key : data.ShiftKey;
            }
            KeyTable.Data.InputData(key);
            if (!KeyTable.Data.isCaps)
            {
                KeyTable.Data.isShift = false;
            }
            return;
        }
        else if (m_keyCode >= VirtualKeyCode.VK_A && m_keyCode <= VirtualKeyCode.VK_Z)
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
                if(KeyTable.Data.isShift)
                {
                    key = char.ToUpper(key);
                }
                else
                {
                    key = char.ToLower(key);
                }
                KeyTable.Data.InputData(key);
                if (!KeyTable.Data.isCaps)
                {
                    KeyTable.Data.isShift = false;
                }
                return;
            }
        }
        else if(m_keyCode == VirtualKeyCode.RETURN)
        {
            KeyTable.Data.EnterData();
            if (!KeyTable.Data.isCaps)
            {
                KeyTable.Data.isShift = false;
            }
        }
        else if (m_keyCode == VirtualKeyCode.TAB)
        {
            KeyTable.Data.TabData();
            if (!KeyTable.Data.isCaps)
            {
                KeyTable.Data.isShift = false;
            }
        }
        else
        {
            if(data.DefaultKey != ' ')
            {
                // �ٸ� Ư�� Ű �����ϱ�
                if (KeyTable.Data.isShift)
                {
                    key = string.IsNullOrWhiteSpace(data.ShiftKey.ToString()) ? key : data.ShiftKey;
                }
                KeyTable.Data.InputData(key);
                if (!KeyTable.Data.isCaps)
                {
                    KeyTable.Data.isShift = false;
                }
            }
        }
    }
}
