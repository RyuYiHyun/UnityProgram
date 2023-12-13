using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int range;           //�̵� ����
    public int jumpHeight;      //���� ����
    public int speed;
    protected Unit unit;        //�̵��ϴ� ��ü(Monster or Hero)
    protected Transform jumper;

    public Board board;
    protected List<Tile> PathList = new List<Tile>();


    /* ==========�߰� */

    public virtual List<Tile> GetTilesInRange()
    {
        board.ClearPathData();
        //List<Tile> RangeList = new List<Tile>();
        Queue<Tile> OpenList = new Queue<Tile>();
        List<Tile> ClosedList = new List<Tile>();
        Tile currentTile = unit.tile;
        OpenList.Enqueue(currentTile);

        while (OpenList.Count > 0)
        {
            currentTile = OpenList.Dequeue();
            ClosedList.Add(currentTile);
            // �� �� �� ��
            Action<Point> OpenListAdd = (Point check) =>
            {
                Tile ckeckTile = board.GetTile(check);
                if (ckeckTile != null)// ���� ����� �ʰ�
                {
                    if (!ClosedList.Contains(ckeckTile)) // ���� ����Ʈ������
                    {
                        int MoveCost = currentTile.G + (currentTile.pos.x - check.x == 0 || currentTile.pos.y - check.y == 0 ? 1 : 2);
                        if (MoveCost <= range)
                        {
                            if(Mathf.Abs(currentTile.height - ckeckTile.height) <= jumpHeight)
                            {
                                if (!OpenList.Contains(ckeckTile))
                                {
                                    ckeckTile.G = MoveCost;
                                    OpenList.Enqueue(ckeckTile);
                                }
                            }
                        }
                    }
                }
            };
            OpenListAdd(new Point(currentTile.pos.x, currentTile.pos.y + 1));
            OpenListAdd(new Point(currentTile.pos.x + 1, currentTile.pos.y));
            OpenListAdd(new Point(currentTile.pos.x, currentTile.pos.y - 1));
            OpenListAdd(new Point(currentTile.pos.x - 1, currentTile.pos.y));
        }
        // �̵� ���� �� �̵��� �� �ִ� Ÿ�ϵ��� ��ȯ
        return ClosedList;
    }

    /*================*/






    protected virtual void Awake()
    {
        if (speed <= 0)
        {
            speed = 1;
        }
        unit = GetComponent<Unit>();
        jumper = transform.Find("Ani");//unit.transform;
    }
    public abstract IEnumerator PathFinding(Tile target);

    public abstract IEnumerator PathFollow();
}
