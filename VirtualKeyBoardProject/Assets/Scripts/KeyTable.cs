using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        Screen.SetResolution(1536, 864, false);


        if (Data == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            Data = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (Data != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
            {
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
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
    private string completeStr = "";
    private string makeStr = "";


    #region Hangul
    private enum HANGUL_TYPE
    {
        CHO, JUNG, JONG
    }

    // 초성INDEX
    private char[] cho = {
          'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ',
          'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ',
          'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    const int cho_count = 19;
    // 중성INDEX
    private char[] jung = {
          'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ',
          'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ',
          'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ',
          'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ'};
    const int jung_count = 21;
    // 종성INDEX
    private char[] jong = {
          ' ', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ',
          'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ',
          'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'};
    const int jong_count = 28;
    // 조합INDEX
    private char[] JungComb = { 'ㅘ', 'ㅙ', 'ㅚ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅢ' };
    private string[] JungSplit = { "ㅗㅏ", "ㅗㅐ", "ㅗㅣ", "ㅜㅓ", "ㅜㅔ", "ㅜㅣ", "ㅡㅣ" };
    private char[] JongComb = { 'ㄳ', 'ㄵ', 'ㄶ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅄ'};
    private string[] JongSplit = { "ㄱㅅ", "ㄴㅈ", "ㄴㅎ", "ㄹㄱ", "ㄹㅁ", "ㄹㅂ", "ㄹㅅ", "ㄹㅌ", "ㄹㅍ", "ㄹㅎ", "ㅂㅅ"};


    public const int BASE_MOD = 0xAC00;        // 처음 값 (가)


    public const int START_WORD = 0xAC00;        // 음성범위 MIN(가)
    public const int LAST_WORD = 0xD7A3;        // 음성범위 MAX(힣)

    public const int START_CHO = 0x1100;        // 음성범위 MIN(ㄱ)
    public const int LAST_CHO = 0x1112;        // 음성범위 MAX(ㅎ)

    public const int START_JUNG = 0x1161;        // 음성범위 MIN(ㅏ)
    public const int LAST_JUNG = 0x1175;        // 음성범위 MAX(ㅣ)

    public const int START_JONG = 0x11A8;        // 음성범위 MIN(ㄱ)
    public const int LAST_JONG = 0x11C3;        // 음성범위 MAX(ㅎ)

    private int FindIndex(char _char, HANGUL_TYPE _type)
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
    private char FindChar(int index, HANGUL_TYPE _type)
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
            if (_jung == element) // 중성 분해 가능 함
            {
                _jungFirst = JungSplit[index][0];
                _jungSecond = JungSplit[index][1];
                return true;
            }
            ++index;
        }
        // 중성 분해 불가능
        _jungFirst = _jung;
        _jungSecond = ' ';
        return false;
    }
    public bool Split_Hangul_Jong(char _jong, out char _jongFirst, out char _jongSecond)
    {
        int index = 0;
        foreach (char element in JongComb)
        {
            if (_jong == element) // 종성 분해 가능 함
            {
                _jongFirst = JongSplit[index][0];
                _jongSecond = JongSplit[index][1];
                return true;
            }
            ++index;
        }
        // 종성 분해 불가능
        _jongFirst = _jong;
        _jongSecond = ' ';
        return false;
    }

    public void InputKRData(char _input)
    {
        // 입력은 초성과 중성만 입력 가능
        int ischo = FindIndex(_input, HANGUL_TYPE.CHO);
        int isjung = FindIndex(_input, HANGUL_TYPE.JUNG);
        if (ischo != -1)// 초성 입력 ex) ㄱ
        {
            if (makeStr == "") // 아무 입력이 없었음
            {
                makeStr += _input;
                edittext.text = completeStr + makeStr; // 임시
                return;
            }
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)
            {// 기존에 초성이 있음 ex) ㄱ  -> ㄱㄱ  또는 ㄳ
                char result = Combine_Hangul_Jong(Convert.ToChar(makeStr), _input);
                if (result == ' ')
                { // 합성 불가능
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // 임시
                    return;
                }
                else
                {
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; // 임시
                    return;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)
            {// 기존에 중성이 있음 ex) ㅏ -> ㅏㄱ
                completeStr += makeStr;
                edittext.text = completeStr;
                makeStr = "";
                makeStr += _input;
                edittext.text = completeStr + makeStr; // 임시
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1)
            {// 기존에 종성이 있음 ex) ㄳ -> ㄳㄱ  합성 가능한 종성은 초성 케이스에서 걸러짐
                completeStr += makeStr;
                edittext.text = completeStr;
                makeStr = "";
                makeStr += _input;
                edittext.text = completeStr + makeStr; // 임시
            }
            else
            {
                char _cho;
                char _jung;
                bool isjungSplit = false;
                char _jung1;// 중성 분해 1
                char _jung2;// 중성 분해 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// 종성 분해 1
                char _jong2;// 종성 분해 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (isjongSplit)
                {
                    // 초성 + 중성 + <종성 + 종성> -> ex) 갃 -> 갃ㄱ (불가능 합성) -> 종성의 분해가 가능  isjongSplit = true
                    // 초성 + <중성 + 중성> + <종성 + 종성> -> ex) 곿 -> 곿ㄱ (불가능 합성) -> 종성의 분해가 가능  isjongSplit = true
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // 임시
                    return;
                }
                else
                {
                    if (_jong == ' ')
                    {
                        // 초성 + 중성 -> ex) 가 -> 각 (무조건 합성)
                        // 초성 + 중성 + 중성 -> ex) 과 -> 곽  (무조건 합성)
                        char result = Combine_Hangul(_cho, _jung, _input); // 합성
                        makeStr = "";
                        makeStr += result;
                        edittext.text = completeStr + makeStr; // 임시
                        return;
                    }
                    // 초성 + 중성 + 종성 -> ex) 각 -> 각ㄱ 또는 갃   (조건부 합성) -> 종성의 합성 가능 여부 확인 , isjongSplit = false
                    // 초성 + <중성 + 중성> + 종성 -> ex) 곽 -> 곽ㄱ  또는 곿 (조건부 합성) -> 종성의 합성 가능 여부 확인 , isjongSplit = false
                    char combineJong = Combine_Hangul_Jong(_jong, _input);
                    if (combineJong != ' ')
                    {// 합성 가능
                        char result = Combine_Hangul(_cho, _jung, combineJong); // 합성
                        makeStr = "";
                        makeStr += result;
                        edittext.text = completeStr + makeStr; // 임시
                        return;
                    }
                    else
                    {// 합성 불가능
                        completeStr += makeStr;
                        edittext.text = completeStr;
                        makeStr = "";
                        makeStr += _input;
                        edittext.text = completeStr + makeStr; // 임시
                        return;
                    }
                }
            }
        }
        if (isjung != -1)// 중성 입력
        {
            if (makeStr == "") // 아무 입력이 없었음
            {
                makeStr += _input;
                edittext.text = completeStr + makeStr; 
                return;
            }
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)
            {// 기존에 초성이 있음 ex) ㄱ  -> 가 (무조건 합성 가능)
                char result = Combine_Hangul(Convert.ToChar(makeStr), _input);
                makeStr = "";
                makeStr += result;
                edittext.text = completeStr + makeStr;
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)
            {// 기존에 중성이 있음 ex) ㅏ -> ㅏㅏ  또는 ㅘ
                char _jung = Convert.ToChar(makeStr);
                char result = Combine_Hangul_Jung(_jung, _input);
                if (result != ' ')
                {// 합성 가능
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; 
                    return;
                }
                else
                {// 합성 불가능
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; 
                    return;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1)
            {// 기존에 종성이 있음 ex) ㄳ -> ㄱ사  나머지는 초성 케이스에서 걸러짐
                char _jong = Convert.ToChar(makeStr);
                bool isjongSplit = false;
                char _jong1;// 종성 분해 1
                char _jong2;// 종성 분해 2
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);
                if (isjongSplit)
                {
                    makeStr = "";
                    makeStr += _jong1;
                    completeStr += makeStr;
                    char result = Combine_Hangul(_jong2, _input);
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; 
                    return;
                }
            }
            else
            {
                char _cho;
                char _jung;
                bool isjungSplit = false;
                char _jung1;// 중성 분해 1
                char _jung2;// 중성 분해 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// 종성 분해 1
                char _jong2;// 종성 분해 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (isjongSplit)
                {
                    // 초성 + 중성 + <종성 + 종성> -> ex) 갃 -> 각사 (무조건 합성 가능) 
                    // 초성 + <중성 + 중성> + <종성 + 종성> -> ex) 곿-> 곽사 (무조건 합성 가능) 
                    char result1 = Combine_Hangul(_cho, _jung, _jong1);
                    char result2 = Combine_Hangul(_jong2, _input);
                    completeStr += result1;
                    makeStr = "";
                    makeStr += result2;
                    edittext.text = completeStr + makeStr;
                    return;
                }
                else
                {
                    if (_jong != ' ') // 종성이 있음
                    {// 초성 + 중성 + 종성 -> ex) 각 -> 가가 (무조건 합성 가능) 
                        char result1 = Combine_Hangul(_cho, _jung);
                        char result2 = Combine_Hangul(_jong, _input);
                        completeStr += result1;
                        makeStr = "";
                        makeStr += result2;
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                    else
                    {
                        if (isjungSplit) // 중성이 분해가 되는지
                        {// 초성 + <중성 + 중성> -> ex) 과 -> 과ㅏ    (불가능 합성)
                            completeStr += makeStr;
                            edittext.text = completeStr;
                            makeStr = "";
                            makeStr += _input;
                            edittext.text = completeStr + makeStr; 
                        }
                        else
                        {// 초성 + 중성 -> ex) 고 -> 과 또는 고ㅗ  (조건부 합성)
                            char result = Combine_Hangul_Jung(_jung, _input);
                            if (result != ' ')
                            {// 합성 가능
                                char combine = Combine_Hangul(_cho, result);
                                makeStr = "";
                                makeStr += combine;
                                edittext.text = completeStr + makeStr;
                                return;
                            }
                            else
                            {// 합성 불가능
                                completeStr += makeStr;
                                edittext.text = completeStr;
                                makeStr = "";
                                makeStr += _input;
                                edittext.text = completeStr + makeStr; 
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
    public void TransData()
    {
        if (makeStr == "") // 쓰던 문자가 없을때
        {
            edittext.text = completeStr;
        }
        else
        {
            completeStr += makeStr;
            makeStr = "";
            edittext.text = completeStr;
        }
    }
    public void InputData(char _input)
    {
        if (makeStr == "") // 쓰던 문자가 없을때
        {
            completeStr += _input;
            edittext.text = completeStr;
        }
        else
        {
            completeStr += makeStr;
            completeStr += _input;
            makeStr = "";
            edittext.text = completeStr;
        }
    }
    public void DeleteData()
    {
        if (makeStr == "") // 쓰던 문자가 없을때
        {
            if (completeStr != null)
            {
                if (completeStr.Length > 0)
                {
                    completeStr = completeStr.Remove(completeStr.Length - 1);
                    edittext.text = completeStr;
                    return;
                }
            }
        }
        else
        {
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)// 초성
            {
                makeStr = "";
                edittext.text = completeStr + makeStr;
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)// 중성
            {
                char jung1;
                char jung2;
                if (Split_Hangul_Jung(Convert.ToChar(makeStr), out jung1, out jung2))
                {// 분해 성공
                    makeStr = "";
                    makeStr += jung1;
                    edittext.text = completeStr + makeStr;
                    return;
                }
                else
                {//분해 안됨
                    makeStr = "";
                    edittext.text = completeStr + makeStr;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1) // 종성
            {
                char jong1;
                char jong2;
                if (Split_Hangul_Jong(Convert.ToChar(makeStr), out jong1, out jong2))
                {// 분해 성공
                    makeStr = "";
                    makeStr += jong1;
                    edittext.text = completeStr + makeStr;
                    return;
                }
                else
                {//분해 안됨
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
                char _jung1;// 중성 분해 1
                char _jung2;// 중성 분해 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// 종성 분해 1
                char _jong2;// 종성 분해 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (_jong != ' ') // 종성 여부
                {
                    if (isjongSplit) // 종성 분해 여부
                    {//갃 -> 각
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung, _jong1);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                    else
                    {//각 -> 가
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                }
                else
                {
                    if (isjungSplit)// 중성 분해 여부
                    {//과 -> 고
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung1);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                    else
                    {// 고 -> ㄱ
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
        if (makeStr == "") // 쓰던 문자가 없을때
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
    public void EnterData()
    {
        if (makeStr == "") // 쓰던 문자가 없을때
        {
            completeStr += "\n";
            edittext.text = completeStr;
        }
        else
        {
            completeStr += makeStr;
            completeStr += "\n";
            makeStr = "";
            edittext.text = completeStr;
        }
    }
    public void TabData()
    {
        if (makeStr == "") // 쓰던 문자가 없을때
        {
            completeStr += "\t";
            edittext.text = completeStr;
        }
        else
        {
            completeStr += makeStr;
            completeStr += "\t";
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

        DicKeyData.Add(VirtualKeyCode.VK_A, new KeyData { DefaultKey = 'a', KorKey = 'ㅁ' });
        DicKeyData.Add(VirtualKeyCode.VK_B, new KeyData { DefaultKey = 'b', KorKey = 'ㅠ' });
        DicKeyData.Add(VirtualKeyCode.VK_C, new KeyData { DefaultKey = 'c', KorKey = 'ㅊ' });
        DicKeyData.Add(VirtualKeyCode.VK_D, new KeyData { DefaultKey = 'd', KorKey = 'ㅇ' });
        DicKeyData.Add(VirtualKeyCode.VK_E, new KeyData { DefaultKey = 'e', KorKey = 'ㄷ', KorShiftKey = 'ㄸ' });
        DicKeyData.Add(VirtualKeyCode.VK_F, new KeyData { DefaultKey = 'f', KorKey = 'ㄹ' });
        DicKeyData.Add(VirtualKeyCode.VK_G, new KeyData { DefaultKey = 'g', KorKey = 'ㅎ' });
        DicKeyData.Add(VirtualKeyCode.VK_H, new KeyData { DefaultKey = 'h', KorKey = 'ㅗ' });
        DicKeyData.Add(VirtualKeyCode.VK_I, new KeyData { DefaultKey = 'i', KorKey = 'ㅑ' });
        DicKeyData.Add(VirtualKeyCode.VK_J, new KeyData { DefaultKey = 'j', KorKey = 'ㅓ' });
        DicKeyData.Add(VirtualKeyCode.VK_K, new KeyData { DefaultKey = 'k', KorKey = 'ㅏ' });
        DicKeyData.Add(VirtualKeyCode.VK_L, new KeyData { DefaultKey = 'l', KorKey = 'ㅣ' });
        DicKeyData.Add(VirtualKeyCode.VK_M, new KeyData { DefaultKey = 'm', KorKey = 'ㅡ' });
        DicKeyData.Add(VirtualKeyCode.VK_N, new KeyData { DefaultKey = 'n', KorKey = 'ㅜ' });
        DicKeyData.Add(VirtualKeyCode.VK_O, new KeyData { DefaultKey = 'o', KorKey = 'ㅐ', KorShiftKey = 'ㅒ' });
        DicKeyData.Add(VirtualKeyCode.VK_P, new KeyData { DefaultKey = 'p', KorKey = 'ㅔ', KorShiftKey = 'ㅖ' });
        DicKeyData.Add(VirtualKeyCode.VK_Q, new KeyData { DefaultKey = 'q', KorKey = 'ㅂ', KorShiftKey = 'ㅃ' });
        DicKeyData.Add(VirtualKeyCode.VK_R, new KeyData { DefaultKey = 'r', KorKey = 'ㄱ', KorShiftKey = 'ㄲ' });
        DicKeyData.Add(VirtualKeyCode.VK_S, new KeyData { DefaultKey = 's', KorKey = 'ㄴ' });
        DicKeyData.Add(VirtualKeyCode.VK_T, new KeyData { DefaultKey = 't', KorKey = 'ㅅ', KorShiftKey = 'ㅆ' });
        DicKeyData.Add(VirtualKeyCode.VK_U, new KeyData { DefaultKey = 'u', KorKey = 'ㅕ' });
        DicKeyData.Add(VirtualKeyCode.VK_V, new KeyData { DefaultKey = 'v', KorKey = 'ㅍ' });
        DicKeyData.Add(VirtualKeyCode.VK_W, new KeyData { DefaultKey = 'w', KorKey = 'ㅈ', KorShiftKey = 'ㅉ' });
        DicKeyData.Add(VirtualKeyCode.VK_X, new KeyData { DefaultKey = 'x', KorKey = 'ㅌ' });
        DicKeyData.Add(VirtualKeyCode.VK_Y, new KeyData { DefaultKey = 'y', KorKey = 'ㅛ' });
        DicKeyData.Add(VirtualKeyCode.VK_Z, new KeyData { DefaultKey = 'z', KorKey = 'ㅋ' });

        DicKeyData.Add(VirtualKeyCode.OEM_3, new KeyData { DefaultKey = '`', ShiftKey = '~' });
        DicKeyData.Add(VirtualKeyCode.OEM_MINUS, new KeyData { DefaultKey = '-', ShiftKey = '_' });
        DicKeyData.Add(VirtualKeyCode.OEM_PLUS, new KeyData { DefaultKey = '=', ShiftKey = '+' });
        DicKeyData.Add(VirtualKeyCode.BACK, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.TAB, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.OEM_4, new KeyData { DefaultKey = '[', ShiftKey = '{' });
        DicKeyData.Add(VirtualKeyCode.OEM_6, new KeyData { DefaultKey = ']', ShiftKey = '}' });
        DicKeyData.Add(VirtualKeyCode.OEM_5, new KeyData { DefaultKey = '￦', ShiftKey = '|' });
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
    //Do Not Any Action
    VK_NONE = -1,
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

//_dicKeyData.Add(VirtualKeyCode.VK_A, new KeyData { DefaultKey = "a", KorKey = "ㅁ" });
//_dicKeyData.Add(VirtualKeyCode.VK_B, new KeyData { DefaultKey = "b", KorKey = "ㅠ" });
//_dicKeyData.Add(VirtualKeyCode.VK_C, new KeyData { DefaultKey = "c", KorKey = "ㅊ" });
//_dicKeyData.Add(VirtualKeyCode.VK_D, new KeyData { DefaultKey = "d", KorKey = "ㅇ" });
//_dicKeyData.Add(VirtualKeyCode.VK_E, new KeyData { DefaultKey = "e", KorKey = "ㄷ", KorShiftKey = "ㄸ" });
//_dicKeyData.Add(VirtualKeyCode.VK_F, new KeyData { DefaultKey = "f", KorKey = "ㄹ" });
//_dicKeyData.Add(VirtualKeyCode.VK_G, new KeyData { DefaultKey = "g", KorKey = "ㅎ" });
//_dicKeyData.Add(VirtualKeyCode.VK_H, new KeyData { DefaultKey = "h", KorKey = "ㅗ" });
//_dicKeyData.Add(VirtualKeyCode.VK_I, new KeyData { DefaultKey = "i", KorKey = "ㅑ" });
//_dicKeyData.Add(VirtualKeyCode.VK_J, new KeyData { DefaultKey = "j", KorKey = "ㅓ" });
//_dicKeyData.Add(VirtualKeyCode.VK_K, new KeyData { DefaultKey = "k", KorKey = "ㅏ" });
//_dicKeyData.Add(VirtualKeyCode.VK_L, new KeyData { DefaultKey = "l", KorKey = "ㅣ" });
//_dicKeyData.Add(VirtualKeyCode.VK_M, new KeyData { DefaultKey = "m", KorKey = "ㅡ" });
//_dicKeyData.Add(VirtualKeyCode.VK_N, new KeyData { DefaultKey = "n", KorKey = "ㅜ" });
//_dicKeyData.Add(VirtualKeyCode.VK_O, new KeyData { DefaultKey = "o", KorKey = "ㅐ", KorShiftKey = "ㅒ" });
//_dicKeyData.Add(VirtualKeyCode.VK_P, new KeyData { DefaultKey = "p", KorKey = "ㅔ", KorShiftKey = "ㅖ" });
//_dicKeyData.Add(VirtualKeyCode.VK_Q, new KeyData { DefaultKey = "q", KorKey = "ㅂ", KorShiftKey = "ㅃ" });
//_dicKeyData.Add(VirtualKeyCode.VK_R, new KeyData { DefaultKey = "r", KorKey = "ㄱ", KorShiftKey = "ㄲ" });
//_dicKeyData.Add(VirtualKeyCode.VK_S, new KeyData { DefaultKey = "s", KorKey = "ㄴ" });
//_dicKeyData.Add(VirtualKeyCode.VK_T, new KeyData { DefaultKey = "t", KorKey = "ㅅ", KorShiftKey = "ㅆ" });
//_dicKeyData.Add(VirtualKeyCode.VK_U, new KeyData { DefaultKey = "u", KorKey = "ㅕ" });
//_dicKeyData.Add(VirtualKeyCode.VK_V, new KeyData { DefaultKey = "v", KorKey = "ㅍ" });
//_dicKeyData.Add(VirtualKeyCode.VK_W, new KeyData { DefaultKey = "w", KorKey = "ㅈ", KorShiftKey = "ㅉ" });
//_dicKeyData.Add(VirtualKeyCode.VK_X, new KeyData { DefaultKey = "x", KorKey = "ㅌ" });
//_dicKeyData.Add(VirtualKeyCode.VK_Y, new KeyData { DefaultKey = "y", KorKey = "ㅛ" });
//_dicKeyData.Add(VirtualKeyCode.VK_Z, new KeyData { DefaultKey = "z", KorKey = "ㅋ" });

//_dicKeyData.Add(VirtualKeyCode.OEM_3, new KeyData { DefaultKey = "`", ShiftKey = "~" });
//_dicKeyData.Add(VirtualKeyCode.OEM_MINUS, new KeyData { DefaultKey = "-", ShiftKey = "_" });
//_dicKeyData.Add(VirtualKeyCode.OEM_PLUS, new KeyData { DefaultKey = "=", ShiftKey = "+" });
//_dicKeyData.Add(VirtualKeyCode.BACK, new KeyData { DefaultKey = "Backspace" });
//_dicKeyData.Add(VirtualKeyCode.TAB, new KeyData { DefaultKey = "Tab" });
//_dicKeyData.Add(VirtualKeyCode.OEM_4, new KeyData { DefaultKey = "[", ShiftKey = "{" });
//_dicKeyData.Add(VirtualKeyCode.OEM_6, new KeyData { DefaultKey = "]", ShiftKey = "}" });
//_dicKeyData.Add(VirtualKeyCode.OEM_5, new KeyData { DefaultKey = "￦", ShiftKey = "|" });
//_dicKeyData.Add(VirtualKeyCode.CAPITAL, new KeyData { DefaultKey = "Caps Lock" });
//_dicKeyData.Add(VirtualKeyCode.OEM_1, new KeyData { DefaultKey = ";", ShiftKey = ":" });
//_dicKeyData.Add(VirtualKeyCode.OEM_7, new KeyData { DefaultKey = "'", ShiftKey = "″" });
//_dicKeyData.Add(VirtualKeyCode.RETURN, new KeyData { DefaultKey = "Enter" });
//_dicKeyData.Add(VirtualKeyCode.SHIFT, new KeyData { DefaultKey = "Shift" });
//_dicKeyData.Add(VirtualKeyCode.OEM_COMMA, new KeyData { DefaultKey = ",", ShiftKey = "<" });
//_dicKeyData.Add(VirtualKeyCode.OEM_PERIOD, new KeyData { DefaultKey = ".", ShiftKey = ">" });
//_dicKeyData.Add(VirtualKeyCode.OEM_2, new KeyData { DefaultKey = "/", ShiftKey = "?" });
//_dicKeyData.Add(VirtualKeyCode.HANGUL, new KeyData { DefaultKey = "한/영" });
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