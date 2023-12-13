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
        // Ÿ�ϸ��� �ε��Ѵ�.
        board.Load(mapData);

        // ���� ���õ� Ÿ���ε������(���ӿ�����Ʈ) �� ��ǥ�� �����Ѵ�.
        Point p = new Point((int)mapData.tiles[0].x, (int)mapData.tiles[0].z);
        SelectTile(p);

        SpawnTestUnits();

        yield return null;

        // ���� ���¸� MoveTargetState�� �����Ѵ�.
        owner.ChangeState<SelectUnitState>();
    }

    void SpawnTestUnits()
    {
        System.Type[] components = new System.Type[] { typeof(WalkMovement) };

        for (int i = 0; i < 1; ++i)
        {
            // ������ �����ϰ�
            GameObject instance = Instantiate(owner.unitPrefab) as GameObject;

            // �ش� ������ ���� ��ǥ�� �ο��ϰ�.
            Point p = new Point((int)mapData.tiles[i].x, (int)mapData.tiles[i].z);
            owner.controllUnit = instance.GetComponent<Unit>();
            owner.controllUnit.Place(board.GetTile(p));
            owner.controllUnit.Match();
            // ������ �̵� ����� �ְ�
            Movement m = instance.AddComponent(components[i]) as Movement;
            m.board = board;
            // �̵������� ���� ���̸� �����Ѵ�.
            m.range = 4;
            m.jumpHeight = 2;
            m.speed = 4;

        }
    }
}
