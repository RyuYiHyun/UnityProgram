using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LState : MonoBehaviour
{
    // ���¸� �غ��Ѵ�.
    public virtual void OnEnter() 
    {
        AddListeners();
    }
    // ���¸� �����Ѵ�.
    public virtual void OnExit() 
    {
        RemoveListeners();
    }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    // �̺�Ʈ�ڵ鷯�� �̺�Ʈ�� �߰��Ѵ�.
    protected virtual void AddListeners()
    {

    }
    // �̺�Ʈ�ڵ鷯�� �̺�Ʈ�� �����Ѵ�.
    protected virtual void RemoveListeners()
    {

    }
}
