using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllStateMachine : MonoBehaviour
{
    public virtual LState CurrentState
    {
        get { return _currentState; }
        set { Transition(value); }
    }
    protected LState _currentState;
    protected bool _inTransition;

    // �����Ϸ��� ���°� �ش� ���ӿ�����Ʈ�� ������Ʈ�� �ִ��� üũ�Ѵ�.
    public virtual T GetState<T>() where T : LState
    {
        T target = GetComponent<T>();

        // �����Ϸ��� State�� ���ӿ�����Ʈ ������
        // �߰���Ų��.
        if (target == null)
            target = gameObject.AddComponent<T>();

        return target;
    }
    // ���¸� �����ų �� ȣ��ȴ�.
    public virtual void ChangeState<T>() where T : LState
    {
        CurrentState = GetState<T>();
    }

    protected virtual void Transition(LState newState)
    {
        // ���� ���¿� �����Ϸ��� ���°� ������ Ȯ��
        // �Ǵ� ���°� ���� ������ Ȯ��.
        if (_currentState == newState || _inTransition) return;
        _inTransition = true;


        // ���¸� ������ �� ���� ������ State.Exit()�� ȣ���Ѵ�.
        if (_currentState != null) _currentState.OnExit();

        // ���� ���¸� �����ϰ�
        _currentState = newState;

        // ����� ������ State.Enter()�� ȣ���Ѵ�.
        if (_currentState != null) _currentState.OnEnter();

        // ������ �Ϸ�Ǹ� _inTransition false�� �����Ѵ�.
        _inTransition = false;
    }
}
