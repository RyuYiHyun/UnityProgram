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

    // 변경하려는 상태가 해당 게임오브젝트에 컴포넌트로 있는지 체크한다.
    public virtual T GetState<T>() where T : LState
    {
        T target = GetComponent<T>();

        // 변경하려는 State가 게임오브젝트 없으면
        // 추가시킨다.
        if (target == null)
            target = gameObject.AddComponent<T>();

        return target;
    }
    // 상태를 변경시킬 때 호출된다.
    public virtual void ChangeState<T>() where T : LState
    {
        CurrentState = GetState<T>();
    }

    protected virtual void Transition(LState newState)
    {
        // 현재 상태와 변경하려는 상태가 같은지 확인
        // 또는 상태가 변경 중인지 확인.
        if (_currentState == newState || _inTransition) return;
        _inTransition = true;


        // 상태를 변경할 때 현재 상태의 State.Exit()를 호출한다.
        if (_currentState != null) _currentState.OnExit();

        // 현재 상태를 변경하고
        _currentState = newState;

        // 변경된 상태의 State.Enter()를 호출한다.
        if (_currentState != null) _currentState.OnEnter();

        // 변경이 완료되면 _inTransition false로 변경한다.
        _inTransition = false;
    }
}
