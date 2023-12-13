using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{
    private float speed = 1f;
    private Unit unit;

    public override void OnEnter()
    {
        unit = GetComponent<Unit>();
        if(unit == null)
        {
            Debug.LogError("À¯´ÖÀÌ¾ø´Ù!");
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        //if (unit.target != null)
        //{
        //    Vector3 dir = (unit.target.GetComponent<Transform>().position - unit.GetComponent<Transform>().position).normalized;
        //    unit.GetComponent<Transform>().LookAt(unit.target.GetComponent<Transform>());
        //    unit.GetComponent<Transform>().position += dir * speed * Time.deltaTime;
        //}
        //else
        //{
        //    unit.state.ChangeState<IdleState>();
        //}   
    }
}
