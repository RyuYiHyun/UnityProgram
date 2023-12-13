using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PriorityQueue;
using System;

public class WalkMovement : Movement
{
    public Transform tester;
    public void ClearData()
    {
        PathList.Clear();
    }
    //public override List<Tile> GetTilesInRange()
    //{
    //    base.GetTilesInRange();
    //    board.ClearPathData();
    //    //List<Tile> RangeList = new List<Tile>();
    //    Queue<Tile> OpenList = new Queue<Tile>();
    //    List<Tile> ClosedList = new List<Tile>();
    //    Tile currentTile = unit.tile;
    //    OpenList.Enqueue(currentTile);

    //    while (OpenList.Count > 0)
    //    {
    //        currentTile = OpenList.Dequeue();
    //        ClosedList.Add(currentTile);
    //        // �� �� �� ��
    //        Action<Point> OpenListAdd = (Point check) =>
    //        {
    //            Tile ckeckTile = board.GetTile(check);
    //            if (ckeckTile != null)// ���� ����� �ʰ�
    //            {
    //                if (!ClosedList.Contains(ckeckTile)) // ���� ����Ʈ������
    //                {
    //                    int MoveCost = currentTile.G + (currentTile.pos.x - check.x == 0 || currentTile.pos.y - check.y == 0 ? 1 : 2);
    //                    if (MoveCost <= range)
    //                    {
    //                        if (!OpenList.Contains(ckeckTile))
    //                        {
    //                            ckeckTile.G = MoveCost;
    //                            OpenList.Enqueue(ckeckTile);
    //                        }
    //                    }
    //                }
    //            }
    //        };
    //        OpenListAdd(new Point(currentTile.pos.x, currentTile.pos.y + 1));
    //        OpenListAdd(new Point(currentTile.pos.x + 1, currentTile.pos.y));
    //        OpenListAdd(new Point(currentTile.pos.x, currentTile.pos.y - 1));
    //        OpenListAdd(new Point(currentTile.pos.x - 1, currentTile.pos.y));
    //    }
    //    // �̵� ���� �� �̵��� �� �ִ� Ÿ�ϵ��� ��ȯ
    //    return ClosedList;
    //}
    public override IEnumerator PathFinding(Tile target)
    {
        ClearData();
        board.ClearPathData();
        Tile startTile = unit.tile;
        Tile currentTile;
        PriorityQueue<Tile> OpenList = new PriorityQueue<Tile>();
        List<Tile> ClosedList = new List<Tile>();
        OpenList.Enqueue(0, startTile);
        while (OpenList.Count > 0)
        {
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            currentTile = OpenList.DequeueValue();
            ClosedList.Add(currentTile);
            // ������
            if (currentTile == target)
            {
                Tile TargetCurNode = target;
                while (TargetCurNode != startTile)
                {
                    PathList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentTile;
                }
                PathList.Add(startTile);
                PathList.Reverse();

                board.PathTiles(PathList);
                yield break;
            }
            // �� �� �� ��
            Action<Point> OpenListAdd = (Point check) =>
            {
                Tile ckeckTile = board.GetTile(check);
                if (ckeckTile != null)// ���� ����� �ʰ�
                {
                    if (!ClosedList.Contains(ckeckTile)) // ���� ����Ʈ������
                    {
                        if (Mathf.Abs(currentTile.height - ckeckTile.height) <= jumpHeight)//ckeckTile.height <= jumpHeight + currentTile.height)
                        {// ���� ������ Ÿ���� ���� ������ ���� ������ ������ Ÿ�� ������ �պ��� �� ������ ����
                            Tile NeighborTile = ckeckTile;
                            int MoveCost = currentTile.G + (currentTile.pos.x - check.x == 0 || currentTile.pos.y - check.y == 0 ? 10 : 14);

                            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
                            if (MoveCost < NeighborTile.G || !OpenList.Contains(NeighborTile))
                            {
                                NeighborTile.G = MoveCost;
                                NeighborTile.H = (Mathf.Abs(NeighborTile.pos.x - target.pos.x) + Mathf.Abs(NeighborTile.pos.y - target.pos.y)) * 10;
                                NeighborTile.ParentTile = currentTile;

                                OpenList.Enqueue(NeighborTile.F, NeighborTile);
                            }
                        }
                    }
                }
            };
            OpenListAdd(new Point(currentTile.pos.x, currentTile.pos.y + 1));
            OpenListAdd(new Point(currentTile.pos.x + 1, currentTile.pos.y));
            OpenListAdd(new Point(currentTile.pos.x, currentTile.pos.y - 1));
            OpenListAdd(new Point(currentTile.pos.x - 1, currentTile.pos.y));
            yield return null;
        }
    }

