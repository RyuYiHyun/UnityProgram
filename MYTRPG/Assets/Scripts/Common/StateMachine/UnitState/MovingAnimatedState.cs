using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimatedState : State
{
    Unit unit;
    // 상태를 준비한다.
    public override void OnEnter() 
    {
        unit = GetComponent<Unit>();
        if (unit == null)
        {
            Debug.LogError("유닛이없다!");
        }
        StartCoroutine(Sequence());
    }

    public override void OnUpdate() { }

    // 상태를 해제한다.
    public override void OnExit() { }

    IEnumerator Sequence()
    {
        Movement m = GetComponent<Movement>();
        // 유닛루트에 해당하는 이동을 진행
        yield return StartCoroutine(m.PathFollow());
        // 다이동 후 휴식
        unit.stateMachine.ChangeState<IdleState>();
    }
}
