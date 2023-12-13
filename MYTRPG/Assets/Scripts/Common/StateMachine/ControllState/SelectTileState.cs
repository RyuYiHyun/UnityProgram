using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTileState : ControllState
{
    public List<Tile> CanSelectTile;
    public override void OnEnter()
    {
        base.OnEnter();
        Movement mover = owner.controllUnit.GetComponent<Movement>();
        CanSelectTile = mover.GetTilesInRange();
        board.SelectTiles(CanSelectTile);
    }

    public override void OnExit()
    {
        base.OnExit();
        board.DeSelectTiles(CanSelectTile);
        CanSelectTile = null;
    }


    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnCommand(object sender, InfoEventArgs<int> e)
    {
        // 클릭한 타일로 이동시킵니다.
        if (CanSelectTile.Contains(owner.currentTile))
        {
            owner.controllUnit.endTile = owner.currentTile;
            owner.ChangeState<SelectUnitState>();
        }
            
    }
}
