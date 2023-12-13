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
    //        // ↑ → ↓ ←
    //        Action<Point> OpenListAdd = (Point check) =>
    //        {
    //            Tile ckeckTile = board.GetTile(check);
    //            if (ckeckTile != null)// 범위 벗어나지 않고
    //            {
    //                if (!ClosedList.Contains(ckeckTile)) // 닫힌 리스트에없고
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
    //    // 이동 범위 내 이동할 수 있는 타일들을 반환
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
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            currentTile = OpenList.DequeueValue();
            ClosedList.Add(currentTile);
            // 마지막
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
            // ↑ → ↓ ←
            Action<Point> OpenListAdd = (Point check) =>
            {
                Tile ckeckTile = board.GetTile(check);
                if (ckeckTile != null)// 범위 벗어나지 않고
                {
                    if (!ClosedList.Contains(ckeckTile)) // 닫힌 리스트에없고
                    {
                        if (Mathf.Abs(currentTile.height - ckeckTile.height) <= jumpHeight)//ckeckTile.height <= jumpHeight + currentTile.height)
                        {// 지금 갈려는 타일의 높이 레벨이 점프 레벨과 현재의 타일 높이의 합보다 더 높으면 못감
                            Tile NeighborTile = ckeckTile;
                            int MoveCost = currentTile.G + (currentTile.pos.x - check.x == 0 || currentTile.pos.y - check.y == 0 ? 10 : 14);

                            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
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
        // 연속해서 각 지점으로 이동.
        for (int i = 1; i < PathList.Count; ++i)
        {
            // targets[target.count-1] 이 최종 목적지이다.
            Tile from = PathList[i - 1];
            Tile to = PathList[i];

            // from 이 to를 바라보는 방향을 enum 값으로 반환한다.
            Directions dir = from.GetDirection(to);
            if (unit.dir != dir)
            {
                //unit.Turn(dir);
                yield return StartCoroutine(Turn(dir));
            }
            if (from.height == to.height)
            {
                // 높이가 같으면 걷고
                yield return StartCoroutine(Walk(from, to));
            }
            else
            {
                // 높이가 다르면 뛴다.
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

    // 점프

    //end -= start;
    //return -end* value * (value - 2) + start;

}
