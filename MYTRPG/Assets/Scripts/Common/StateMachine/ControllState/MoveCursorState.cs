using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ������� ����
public class MoveCursorState : ControllState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }
}
