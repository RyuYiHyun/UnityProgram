using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");
    // �̵� �̺�Ʈ ��鷯
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    // �߻� �̺�Ʈ ��鷯
    public static event EventHandler<InfoEventArgs<int>> commendEvent;

    // Fire1 =����Ʈ��, Fire2 ���� ��Ʈ, Fire3 ���� ����Ʈ 
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };
    void Update()
    {
        int x = _hor.Update(); // -1, 0, 1 �߿� �ϳ��� ��ȯ�ȴ�.
        int y = _ver.Update();

        // Ű �Է��� �ִٸ�
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
{//�̺�Ʈ�ڵ鷯�� T ������ ������ �����ϱ�����
    public T info;
    // ������1
    public InfoEventArgs()
    {
        info = default(T);
    }
    // ������2
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
        //GetAxisRaw �� �Է°��� ���� -1~1 ������ ���� ��ȯ.
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));
        //�Է��� �ִٸ�
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
