using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.UI;
using UnityEngine.Windows;


public class KeyData
{
    public char DefaultKey;
    public char ShiftKey;
    public char KorKey;
    public char KorShiftKey;
}
public class KeyTable : MonoBehaviour
{
    public static KeyTable Data = null;
    private void Awake()
    {
        if (Data == null) //instance�� null. ��, �ý��ۻ� �����ϰ� ���� ������
        {
            Data = this; //���ڽ��� instance�� �־��ݴϴ�.
            DontDestroyOnLoad(gameObject); //OnLoad(���� �ε� �Ǿ�����) �ڽ��� �ı����� �ʰ� ����
        }
        else
        {
            if (Data != this) //instance�� ���� �ƴ϶�� �̹� instance�� �ϳ� �����ϰ� �ִٴ� �ǹ�
            {
                Destroy(this.gameObject); //�� �̻� �����ϸ� �ȵǴ� ��ü�̴� ��� AWake�� �ڽ��� ����
            }
        }
    }
    private void Start()
    {
        MappingKeys();
    }

    public Text edittext;

    public Dictionary<VirtualKeyCode, KeyData> DicKeyData;
    public bool isKorean;
    public bool isShift;
    public bool isCaps;
    public string completeStr = "";
    public string makeStr = "";


    #region Hangul
    public enum HANGUL_TYPE
    {
        CHO, JUNG, JONG
    }

    // �ʼ�INDEX
    public char[] cho = {
          '��', '��', '��', '��', '��',
          '��', '��', '��', '��', '��', '��',
          '��', '��', '��', '��', '��', '��', '��', '��' };
    const int cho_count = 19;
    // �߼�INDEX
    public char[] jung = {
          '��', '��', '��', '��', '��',
          '��', '��', '��', '��', '��', '��',
          '��', '��', '��', '��', '��', '��',
          '��', '��', '��', '��'};
    const int jung_count = 21;
    // ����INDEX
    public char[] jong = {
          ' ', '��', '��', '��', '��', '��', '��', '��', '��',
          '��', '��', '��', '��', '��', '��', '��', '��',
          '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��'};
    const int jong_count = 28;
    // ����INDEX
    public char[] JungComb = { '��', '��', '��', '��', '��', '��', '��' };
    public string[] JungSplit = { "�Ǥ�", "�Ǥ�", "�Ǥ�", "�̤�", "�̤�", "�̤�", "�Ѥ�" };
    public char[] JongComb = { '��', '��', '��', '��', '��', '��', '��', '��', '��', '��', '��' };
    public string[] JongSplit = { "����", "����", "����", "����", "����", "����", "����", "����", "����", "����", "��" };


    public const int BASE_MOD = 0xAC00;        // ó�� �� (��)


    public const int START_WORD = 0xAC00;        // �������� MIN(��)
    public const int LAST_WORD = 0xD7A3;        // �������� MAX(�R)

    public const int START_CHO = 0x1100;        // �������� MIN(��)
    public const int LAST_CHO = 0x1112;        // �������� MAX(��)

    public const int START_JUNG = 0x1161;        // �������� MIN(��)
    public const int LAST_JUNG = 0x1175;        // �������� MAX(��)

    public const int START_JONG = 0x11A8;        // �������� MIN(��)
    public const int LAST_JONG = 0x11C3;        // �������� MAX(��)

    public int FindIndex(char _char, HANGUL_TYPE _type)
    {
        int index = 0;
        char[] hanguls = cho;
        switch (_type)
        {
            case HANGUL_TYPE.CHO:
                hanguls = cho;
                break;
            case HANGUL_TYPE.JUNG:
                hanguls = jung;
                break;
            case HANGUL_TYPE.JONG:
                hanguls = jong;
                break;
        }
        foreach (char element in hanguls)
        {
            if (element == _char)
            {
                return index;
            }
            ++index;
        }
        return -1;
    }
    public char FindChar(int index, HANGUL_TYPE _type)
    {
        char[] hanguls = cho;
        switch (_type)
        {
            case HANGUL_TYPE.CHO:
                hanguls = cho;
                break;
            case HANGUL_TYPE.JUNG:
                hanguls = jung;
                break;
            case HANGUL_TYPE.JONG:
                hanguls = jong;
                break;
        }
        return hanguls[index];
    }

