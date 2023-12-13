using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] MapData mapData;

    public Board board;

    public GameObject Testprefab;
    public Unit Testunit;
    private void Start()
    {
        if(mapData != null)
        {
            board.Load(mapData);
        }
        SpawnTestUnits();
    }
    // �ӽ� �Լ�
    void SpawnTestUnits()
    {
        System.Type[] components = new System.Type[] { typeof(WalkMovement)};

        for (int i = 0; i < 1; ++i)
        {
            // ������ �����ϰ�
            GameObject instance = Instantiate(Testprefab) as GameObject;

            // �ش� ������ ���� ��ǥ�� �ο��ϰ�.
            Point p = new Point((int)mapData.tiles[i].x, (int)mapData.tiles[i].z);
            Testunit = instance.GetComponent<Unit>();
            Testunit.Place(board.GetTile(p));
            Testunit.Match();
            // ������ �̵� ����� �ְ�
            Movement m = instance.AddComponent(components[i]) as Movement;
            m.board = board;
            // �̵������� ���� ���̸� �����Ѵ�.
            m.range = 5;
            m.jumpHeight = 1;
            m.speed = 1;

        }
    }



    //public override void Enter()
    //{
    //    base.Enter();
    //    StartCoroutine(Init());
    //}
    //IEnumerator Init()
    //{
    //    // Ÿ�ϸ��� �ε��Ѵ�.
    //    board.Load(levelData);

    //    // ���� ���õ� Ÿ���ε������(���ӿ�����Ʈ) �� ��ǥ�� �����Ѵ�.
    //    Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
    //    SelectTile(p);

    //    // �ӽ� �ڵ�(������ ��ȯ)
    //    SpawnTestUnits();

    //    yield return null;

    //    // ���� ���¸� SelectUnitState�� �����Ѵ�.
    //    owner.ChangeState<SelectUnitState>();
    //}

    //// �ӽ� �Լ�
    //void SpawnTestUnits()
    //{
    //    System.Type[] components
    //        = new System.Type[]
    //        { typeof(WalkMovement), typeof(FlyMovement), typeof(TeleportMovement) };


    //    for (int i = 0; i < 3; ++i)
    //    {
    //        // ������ �����ϰ�
    //        GameObject instance = Instantiate(owner.heroPrefab) as GameObject;

    //        // �ش� ������ ���� ��ǥ�� �ο��ϰ�.
    //        Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
    //        Unit unit = instance.GetComponent<Unit>();
    //        unit.Place(board.GetTile(p));
    //        unit.Match();

    //        // ������ �̵� ����� �ְ�
    //        Movement m = instance.AddComponent(components[i]) as Movement;

    //        // �̵������� ���� ���̸� �����Ѵ�.
    //        m.range = 5;
    //        m.jumpHeight = 1;
    //    }
    //}
}
