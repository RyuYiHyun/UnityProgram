using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : ControllState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnCommand(object sender, InfoEventArgs<int> e)
    {
        GameObject content = owner.currentTile.content;
        if (content != null)
        {
            owner.controllUnit = content.GetComponent<Unit>();
            owner.ChangeState<SelectTileState>();
        }
    }
}
