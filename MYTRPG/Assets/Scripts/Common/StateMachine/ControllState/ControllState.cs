using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllState : LState
{
    protected BattleController owner;
    public FollowCamera followCamera { get { return owner.followCamera; } }
    public Board board { get { return owner.board; } }
    public MapData mapData { get { return owner.mapData; } }
    public Transform tileCursor { get { return owner.tileCursor; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }
    private void Awake()
    {
        owner = GetComponent<BattleController>();
    }
    protected override void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.commendEvent += OnCommand;
    }
    protected override void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.commendEvent -= OnCommand;
    }

    protected virtual void OnMove(object sender, InfoEventArgs<Point> e)
    {

    }
    protected virtual void OnCommand(object sender, InfoEventArgs<int> e)
    {

    }
    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
        {
            return;
        }
        pos = p;
        tileCursor.localPosition = board.tiles[p].tileCenter;
    }
}
