using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : ControllStateMachine
{
    public FollowCamera followCamera;
    public Board board;
    public MapData mapData;
    public Transform tileCursor;
    public Point pos;


    public GameObject unitPrefab;
    public Unit controllUnit;
    public Tile currentTile { get { return board.GetTile(pos); } }
    void Start()
    {
        ChangeState<InitControllState>();
    }
}