    public char Combine_Hangul(char _cho, char _jung, char _jong = ' ')
    {
        return Convert.ToChar(BASE_MOD +
            FindIndex(_cho, HANGUL_TYPE.CHO) * jung_count * jong_count +
            FindIndex(_jung, HANGUL_TYPE.JUNG) * jong_count +
            FindIndex(_jong, HANGUL_TYPE.JONG));
    }
    public char Combine_Hangul_Jung(char _jungFirst, char _jungSecond)
    {
        int index = 0;
        foreach (string element in JungSplit)
        {
            if (element[0] == _jungFirst && element[1] == _jungSecond)
            {
                return JungComb[index];
            }
            ++index;
        }
        return ' ';
    }
    public char Combine_Hangul_Jong(char _jongFirst, char _jongSecond)
    {
        int index = 0;
        foreach (string element in JongSplit)
        {
            if (element[0] == _jongFirst && element[1] == _jongSecond)
            {
                return JongComb[index];
            }
            ++index;
        }
        return ' ';
    }

    public void Split_Hangul(char word, out char _cho, out char _jung, out char _jong)
    {
        int wordCode = Convert.ToInt32(word);
        wordCode -= BASE_MOD;
        int choCode = (wordCode / 28) / 21;
        _cho = cho[choCode];
        int jungCode = (wordCode / 28) % 21;
        _jung = jung[jungCode];
        int jongCode = wordCode % 28;
        _jong = jong[jongCode];
        return;
    }
    public bool Split_Hangul_Jung(char _jung, out char _jungFirst, out char _jungSecond)
    {
        int index = 0;
        foreach (char element in JungComb)
        {
            if (_jung == element) // �߼� ���� ���� ��
            {
                _jungFirst = JungSplit[index][0];
                _jungSecond = JungSplit[index][1];
                return true;
            }
            ++index;
        }
        // �߼� ���� �Ұ���
        _jungFirst = _jung;
        _jungSecond = ' ';
        return false;
    }
    public bool Split_Hangul_Jong(char _jong, out char _jongFirst, out char _jongSecond)
    {
        int index = 0;
        foreach (char element in JongComb)
        {
            if (_jong == element) // ���� ���� ���� ��
            {
                _jongFirst = JongSplit[index][0];
                _jongSecond = JongSplit[index][1];
                return true;
            }
            ++index;
        }
        // ���� ���� �Ұ���
        _jongFirst = _jong;
        _jongSecond = ' ';
        return false;
    }

