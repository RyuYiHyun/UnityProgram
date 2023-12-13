using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitControllState : ControllState
{
    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        // 타일맵을 로드한다.
        board.Load(mapData);

        // 현재 선택된 타일인디게이터(게임오브젝트) 의 좌표를 설정한다.
        Point p = new Point((int)mapData.tiles[0].x, (int)mapData.tiles[0].z);
        SelectTile(p);

        SpawnTestUnits();

        yield return null;

        // 현재 상태를 MoveTargetState로 변경한다.
        owner.ChangeState<SelectUnitState>();
    }

    void SpawnTestUnits()
    {
        System.Type[] components = new System.Type[] { typeof(WalkMovement) };

        for (int i = 0; i < 1; ++i)
        {
            // 영웅을 생성하고
            GameObject instance = Instantiate(owner.unitPrefab) as GameObject;

            // 해당 영웅의 시작 좌표를 부여하고.
            Point p = new Point((int)mapData.tiles[i].x, (int)mapData.tiles[i].z);
            owner.controllUnit = instance.GetComponent<Unit>();
            owner.controllUnit.Place(board.GetTile(p));
            owner.controllUnit.Match();
            // 영웅의 이동 방식을 넣고
            Movement m = instance.AddComponent(components[i]) as Movement;
            m.board = board;
            // 이동범위와 점프 높이를 설정한다.
            m.range = 4;
            m.jumpHeight = 2;
            m.speed = 4;

        }
    }
}
