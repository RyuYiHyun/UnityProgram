using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public virtual State CurrentState
    {
        get { return _currentState; }
        set { Transition(value); }
    }
    protected State _currentState;
    protected bool _inTransition;

    // �����Ϸ��� ���°� �ش� ���ӿ�����Ʈ�� ������Ʈ�� �ִ��� üũ�Ѵ�.
    public virtual T GetState<T>() where T : State
    {
        T target = GetComponent<T>();

        // �����Ϸ��� State�� ���ӿ�����Ʈ ������
        // �߰���Ų��.
        if (target == null)
            target = gameObject.AddComponent<T>();

        return target;
    }
    // ���¸� �����ų �� ȣ��ȴ�.
    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }

    protected virtual void Transition(State newState)
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
