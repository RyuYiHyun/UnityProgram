using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingState : State
{
    Unit unit;
    public override void OnEnter()
    {
        unit = GetComponent<Unit>();
        if (unit == null)
        {
            Debug.LogError("유닛이없다!");
        }
        StartCoroutine(Sequence());
    }
    public override void OnUpdate()
    {
    }
    public override void OnExit()
    {
    }
    IEnumerator Sequence()
    {
        Movement m = GetComponent<Movement>();
        // 유닛이 이동할 루트를 계산한다.
        yield return StartCoroutine(m.PathFinding(unit.endTile));
        // 이동
        unit.stateMachine.ChangeState<MovingAnimatedState>();
    }
}
