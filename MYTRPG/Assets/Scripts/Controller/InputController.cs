using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");
    // 이동 이벤트 헨들러
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    // 발사 이벤트 헨들러
    public static event EventHandler<InfoEventArgs<int>> commendEvent;

    // Fire1 =왼컨트롤, Fire2 왼쪽 알트, Fire3 왼쪽 쉬프트 
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };
    void Update()
    {
        int x = _hor.Update(); // -1, 0, 1 중에 하나가 반환된다.
        int y = _ver.Update();

        // 키 입력이 있다면
        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
            {
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (commendEvent != null)
                    commendEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }
}

public class InfoEventArgs<T> : EventArgs
{//이벤트핸들러에 T 형태의 정보를 전달하기위함
    public T info;
    // 생성자1
    public InfoEventArgs()
    {
        info = default(T);
    }
    // 생성자2
    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}

class Repeater
{
    const float threshold = 0.5f;
    const float rate = 0.25f;
    float _next;
    bool _hold;
    string _axis;

    public Repeater(string axisName)
    {
        _axis = axisName;
    }

    public int Update()
    {
        int retValue = 0;
        //GetAxisRaw 는 입력값에 따라 -1~1 사이의 값을 반환.
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));
        //입력이 있다면
        if (value != 0)
        {
            if (Time.time > _next)
            {
                retValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }
        return retValue;
    }
}
