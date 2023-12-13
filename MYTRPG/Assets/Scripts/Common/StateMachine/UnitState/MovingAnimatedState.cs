using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAnimatedState : State
{
    Unit unit;
    // ���¸� �غ��Ѵ�.
    public override void OnEnter() 
    {
        unit = GetComponent<Unit>();
        if (unit == null)
        {
            Debug.LogError("�����̾���!");
        }
        StartCoroutine(Sequence());
    }

    public override void OnUpdate() { }

    // ���¸� �����Ѵ�.
    public override void OnExit() { }

    IEnumerator Sequence()
    {
        Movement m = GetComponent<Movement>();
        // ���ַ�Ʈ�� �ش��ϴ� �̵��� ����
        yield return StartCoroutine(m.PathFollow());
        // ���̵� �� �޽�
        unit.stateMachine.ChangeState<IdleState>();
    }
}
