using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    // ���¸� �غ��Ѵ�.
    public virtual void OnEnter() { }


    public virtual void OnUpdate() { }

    // ���¸� �����Ѵ�.
    public virtual void OnExit() { }
}
