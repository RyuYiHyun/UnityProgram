using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    // 상태를 준비한다.
    public virtual void OnEnter() { }


    public virtual void OnUpdate() { }

    // 상태를 해제한다.
    public virtual void OnExit() { }
}
