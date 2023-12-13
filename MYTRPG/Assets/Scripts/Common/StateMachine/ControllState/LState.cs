using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LState : MonoBehaviour
{
    // 상태를 준비한다.
    public virtual void OnEnter() 
    {
        AddListeners();
    }
    // 상태를 해제한다.
    public virtual void OnExit() 
    {
        RemoveListeners();
    }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    // 이벤트핸들러에 이벤트를 추가한다.
    protected virtual void AddListeners()
    {

    }
    // 이벤트핸들러에 이벤트를 제거한다.
    protected virtual void RemoveListeners()
    {

    }
}