    public void InputKRData(char _input)
    {
        // �Է��� �ʼ��� �߼��� �Է� ����
        int ischo = FindIndex(_input, HANGUL_TYPE.CHO);
        int isjung = FindIndex(_input, HANGUL_TYPE.JUNG);
        if (ischo != -1)// �ʼ� �Է� ex) ��
        {
            if (makeStr == "") // �ƹ� �Է��� ������
            {
                makeStr += _input;
                edittext.text = completeStr + makeStr; // �ӽ�
                return;
            }
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)
            {// ������ �ʼ��� ���� ex) ��  -> ����  �Ǵ� ��
                char result = Combine_Hangul_Jong(Convert.ToChar(makeStr), _input);
                if (result == ' ')
                { // �ռ� �Ұ���
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // �ӽ�
                    return;
                }
                else
                {
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; // �ӽ�
                    return;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)
            {// ������ �߼��� ���� ex) �� -> ����
                completeStr += makeStr;
                edittext.text = completeStr;
                makeStr = "";
                makeStr += _input;
                edittext.text = completeStr + makeStr; // �ӽ�
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1)
            {// ������ ������ ���� ex) �� -> ����  �ռ� ������ ������ �ʼ� ���̽����� �ɷ���
                completeStr += makeStr;
                edittext.text = completeStr;
                makeStr = "";
                makeStr += _input;
                edittext.text = completeStr + makeStr; // �ӽ�
            }
            else
            {
                char _cho;
                char _jung;
                bool isjungSplit = false;
                char _jung1;// �߼� ���� 1
                char _jung2;// �߼� ���� 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// ���� ���� 1
                char _jong2;// ���� ���� 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (isjongSplit)
                {
                    // �ʼ� + �߼� + <���� + ����> -> ex) �B -> �B�� (�Ұ��� �ռ�) -> ������ ���ذ� ����  isjongSplit = true
                    // �ʼ� + <�߼� + �߼�> + <���� + ����> -> ex) �� -> �� (�Ұ��� �ռ�) -> ������ ���ذ� ����  isjongSplit = true
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // �ӽ�
                    return;
                }
                else
                {
                    if (_jong == ' ')
                    {
                        // �ʼ� + �߼� -> ex) �� -> �� (������ �ռ�)
                        // �ʼ� + �߼� + �߼� -> ex) �� -> ��  (������ �ռ�)
                        char result = Combine_Hangul(_cho, _jung, _input); // �ռ�
                        makeStr = "";
                        makeStr += result;
                        edittext.text = completeStr + makeStr; // �ӽ�
                        return;
                    }
                    // �ʼ� + �߼� + ���� -> ex) �� -> ���� �Ǵ� �B   (���Ǻ� �ռ�) -> ������ �ռ� ���� ���� Ȯ�� , isjongSplit = false
                    // �ʼ� + <�߼� + �߼�> + ���� -> ex) �� -> ����  �Ǵ� �� (���Ǻ� �ռ�) -> ������ �ռ� ���� ���� Ȯ�� , isjongSplit = false
                    char combineJong = Combine_Hangul_Jong(_jong, _input);
                    if (combineJong != ' ')
                    {// �ռ� ����
                        char result = Combine_Hangul(_cho, _jung, combineJong); // �ռ�
                        makeStr = "";
                        makeStr += result;
                        edittext.text = completeStr + makeStr; // �ӽ�
                        return;
                    }
                    else
                    {// �ռ� �Ұ���
                        completeStr += makeStr;
                        edittext.text = completeStr;
                        makeStr = "";
                        makeStr += _input;
                        edittext.text = completeStr + makeStr; // �ӽ�
                        return;
                    }
                }
            }
        }
        if (isjung != -1)// �߼� �Է�
        {
            if (makeStr == "") // �ƹ� �Է��� ������
            {
                makeStr += _input;
                edittext.text = completeStr + makeStr; // �ӽ�
                return;
            }
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)
            {// ������ �ʼ��� ���� ex) ��  -> �� (������ �ռ� ����)
                char result = Combine_Hangul(Convert.ToChar(makeStr), _input);
                makeStr = "";
                makeStr += result;
                edittext.text = completeStr + makeStr; // �ӽ�
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)
            {// ������ �߼��� ���� ex) �� -> ����  �Ǵ� ��
                char _jung = Convert.ToChar(makeStr);
                char result = Combine_Hangul_Jung(_jung, _input);
                if (result != ' ')
                {// �ռ� ����
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; // �ӽ�
                    return;
                }
                else
                {// �ռ� �Ұ���
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // �ӽ�
                    return;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1)
            {// ������ ������ ���� ex) �� -> ����  �������� �ʼ� ���̽����� �ɷ���
                char _jong = Convert.ToChar(makeStr);
                bool isjongSplit = false;
                char _jong1;// ���� ���� 1
                char _jong2;// ���� ���� 2
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);
                if (isjongSplit)
                {
                    makeStr = "";
                    makeStr += _jong1;
                    completeStr += makeStr;
                    char result = Combine_Hangul(_jong2, _input);
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; // �ӽ�
                    return;
                }
            }
            else
            {
                char _cho;
                char _jung;
                bool isjungSplit = false;
                char _jung1;// �߼� ���� 1
                char _jung2;// �߼� ���� 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// ���� ���� 1
                char _jong2;// ���� ���� 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (isjongSplit)
                {
                    // �ʼ� + �߼� + <���� + ����> -> ex) �B -> ���� (������ �ռ� ����) 
                    // �ʼ� + <�߼� + �߼�> + <���� + ����> -> ex) ��-> ���� (������ �ռ� ����) 
                    char result1 = Combine_Hangul(_cho, _jung, _jong1);
                    char result2 = Combine_Hangul(_jong2, _input);
                    completeStr += result1;
                    makeStr = "";
                    makeStr += result2;
                    edittext.text = completeStr + makeStr; // �ӽ�
                    return;
                }
                else
                {
                    if (_jong != ' ') // ������ ����
                    {// �ʼ� + �߼� + ���� -> ex) �� -> ���� (������ �ռ� ����) 
                        char result1 = Combine_Hangul(_cho, _jung);
                        char result2 = Combine_Hangul(_jong, _input);
                        completeStr += result1;
                        makeStr = "";
                        makeStr += result2;
                        edittext.text = completeStr + makeStr; // �ӽ�
                        return;
                    }
                    else
                    {
                        if (isjungSplit) // �߼��� ���ذ� �Ǵ���
                        {// �ʼ� + <�߼� + �߼�> -> ex) �� -> ����    (�Ұ��� �ռ�)
                            completeStr += makeStr;
                            edittext.text = completeStr;
                            makeStr = "";
                            makeStr += _input;
                            edittext.text = completeStr + makeStr; // �ӽ�
                        }
                        else
                        {// �ʼ� + �߼� -> ex) �� -> �� �Ǵ� ���  (���Ǻ� �ռ�)
                            char result = Combine_Hangul_Jung(_jung, _input);
                            if (result != ' ')
                            {// �ռ� ����
                                char combine = Combine_Hangul(_cho, result);
                                makeStr = "";
                                makeStr += combine;
                                edittext.text = completeStr + makeStr; // �ӽ�
                                return;
                            }
                            else
                            {// �ռ� �Ұ���
                                completeStr += makeStr;
                                edittext.text = completeStr;
                                makeStr = "";
                                makeStr += _input;
                                edittext.text = completeStr + makeStr; // �ӽ�
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    public void DeleteData()
    {
        if (makeStr == "") // ���� ���ڰ� ������
        {
            if (completeStr != null)
            {
                if (completeStr.Length > 0)
                {
                    int n = completeStr.LastIndexOf("\n");
                    if (n == -1)
                    {
                        completeStr = completeStr.Remove(completeStr.Length - 1);
                        edittext.text = completeStr;
                        return;
                    }
                    else
                    {
                        completeStr = completeStr.Remove(completeStr.Length - 2);
                        edittext.text = completeStr;
                        return;
                    }
                }
            }
        }
        else
        {
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)// �ʼ�
            {
                makeStr = "";
                edittext.text = completeStr + makeStr;
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)// �߼�
            {
                char jung1;
                char jung2;
                if (Split_Hangul_Jung(Convert.ToChar(makeStr), out jung1, out jung2))
                {// ���� ����
                    makeStr = "";
                    makeStr += jung1;
                    edittext.text = completeStr + makeStr;
                    return;
                }
                else
                {//���� �ȵ�
                    makeStr = "";
                    edittext.text = completeStr + makeStr;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1) // ����
            {
                char jong1;
                char jong2;
                if (Split_Hangul_Jong(Convert.ToChar(makeStr), out jong1, out jong2))
                {// ���� ����
                    makeStr = "";
                    makeStr += jong1;
                    edittext.text = completeStr + makeStr;
                    return;
                }
                else
                {//���� �ȵ�
                    makeStr = "";
                    edittext.text = completeStr + makeStr;
                    return;
                }
            }
            else
            {
                char _cho;
                char _jung;
                bool isjungSplit = false;
                char _jung1;// �߼� ���� 1
                char _jung2;// �߼� ���� 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// ���� ���� 1
                char _jong2;// ���� ���� 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (_jong != ' ') // ���� ����
                {
                    if (isjongSplit) // ���� ���� ����
                    {//�B -> ��
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung, _jong1);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                    else
                    {//�� -> ��
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                }
                else
                {
                    if (isjungSplit)// �߼� ���� ����
                    {//�� -> ��
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung1);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                    else
                    {// �� -> ��
                        makeStr = "";
                        makeStr += _cho;
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                }
            }
        }
    }

    public void SpaceData()
    {
        if (makeStr == "") // ���� ���ڰ� ������
        {
            completeStr += " ";
            edittext.text = completeStr;
        }
        else
        {
            completeStr += makeStr;
            completeStr += " ";
            makeStr = "";
            edittext.text = completeStr;
        }
    }

    #region Private Method
    private void MappingKeys()
    {
        DicKeyData = new Dictionary<VirtualKeyCode, KeyData>();
        DicKeyData.Add(VirtualKeyCode.VK_1, new KeyData { DefaultKey = '1', ShiftKey = '!' });
        DicKeyData.Add(VirtualKeyCode.VK_2, new KeyData { DefaultKey = '2', ShiftKey = '@' });
        DicKeyData.Add(VirtualKeyCode.VK_3, new KeyData { DefaultKey = '3', ShiftKey = '#' });
        DicKeyData.Add(VirtualKeyCode.VK_4, new KeyData { DefaultKey = '4', ShiftKey = '$' });
        DicKeyData.Add(VirtualKeyCode.VK_5, new KeyData { DefaultKey = '5', ShiftKey = '%' });
        DicKeyData.Add(VirtualKeyCode.VK_6, new KeyData { DefaultKey = '6', ShiftKey = '^' });
        DicKeyData.Add(VirtualKeyCode.VK_7, new KeyData { DefaultKey = '7', ShiftKey = '&' });
        DicKeyData.Add(VirtualKeyCode.VK_8, new KeyData { DefaultKey = '8', ShiftKey = '*' });
        DicKeyData.Add(VirtualKeyCode.VK_9, new KeyData { DefaultKey = '9', ShiftKey = '(' });
        DicKeyData.Add(VirtualKeyCode.VK_0, new KeyData { DefaultKey = '0', ShiftKey = ')' });

        DicKeyData.Add(VirtualKeyCode.VK_A, new KeyData { DefaultKey = 'a', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_B, new KeyData { DefaultKey = 'b', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_C, new KeyData { DefaultKey = 'c', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_D, new KeyData { DefaultKey = 'd', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_E, new KeyData { DefaultKey = 'e', KorKey = '��', KorShiftKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_F, new KeyData { DefaultKey = 'f', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_G, new KeyData { DefaultKey = 'g', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_H, new KeyData { DefaultKey = 'h', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_I, new KeyData { DefaultKey = 'i', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_J, new KeyData { DefaultKey = 'j', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_K, new KeyData { DefaultKey = 'k', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_L, new KeyData { DefaultKey = 'l', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_M, new KeyData { DefaultKey = 'm', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_N, new KeyData { DefaultKey = 'n', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_O, new KeyData { DefaultKey = 'o', KorKey = '��', KorShiftKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_P, new KeyData { DefaultKey = 'p', KorKey = '��', KorShiftKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_Q, new KeyData { DefaultKey = 'q', KorKey = '��', KorShiftKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_R, new KeyData { DefaultKey = 'r', KorKey = '��', KorShiftKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_S, new KeyData { DefaultKey = 's', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_T, new KeyData { DefaultKey = 't', KorKey = '��', KorShiftKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_U, new KeyData { DefaultKey = 'u', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_V, new KeyData { DefaultKey = 'v', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_W, new KeyData { DefaultKey = 'w', KorKey = '��', KorShiftKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_X, new KeyData { DefaultKey = 'x', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_Y, new KeyData { DefaultKey = 'y', KorKey = '��' });
        DicKeyData.Add(VirtualKeyCode.VK_Z, new KeyData { DefaultKey = 'z', KorKey = '��' });

        DicKeyData.Add(VirtualKeyCode.OEM_3, new KeyData { DefaultKey = '`', ShiftKey = '~' });
        DicKeyData.Add(VirtualKeyCode.OEM_MINUS, new KeyData { DefaultKey = '-', ShiftKey = '_' });
        DicKeyData.Add(VirtualKeyCode.OEM_PLUS, new KeyData { DefaultKey = '=', ShiftKey = '+' });
        DicKeyData.Add(VirtualKeyCode.BACK, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.TAB, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.OEM_4, new KeyData { DefaultKey = '[', ShiftKey = '{' });
        DicKeyData.Add(VirtualKeyCode.OEM_6, new KeyData { DefaultKey = ']', ShiftKey = '}' });
        DicKeyData.Add(VirtualKeyCode.OEM_5, new KeyData { DefaultKey = '��', ShiftKey = '|' });
        DicKeyData.Add(VirtualKeyCode.CAPITAL, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.OEM_1, new KeyData { DefaultKey = ';', ShiftKey = ':' });
        DicKeyData.Add(VirtualKeyCode.OEM_7, new KeyData { DefaultKey = '\'', ShiftKey = '\"' });
        DicKeyData.Add(VirtualKeyCode.RETURN, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.SHIFT, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.OEM_COMMA, new KeyData { DefaultKey = ',', ShiftKey = '<' });
        DicKeyData.Add(VirtualKeyCode.OEM_PERIOD, new KeyData { DefaultKey = '.', ShiftKey = '>' });
        DicKeyData.Add(VirtualKeyCode.OEM_2, new KeyData { DefaultKey = '/', ShiftKey = '?' });
        DicKeyData.Add(VirtualKeyCode.HANGUL, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.SPACE, new KeyData { DefaultKey = ' ' });
    }
    #endregion


}


public enum VirtualKeyCode
{
    //NumKey
    VK_1,
    VK_2,
    VK_3,
    VK_4,
    VK_5,
    VK_6,
    VK_7,
    VK_8,
    VK_9,
    VK_0,
    //CharKey
    VK_A,
    VK_B,
    VK_C,
    VK_D,
    VK_E,
    VK_F,
    VK_G,
    VK_H,
    VK_I,
    VK_J,
    VK_K,
    VK_L,
    VK_M,
    VK_N,
    VK_O,
    VK_P,
    VK_Q,
    VK_R,
    VK_S,
    VK_T,
    VK_U,
    VK_V,
    VK_W,
    VK_X,
    VK_Y,
    VK_Z,
    //
    OEM_3,
    OEM_MINUS,
    OEM_PLUS,
    BACK, TAB,
    OEM_4,
    OEM_6,
    OEM_5,
    CAPITAL,
    OEM_1,
    OEM_7,
    RETURN,
    SHIFT,
    OEM_COMMA,
    OEM_PERIOD,
    OEM_2,
    HANGUL,
    SPACE
}


//_dicKeyData.Add(VirtualKeyCode.VK_1, new KeyData { DefaultKey = "1", ShiftKey = "!" });
//_dicKeyData.Add(VirtualKeyCode.VK_2, new KeyData { DefaultKey = "2", ShiftKey = "@" });
//_dicKeyData.Add(VirtualKeyCode.VK_3, new KeyData { DefaultKey = "3", ShiftKey = "#" });
//_dicKeyData.Add(VirtualKeyCode.VK_4, new KeyData { DefaultKey = "4", ShiftKey = "$" });
//_dicKeyData.Add(VirtualKeyCode.VK_5, new KeyData { DefaultKey = "5", ShiftKey = "%" });
//_dicKeyData.Add(VirtualKeyCode.VK_6, new KeyData { DefaultKey = "6", ShiftKey = "^" });
//_dicKeyData.Add(VirtualKeyCode.VK_7, new KeyData { DefaultKey = "7", ShiftKey = "&" });
//_dicKeyData.Add(VirtualKeyCode.VK_8, new KeyData { DefaultKey = "8", ShiftKey = "*" });
//_dicKeyData.Add(VirtualKeyCode.VK_9, new KeyData { DefaultKey = "9", ShiftKey = "(" });
//_dicKeyData.Add(VirtualKeyCode.VK_0, new KeyData { DefaultKey = "0", ShiftKey = ")" });

//_dicKeyData.Add(VirtualKeyCode.VK_A, new KeyData { DefaultKey = "a", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_B, new KeyData { DefaultKey = "b", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_C, new KeyData { DefaultKey = "c", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_D, new KeyData { DefaultKey = "d", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_E, new KeyData { DefaultKey = "e", KorKey = "��", KorShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_F, new KeyData { DefaultKey = "f", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_G, new KeyData { DefaultKey = "g", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_H, new KeyData { DefaultKey = "h", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_I, new KeyData { DefaultKey = "i", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_J, new KeyData { DefaultKey = "j", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_K, new KeyData { DefaultKey = "k", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_L, new KeyData { DefaultKey = "l", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_M, new KeyData { DefaultKey = "m", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_N, new KeyData { DefaultKey = "n", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_O, new KeyData { DefaultKey = "o", KorKey = "��", KorShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_P, new KeyData { DefaultKey = "p", KorKey = "��", KorShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_Q, new KeyData { DefaultKey = "q", KorKey = "��", KorShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_R, new KeyData { DefaultKey = "r", KorKey = "��", KorShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_S, new KeyData { DefaultKey = "s", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_T, new KeyData { DefaultKey = "t", KorKey = "��", KorShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_U, new KeyData { DefaultKey = "u", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_V, new KeyData { DefaultKey = "v", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_W, new KeyData { DefaultKey = "w", KorKey = "��", KorShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_X, new KeyData { DefaultKey = "x", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_Y, new KeyData { DefaultKey = "y", KorKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.VK_Z, new KeyData { DefaultKey = "z", KorKey = "��" });

//_dicKeyData.Add(VirtualKeyCode.OEM_3, new KeyData { DefaultKey = "`", ShiftKey = "~" });
//_dicKeyData.Add(VirtualKeyCode.OEM_MINUS, new KeyData { DefaultKey = "-", ShiftKey = "_" });
//_dicKeyData.Add(VirtualKeyCode.OEM_PLUS, new KeyData { DefaultKey = "=", ShiftKey = "+" });
//_dicKeyData.Add(VirtualKeyCode.BACK, new KeyData { DefaultKey = "Backspace" });
//_dicKeyData.Add(VirtualKeyCode.TAB, new KeyData { DefaultKey = "Tab" });
//_dicKeyData.Add(VirtualKeyCode.OEM_4, new KeyData { DefaultKey = "[", ShiftKey = "{" });
//_dicKeyData.Add(VirtualKeyCode.OEM_6, new KeyData { DefaultKey = "]", ShiftKey = "}" });
//_dicKeyData.Add(VirtualKeyCode.OEM_5, new KeyData { DefaultKey = "��", ShiftKey = "|" });
//_dicKeyData.Add(VirtualKeyCode.CAPITAL, new KeyData { DefaultKey = "Caps Lock" });
//_dicKeyData.Add(VirtualKeyCode.OEM_1, new KeyData { DefaultKey = ";", ShiftKey = ":" });
//_dicKeyData.Add(VirtualKeyCode.OEM_7, new KeyData { DefaultKey = "'", ShiftKey = "��" });
//_dicKeyData.Add(VirtualKeyCode.RETURN, new KeyData { DefaultKey = "Enter" });
//_dicKeyData.Add(VirtualKeyCode.SHIFT, new KeyData { DefaultKey = "Shift" });
//_dicKeyData.Add(VirtualKeyCode.OEM_COMMA, new KeyData { DefaultKey = ",", ShiftKey = "<" });
//_dicKeyData.Add(VirtualKeyCode.OEM_PERIOD, new KeyData { DefaultKey = ".", ShiftKey = ">" });
//_dicKeyData.Add(VirtualKeyCode.OEM_2, new KeyData { DefaultKey = "/", ShiftKey = "?" });
//_dicKeyData.Add(VirtualKeyCode.HANGUL, new KeyData { DefaultKey = "��/��" });
//_dicKeyData.Add(VirtualKeyCode.SPACE, new KeyData { DefaultKey = "Space" });

//_dicKeyData.Add(VirtualKeyCode.NUMPAD0, new KeyData { DefaultKey = "0" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD1, new KeyData { DefaultKey = "1" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD2, new KeyData { DefaultKey = "2" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD3, new KeyData { DefaultKey = "3" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD4, new KeyData { DefaultKey = "4" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD5, new KeyData { DefaultKey = "5" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD6, new KeyData { DefaultKey = "6" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD7, new KeyData { DefaultKey = "7" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD8, new KeyData { DefaultKey = "8" });
//_dicKeyData.Add(VirtualKeyCode.NUMPAD9, new KeyData { DefaultKey = "9" });