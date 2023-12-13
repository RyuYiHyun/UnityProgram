using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private Unit unit;
    public override void OnEnter()
    {
        unit = GetComponent<Unit>();
        if (unit == null)
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
        //    unit.state.ChangeState<FollowState>();
        //}
        if(unit.endTile != null && unit.tile != null)
        {
            if(unit.endTile != unit.tile)
            {
                unit.stateMachine.ChangeState<PathFindingState>();
            }
        }
    }
}