    public override IEnumerator PathFollow()
    {
        // �����ؼ� �� �������� �̵�.
        for (int i = 1; i < PathList.Count; ++i)
        {
            // targets[target.count-1] �� ���� �������̴�.
            Tile from = PathList[i - 1];
            Tile to = PathList[i];

            // from �� to�� �ٶ󺸴� ������ enum ������ ��ȯ�Ѵ�.
            Directions dir = from.GetDirection(to);
            if (unit.dir != dir)
            {
                //unit.Turn(dir);
                yield return StartCoroutine(Turn(dir));
            }
            if (from.height == to.height)
            {
                // ���̰� ������ �Ȱ�
                yield return StartCoroutine(Walk(from, to));
            }
            else
            {
                // ���̰� �ٸ��� �ڴ�.
                yield return StartCoroutine(Jump(from, to));
            }
        }
        unit.Place(PathList[PathList.Count - 1]);
        board.DeSelectTiles(PathList);
        yield return null;
    }
    IEnumerator Turn(Directions dir)
    {
        Transform unitT = unit.GetComponent<Transform>();
        float time = 0;
        float duration = 1;
        //Mathf.Approximately(t.startValue.y, 0f)
        while (time < duration)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp(time + 6 * Time.deltaTime, 0, duration);
            Vector3 value = AcceleratedMotion.FirstFastLostSlow(unit.dir.ToEuler(), dir.ToEuler(), (time / duration));
            unitT.localEulerAngles = new Vector3(0, value.y, 0);
        }
        unit.Turn(dir);
    }

    IEnumerator Walk(Tile from, Tile to)
    {
        Transform unitT = unit.GetComponent<Transform>();
        Transform fromT = from.GetComponent<Transform>();
        Transform toT = to.GetComponent<Transform>();
        float time = 0;
        float duration = 1;

        while (time < duration)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp(time + speed * Time.deltaTime, 0, duration);
            Vector3 value = AcceleratedMotion.SameSpeed(fromT.localPosition, toT.localPosition, (time / duration));
            unitT.localPosition = new Vector3(value.x, unitT.localPosition.y, value.z);
        }
        unit.Interpolation(to);
    }
    IEnumerator Jump(Tile from, Tile to)
    {
        Transform unitT = unit.GetComponent<Transform>();
        Transform fromT = from.GetComponent<Transform>();
        Transform toT = to.GetComponent<Transform>();
        float time = 0;
        float duration = 1;
        bool isUp = true;
        if(from.height > to.height)
        {
            isUp = false;
        }
        while (time < duration)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp(time + speed * Time.deltaTime, 0, duration);
            Vector3 value = AcceleratedMotion.SameSpeed(fromT.localPosition, toT.localPosition, (time / duration));
            float jumpvalue = unitT.localPosition.y;
            if(isUp)
            {
                if (time <= duration / 2)
                {
                    jumpvalue = AcceleratedMotion.FirstFastLostSlow(from.height, to.height + 2, (time / duration) * 2);
                }
                else
                {
                    jumpvalue = AcceleratedMotion.FirstFastLostSlow(to.height, to.height + 2, (time / duration) * 2);
                }
            }
            else
            {
                if (time <= duration / 2)
                {
                    jumpvalue = AcceleratedMotion.FirstFastLostSlow(from.height, from.height + 2, (time / duration) * 2);
                }
                else
                {
                    jumpvalue = AcceleratedMotion.FirstFastLostSlow(to.height, from.height + 2, (time / duration) * 2);
                }
            }

            unitT.localPosition = new Vector3(value.x, jumpvalue * Tile.stepHeight + 0.25f, value.z);
        }
        unit.Interpolation(to);

    }

    // ����

    //end -= start;
    //return -end* value * (value - 2) + start;

}
