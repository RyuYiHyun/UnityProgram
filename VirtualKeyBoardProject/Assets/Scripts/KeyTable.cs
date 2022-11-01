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
        if (Data == null) //instance°¡ null. Áï, ½Ã½ºÅÛ»ó¿¡ Á¸ÀçÇÏ°í ÀÖÁö ¾ÊÀ»¶§
        {
            Data = this; //³»ÀÚ½ÅÀ» instance·Î ³Ö¾îÁÝ´Ï´Ù.
            DontDestroyOnLoad(gameObject); //OnLoad(¾ÀÀÌ ·Îµå µÇ¾úÀ»¶§) ÀÚ½ÅÀ» ÆÄ±«ÇÏÁö ¾Ê°í À¯Áö
        }
        else
        {
            if (Data != this) //instance°¡ ³»°¡ ¾Æ´Ï¶ó¸é ÀÌ¹Ì instance°¡ ÇÏ³ª Á¸ÀçÇÏ°í ÀÖ´Ù´Â ÀÇ¹Ì
            {
                Destroy(this.gameObject); //µÑ ÀÌ»ó Á¸ÀçÇÏ¸é ¾ÈµÇ´Â °´Ã¼ÀÌ´Ï ¹æ±Ý AWakeµÈ ÀÚ½ÅÀ» »èÁ¦
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

    // ÃÊ¼ºINDEX
    public char[] cho = {
          '¤¡', '¤¢', '¤¤', '¤§', '¤¨',
          '¤©', '¤±', '¤²', '¤³', '¤µ', '¤¶',
          '¤·', '¤¸', '¤¹', '¤º', '¤»', '¤¼', '¤½', '¤¾' };
    const int cho_count = 19;
    // Áß¼ºINDEX
    public char[] jung = {
          '¤¿', '¤À', '¤Á', '¤Â', '¤Ã',
          '¤Ä', '¤Å', '¤Æ', '¤Ç', '¤È', '¤É',
          '¤Ê', '¤Ë', '¤Ì', '¤Í', '¤Î', '¤Ï',
          '¤Ð', '¤Ñ', '¤Ò', '¤Ó'};
    const int jung_count = 21;
    // Á¾¼ºINDEX
    public char[] jong = {
          ' ', '¤¡', '¤¢', '¤£', '¤¤', '¤¥', '¤¦', '¤§', '¤©',
          '¤ª', '¤«', '¤¬', '¤­', '¤®', '¤¯', '¤°', '¤±',
          '¤²', '¤´', '¤µ', '¤¶', '¤·', '¤¸', '¤º', '¤»', '¤¼', '¤½', '¤¾'};
    const int jong_count = 28;
    // Á¶ÇÕINDEX
    public char[] JungComb = { '¤È', '¤É', '¤Ê', '¤Í', '¤Î', '¤Ï', '¤Ò' };
    public string[] JungSplit = { "¤Ç¤¿", "¤Ç¤À", "¤Ç¤Ó", "¤Ì¤Ã", "¤Ì¤Ä", "¤Ì¤Ó", "¤Ñ¤Ó" };
    public char[] JongComb = { '¤£', '¤¥', '¤¦', '¤ª', '¤«', '¤¬', '¤­', '¤®', '¤¯', '¤°', '¤´' };
    public string[] JongSplit = { "¤¡¤µ", "¤¤¤¸", "¤¤¤¾", "¤©¤¡", "¤©¤±", "¤©¤²", "¤©¤µ", "¤©¤¼", "¤©¤½", "¤©¤¾", "¤´" };


    public const int BASE_MOD = 0xAC00;        // Ã³À½ °ª (°¡)


    public const int START_WORD = 0xAC00;        // À½¼º¹üÀ§ MIN(°¡)
    public const int LAST_WORD = 0xD7A3;        // À½¼º¹üÀ§ MAX(ÆR)

    public const int START_CHO = 0x1100;        // À½¼º¹üÀ§ MIN(¤¡)
    public const int LAST_CHO = 0x1112;        // À½¼º¹üÀ§ MAX(¤¾)

    public const int START_JUNG = 0x1161;        // À½¼º¹üÀ§ MIN(¤¿)
    public const int LAST_JUNG = 0x1175;        // À½¼º¹üÀ§ MAX(¤Ó)

    public const int START_JONG = 0x11A8;        // À½¼º¹üÀ§ MIN(¤¡)
    public const int LAST_JONG = 0x11C3;        // À½¼º¹üÀ§ MAX(¤¾)

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
            if (_jung == element) // Áß¼º ºÐÇØ °¡´É ÇÔ
            {
                _jungFirst = JungSplit[index][0];
                _jungSecond = JungSplit[index][1];
                return true;
            }
            ++index;
        }
        // Áß¼º ºÐÇØ ºÒ°¡´É
        _jungFirst = _jung;
        _jungSecond = ' ';
        return false;
    }
    public bool Split_Hangul_Jong(char _jong, out char _jongFirst, out char _jongSecond)
    {
        int index = 0;
        foreach (char element in JongComb)
        {
            if (_jong == element) // Á¾¼º ºÐÇØ °¡´É ÇÔ
            {
                _jongFirst = JongSplit[index][0];
                _jongSecond = JongSplit[index][1];
                return true;
            }
            ++index;
        }
        // Á¾¼º ºÐÇØ ºÒ°¡´É
        _jongFirst = _jong;
        _jongSecond = ' ';
        return false;
    }

    public void InputKRData(char _input)
    {
        // ÀÔ·ÂÀº ÃÊ¼º°ú Áß¼º¸¸ ÀÔ·Â °¡´É
        int ischo = FindIndex(_input, HANGUL_TYPE.CHO);
        int isjung = FindIndex(_input, HANGUL_TYPE.JUNG);
        if (ischo != -1)// ÃÊ¼º ÀÔ·Â ex) ¤¡
        {
            if (makeStr == "") // ¾Æ¹« ÀÔ·ÂÀÌ ¾ø¾úÀ½
            {
                makeStr += _input;
                edittext.text = completeStr + makeStr; // ÀÓ½Ã
                return;
            }
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)
            {// ±âÁ¸¿¡ ÃÊ¼ºÀÌ ÀÖÀ½ ex) ¤¡  -> ¤¡¤¡  ¶Ç´Â ¤£
                char result = Combine_Hangul_Jong(Convert.ToChar(makeStr), _input);
                if (result == ' ')
                { // ÇÕ¼º ºÒ°¡´É
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // ÀÓ½Ã
                    return;
                }
                else
                {
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; // ÀÓ½Ã
                    return;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)
            {// ±âÁ¸¿¡ Áß¼ºÀÌ ÀÖÀ½ ex) ¤¿ -> ¤¿¤¡
                completeStr += makeStr;
                edittext.text = completeStr;
                makeStr = "";
                makeStr += _input;
                edittext.text = completeStr + makeStr; // ÀÓ½Ã
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1)
            {// ±âÁ¸¿¡ Á¾¼ºÀÌ ÀÖÀ½ ex) ¤£ -> ¤£¤¡  ÇÕ¼º °¡´ÉÇÑ Á¾¼ºÀº ÃÊ¼º ÄÉÀÌ½º¿¡¼­ °É·¯Áü
                completeStr += makeStr;
                edittext.text = completeStr;
                makeStr = "";
                makeStr += _input;
                edittext.text = completeStr + makeStr; // ÀÓ½Ã
            }
            else
            {
                char _cho;
                char _jung;
                bool isjungSplit = false;
                char _jung1;// Áß¼º ºÐÇØ 1
                char _jung2;// Áß¼º ºÐÇØ 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// Á¾¼º ºÐÇØ 1
                char _jong2;// Á¾¼º ºÐÇØ 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (isjongSplit)
                {
                    // ÃÊ¼º + Áß¼º + <Á¾¼º + Á¾¼º> -> ex) B -> B¤¡ (ºÒ°¡´É ÇÕ¼º) -> Á¾¼ºÀÇ ºÐÇØ°¡ °¡´É  isjongSplit = true
                    // ÃÊ¼º + <Áß¼º + Áß¼º> + <Á¾¼º + Á¾¼º> -> ex) ñ -> ñ¤¡ (ºÒ°¡´É ÇÕ¼º) -> Á¾¼ºÀÇ ºÐÇØ°¡ °¡´É  isjongSplit = true
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // ÀÓ½Ã
                    return;
                }
                else
                {
                    if (_jong == ' ')
                    {
                        // ÃÊ¼º + Áß¼º -> ex) °¡ -> °¢ (¹«Á¶°Ç ÇÕ¼º)
                        // ÃÊ¼º + Áß¼º + Áß¼º -> ex) °ú -> °û  (¹«Á¶°Ç ÇÕ¼º)
                        char result = Combine_Hangul(_cho, _jung, _input); // ÇÕ¼º
                        makeStr = "";
                        makeStr += result;
                        edittext.text = completeStr + makeStr; // ÀÓ½Ã
                        return;
                    }
                    // ÃÊ¼º + Áß¼º + Á¾¼º -> ex) °¢ -> °¢¤¡ ¶Ç´Â B   (Á¶°ÇºÎ ÇÕ¼º) -> Á¾¼ºÀÇ ÇÕ¼º °¡´É ¿©ºÎ È®ÀÎ , isjongSplit = false
                    // ÃÊ¼º + <Áß¼º + Áß¼º> + Á¾¼º -> ex) °û -> °û¤¡  ¶Ç´Â ñ (Á¶°ÇºÎ ÇÕ¼º) -> Á¾¼ºÀÇ ÇÕ¼º °¡´É ¿©ºÎ È®ÀÎ , isjongSplit = false
                    char combineJong = Combine_Hangul_Jong(_jong, _input);
                    if (combineJong != ' ')
                    {// ÇÕ¼º °¡´É
                        char result = Combine_Hangul(_cho, _jung, combineJong); // ÇÕ¼º
                        makeStr = "";
                        makeStr += result;
                        edittext.text = completeStr + makeStr; // ÀÓ½Ã
                        return;
                    }
                    else
                    {// ÇÕ¼º ºÒ°¡´É
                        completeStr += makeStr;
                        edittext.text = completeStr;
                        makeStr = "";
                        makeStr += _input;
                        edittext.text = completeStr + makeStr; // ÀÓ½Ã
                        return;
                    }
                }
            }
        }
        if (isjung != -1)// Áß¼º ÀÔ·Â
        {
            if (makeStr == "") // ¾Æ¹« ÀÔ·ÂÀÌ ¾ø¾úÀ½
            {
                makeStr += _input;
                edittext.text = completeStr + makeStr; // ÀÓ½Ã
                return;
            }
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)
            {// ±âÁ¸¿¡ ÃÊ¼ºÀÌ ÀÖÀ½ ex) ¤¡  -> °¡ (¹«Á¶°Ç ÇÕ¼º °¡´É)
                char result = Combine_Hangul(Convert.ToChar(makeStr), _input);
                makeStr = "";
                makeStr += result;
                edittext.text = completeStr + makeStr; // ÀÓ½Ã
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)
            {// ±âÁ¸¿¡ Áß¼ºÀÌ ÀÖÀ½ ex) ¤¿ -> ¤¿¤¿  ¶Ç´Â ¤È
                char _jung = Convert.ToChar(makeStr);
                char result = Combine_Hangul_Jung(_jung, _input);
                if (result != ' ')
                {// ÇÕ¼º °¡´É
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; // ÀÓ½Ã
                    return;
                }
                else
                {// ÇÕ¼º ºÒ°¡´É
                    completeStr += makeStr;
                    edittext.text = completeStr;
                    makeStr = "";
                    makeStr += _input;
                    edittext.text = completeStr + makeStr; // ÀÓ½Ã
                    return;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1)
            {// ±âÁ¸¿¡ Á¾¼ºÀÌ ÀÖÀ½ ex) ¤£ -> ¤¡»ç  ³ª¸ÓÁö´Â ÃÊ¼º ÄÉÀÌ½º¿¡¼­ °É·¯Áü
                char _jong = Convert.ToChar(makeStr);
                bool isjongSplit = false;
                char _jong1;// Á¾¼º ºÐÇØ 1
                char _jong2;// Á¾¼º ºÐÇØ 2
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);
                if (isjongSplit)
                {
                    makeStr = "";
                    makeStr += _jong1;
                    completeStr += makeStr;
                    char result = Combine_Hangul(_jong2, _input);
                    makeStr = "";
                    makeStr += result;
                    edittext.text = completeStr + makeStr; // ÀÓ½Ã
                    return;
                }
            }
            else
            {
                char _cho;
                char _jung;
                bool isjungSplit = false;
                char _jung1;// Áß¼º ºÐÇØ 1
                char _jung2;// Áß¼º ºÐÇØ 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// Á¾¼º ºÐÇØ 1
                char _jong2;// Á¾¼º ºÐÇØ 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (isjongSplit)
                {
                    // ÃÊ¼º + Áß¼º + <Á¾¼º + Á¾¼º> -> ex) B -> °¢»ç (¹«Á¶°Ç ÇÕ¼º °¡´É) 
                    // ÃÊ¼º + <Áß¼º + Áß¼º> + <Á¾¼º + Á¾¼º> -> ex) ñ-> °û»ç (¹«Á¶°Ç ÇÕ¼º °¡´É) 
                    char result1 = Combine_Hangul(_cho, _jung, _jong1);
                    char result2 = Combine_Hangul(_jong2, _input);
                    completeStr += result1;
                    makeStr = "";
                    makeStr += result2;
                    edittext.text = completeStr + makeStr; // ÀÓ½Ã
                    return;
                }
                else
                {
                    if (_jong != ' ') // Á¾¼ºÀÌ ÀÖÀ½
                    {// ÃÊ¼º + Áß¼º + Á¾¼º -> ex) °¢ -> °¡°¡ (¹«Á¶°Ç ÇÕ¼º °¡´É) 
                        char result1 = Combine_Hangul(_cho, _jung);
                        char result2 = Combine_Hangul(_jong, _input);
                        completeStr += result1;
                        makeStr = "";
                        makeStr += result2;
                        edittext.text = completeStr + makeStr; // ÀÓ½Ã
                        return;
                    }
                    else
                    {
                        if (isjungSplit) // Áß¼ºÀÌ ºÐÇØ°¡ µÇ´ÂÁö
                        {// ÃÊ¼º + <Áß¼º + Áß¼º> -> ex) °ú -> °ú¤¿    (ºÒ°¡´É ÇÕ¼º)
                            completeStr += makeStr;
                            edittext.text = completeStr;
                            makeStr = "";
                            makeStr += _input;
                            edittext.text = completeStr + makeStr; // ÀÓ½Ã
                        }
                        else
                        {// ÃÊ¼º + Áß¼º -> ex) °í -> °ú ¶Ç´Â °í¤Ç  (Á¶°ÇºÎ ÇÕ¼º)
                            char result = Combine_Hangul_Jung(_jung, _input);
                            if (result != ' ')
                            {// ÇÕ¼º °¡´É
                                char combine = Combine_Hangul(_cho, result);
                                makeStr = "";
                                makeStr += combine;
                                edittext.text = completeStr + makeStr; // ÀÓ½Ã
                                return;
                            }
                            else
                            {// ÇÕ¼º ºÒ°¡´É
                                completeStr += makeStr;
                                edittext.text = completeStr;
                                makeStr = "";
                                makeStr += _input;
                                edittext.text = completeStr + makeStr; // ÀÓ½Ã
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
        if (makeStr == "") // ¾²´ø ¹®ÀÚ°¡ ¾øÀ»¶§
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
            if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.CHO) != -1)// ÃÊ¼º
            {
                makeStr = "";
                edittext.text = completeStr + makeStr;
                return;
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JUNG) != -1)// Áß¼º
            {
                char jung1;
                char jung2;
                if (Split_Hangul_Jung(Convert.ToChar(makeStr), out jung1, out jung2))
                {// ºÐÇØ ¼º°ø
                    makeStr = "";
                    makeStr += jung1;
                    edittext.text = completeStr + makeStr;
                    return;
                }
                else
                {//ºÐÇØ ¾ÈµÊ
                    makeStr = "";
                    edittext.text = completeStr + makeStr;
                }
            }
            else if (FindIndex(Convert.ToChar(makeStr), HANGUL_TYPE.JONG) != -1) // Á¾¼º
            {
                char jong1;
                char jong2;
                if (Split_Hangul_Jong(Convert.ToChar(makeStr), out jong1, out jong2))
                {// ºÐÇØ ¼º°ø
                    makeStr = "";
                    makeStr += jong1;
                    edittext.text = completeStr + makeStr;
                    return;
                }
                else
                {//ºÐÇØ ¾ÈµÊ
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
                char _jung1;// Áß¼º ºÐÇØ 1
                char _jung2;// Áß¼º ºÐÇØ 2
                char _jong;
                bool isjongSplit = false;
                char _jong1;// Á¾¼º ºÐÇØ 1
                char _jong2;// Á¾¼º ºÐÇØ 2
                Split_Hangul(Convert.ToChar(makeStr), out _cho, out _jung, out _jong);
                isjungSplit = Split_Hangul_Jung(_jung, out _jung1, out _jung2);
                isjongSplit = Split_Hangul_Jong(_jong, out _jong1, out _jong2);

                if (_jong != ' ') // Á¾¼º ¿©ºÎ
                {
                    if (isjongSplit) // Á¾¼º ºÐÇØ ¿©ºÎ
                    {//B -> °¢
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung, _jong1);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                    else
                    {//°¢ -> °¡
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                }
                else
                {
                    if (isjungSplit)// Áß¼º ºÐÇØ ¿©ºÎ
                    {//°ú -> °í
                        makeStr = "";
                        makeStr += Combine_Hangul(_cho, _jung1);
                        edittext.text = completeStr + makeStr;
                        return;
                    }
                    else
                    {// °í -> ¤¡
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
        if (makeStr == "") // ¾²´ø ¹®ÀÚ°¡ ¾øÀ»¶§
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

        DicKeyData.Add(VirtualKeyCode.VK_A, new KeyData { DefaultKey = 'a', KorKey = '¤±' });
        DicKeyData.Add(VirtualKeyCode.VK_B, new KeyData { DefaultKey = 'b', KorKey = '¤Ð' });
        DicKeyData.Add(VirtualKeyCode.VK_C, new KeyData { DefaultKey = 'c', KorKey = '¤º' });
        DicKeyData.Add(VirtualKeyCode.VK_D, new KeyData { DefaultKey = 'd', KorKey = '¤·' });
        DicKeyData.Add(VirtualKeyCode.VK_E, new KeyData { DefaultKey = 'e', KorKey = '¤§', KorShiftKey = '¤¨' });
        DicKeyData.Add(VirtualKeyCode.VK_F, new KeyData { DefaultKey = 'f', KorKey = '¤©' });
        DicKeyData.Add(VirtualKeyCode.VK_G, new KeyData { DefaultKey = 'g', KorKey = '¤¾' });
        DicKeyData.Add(VirtualKeyCode.VK_H, new KeyData { DefaultKey = 'h', KorKey = '¤Ç' });
        DicKeyData.Add(VirtualKeyCode.VK_I, new KeyData { DefaultKey = 'i', KorKey = '¤Á' });
        DicKeyData.Add(VirtualKeyCode.VK_J, new KeyData { DefaultKey = 'j', KorKey = '¤Ã' });
        DicKeyData.Add(VirtualKeyCode.VK_K, new KeyData { DefaultKey = 'k', KorKey = '¤¿' });
        DicKeyData.Add(VirtualKeyCode.VK_L, new KeyData { DefaultKey = 'l', KorKey = '¤Ó' });
        DicKeyData.Add(VirtualKeyCode.VK_M, new KeyData { DefaultKey = 'm', KorKey = '¤Ñ' });
        DicKeyData.Add(VirtualKeyCode.VK_N, new KeyData { DefaultKey = 'n', KorKey = '¤Ì' });
        DicKeyData.Add(VirtualKeyCode.VK_O, new KeyData { DefaultKey = 'o', KorKey = '¤À', KorShiftKey = '¤Â' });
        DicKeyData.Add(VirtualKeyCode.VK_P, new KeyData { DefaultKey = 'p', KorKey = '¤Ä', KorShiftKey = '¤Æ' });
        DicKeyData.Add(VirtualKeyCode.VK_Q, new KeyData { DefaultKey = 'q', KorKey = '¤²', KorShiftKey = '¤³' });
        DicKeyData.Add(VirtualKeyCode.VK_R, new KeyData { DefaultKey = 'r', KorKey = '¤¡', KorShiftKey = '¤¢' });
        DicKeyData.Add(VirtualKeyCode.VK_S, new KeyData { DefaultKey = 's', KorKey = '¤¤' });
        DicKeyData.Add(VirtualKeyCode.VK_T, new KeyData { DefaultKey = 't', KorKey = '¤µ', KorShiftKey = '¤¶' });
        DicKeyData.Add(VirtualKeyCode.VK_U, new KeyData { DefaultKey = 'u', KorKey = '¤Å' });
        DicKeyData.Add(VirtualKeyCode.VK_V, new KeyData { DefaultKey = 'v', KorKey = '¤½' });
        DicKeyData.Add(VirtualKeyCode.VK_W, new KeyData { DefaultKey = 'w', KorKey = '¤¸', KorShiftKey = '¤¹' });
        DicKeyData.Add(VirtualKeyCode.VK_X, new KeyData { DefaultKey = 'x', KorKey = '¤¼' });
        DicKeyData.Add(VirtualKeyCode.VK_Y, new KeyData { DefaultKey = 'y', KorKey = '¤Ë' });
        DicKeyData.Add(VirtualKeyCode.VK_Z, new KeyData { DefaultKey = 'z', KorKey = '¤»' });

        DicKeyData.Add(VirtualKeyCode.OEM_3, new KeyData { DefaultKey = '`', ShiftKey = '~' });
        DicKeyData.Add(VirtualKeyCode.OEM_MINUS, new KeyData { DefaultKey = '-', ShiftKey = '_' });
        DicKeyData.Add(VirtualKeyCode.OEM_PLUS, new KeyData { DefaultKey = '=', ShiftKey = '+' });
        DicKeyData.Add(VirtualKeyCode.BACK, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.TAB, new KeyData { DefaultKey = ' ' });
        DicKeyData.Add(VirtualKeyCode.OEM_4, new KeyData { DefaultKey = '[', ShiftKey = '{' });
        DicKeyData.Add(VirtualKeyCode.OEM_6, new KeyData { DefaultKey = ']', ShiftKey = '}' });
        DicKeyData.Add(VirtualKeyCode.OEM_5, new KeyData { DefaultKey = '£Ü', ShiftKey = '|' });
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

//_dicKeyData.Add(VirtualKeyCode.VK_A, new KeyData { DefaultKey = "a", KorKey = "¤±" });
//_dicKeyData.Add(VirtualKeyCode.VK_B, new KeyData { DefaultKey = "b", KorKey = "¤Ð" });
//_dicKeyData.Add(VirtualKeyCode.VK_C, new KeyData { DefaultKey = "c", KorKey = "¤º" });
//_dicKeyData.Add(VirtualKeyCode.VK_D, new KeyData { DefaultKey = "d", KorKey = "¤·" });
//_dicKeyData.Add(VirtualKeyCode.VK_E, new KeyData { DefaultKey = "e", KorKey = "¤§", KorShiftKey = "¤¨" });
//_dicKeyData.Add(VirtualKeyCode.VK_F, new KeyData { DefaultKey = "f", KorKey = "¤©" });
//_dicKeyData.Add(VirtualKeyCode.VK_G, new KeyData { DefaultKey = "g", KorKey = "¤¾" });
//_dicKeyData.Add(VirtualKeyCode.VK_H, new KeyData { DefaultKey = "h", KorKey = "¤Ç" });
//_dicKeyData.Add(VirtualKeyCode.VK_I, new KeyData { DefaultKey = "i", KorKey = "¤Á" });
//_dicKeyData.Add(VirtualKeyCode.VK_J, new KeyData { DefaultKey = "j", KorKey = "¤Ã" });
//_dicKeyData.Add(VirtualKeyCode.VK_K, new KeyData { DefaultKey = "k", KorKey = "¤¿" });
//_dicKeyData.Add(VirtualKeyCode.VK_L, new KeyData { DefaultKey = "l", KorKey = "¤Ó" });
//_dicKeyData.Add(VirtualKeyCode.VK_M, new KeyData { DefaultKey = "m", KorKey = "¤Ñ" });
//_dicKeyData.Add(VirtualKeyCode.VK_N, new KeyData { DefaultKey = "n", KorKey = "¤Ì" });
//_dicKeyData.Add(VirtualKeyCode.VK_O, new KeyData { DefaultKey = "o", KorKey = "¤À", KorShiftKey = "¤Â" });
//_dicKeyData.Add(VirtualKeyCode.VK_P, new KeyData { DefaultKey = "p", KorKey = "¤Ä", KorShiftKey = "¤Æ" });
//_dicKeyData.Add(VirtualKeyCode.VK_Q, new KeyData { DefaultKey = "q", KorKey = "¤²", KorShiftKey = "¤³" });
//_dicKeyData.Add(VirtualKeyCode.VK_R, new KeyData { DefaultKey = "r", KorKey = "¤¡", KorShiftKey = "¤¢" });
//_dicKeyData.Add(VirtualKeyCode.VK_S, new KeyData { DefaultKey = "s", KorKey = "¤¤" });
//_dicKeyData.Add(VirtualKeyCode.VK_T, new KeyData { DefaultKey = "t", KorKey = "¤µ", KorShiftKey = "¤¶" });
//_dicKeyData.Add(VirtualKeyCode.VK_U, new KeyData { DefaultKey = "u", KorKey = "¤Å" });
//_dicKeyData.Add(VirtualKeyCode.VK_V, new KeyData { DefaultKey = "v", KorKey = "¤½" });
//_dicKeyData.Add(VirtualKeyCode.VK_W, new KeyData { DefaultKey = "w", KorKey = "¤¸", KorShiftKey = "¤¹" });
//_dicKeyData.Add(VirtualKeyCode.VK_X, new KeyData { DefaultKey = "x", KorKey = "¤¼" });
//_dicKeyData.Add(VirtualKeyCode.VK_Y, new KeyData { DefaultKey = "y", KorKey = "¤Ë" });
//_dicKeyData.Add(VirtualKeyCode.VK_Z, new KeyData { DefaultKey = "z", KorKey = "¤»" });

//_dicKeyData.Add(VirtualKeyCode.OEM_3, new KeyData { DefaultKey = "`", ShiftKey = "~" });
//_dicKeyData.Add(VirtualKeyCode.OEM_MINUS, new KeyData { DefaultKey = "-", ShiftKey = "_" });
//_dicKeyData.Add(VirtualKeyCode.OEM_PLUS, new KeyData { DefaultKey = "=", ShiftKey = "+" });
//_dicKeyData.Add(VirtualKeyCode.BACK, new KeyData { DefaultKey = "Backspace" });
//_dicKeyData.Add(VirtualKeyCode.TAB, new KeyData { DefaultKey = "Tab" });
//_dicKeyData.Add(VirtualKeyCode.OEM_4, new KeyData { DefaultKey = "[", ShiftKey = "{" });
//_dicKeyData.Add(VirtualKeyCode.OEM_6, new KeyData { DefaultKey = "]", ShiftKey = "}" });
//_dicKeyData.Add(VirtualKeyCode.OEM_5, new KeyData { DefaultKey = "£Ü", ShiftKey = "|" });
//_dicKeyData.Add(VirtualKeyCode.CAPITAL, new KeyData { DefaultKey = "Caps Lock" });
//_dicKeyData.Add(VirtualKeyCode.OEM_1, new KeyData { DefaultKey = ";", ShiftKey = ":" });
//_dicKeyData.Add(VirtualKeyCode.OEM_7, new KeyData { DefaultKey = "'", ShiftKey = "¡È" });
//_dicKeyData.Add(VirtualKeyCode.RETURN, new KeyData { DefaultKey = "Enter" });
//_dicKeyData.Add(VirtualKeyCode.SHIFT, new KeyData { DefaultKey = "Shift" });
//_dicKeyData.Add(VirtualKeyCode.OEM_COMMA, new KeyData { DefaultKey = ",", ShiftKey = "<" });
//_dicKeyData.Add(VirtualKeyCode.OEM_PERIOD, new KeyData { DefaultKey = ".", ShiftKey = ">" });
//_dicKeyData.Add(VirtualKeyCode.OEM_2, new KeyData { DefaultKey = "/", ShiftKey = "?" });
//_dicKeyData.Add(VirtualKeyCode.HANGUL, new KeyData { DefaultKey = "ÇÑ/¿µ" });
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