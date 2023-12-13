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
            Debug.LogError("�����̾���!");
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
        // ������ �̵��� ��Ʈ�� ����Ѵ�.
        yield return StartCoroutine(m.PathFinding(unit.endTile));
        // �̵�
        unit.stateMachine.ChangeState<MovingAnimatedState>();
    }
}
