using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusTileControllor : MonoBehaviour
{
    public Transform tileSelectionIndicator;

    public Board board;

    public Point pos;

    public Point test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectTile(test);
    }

    // 선택된 타일 인디케이터(게임오브젝트)의 위치를 변경합니다.
    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
            return;
        pos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
    }
}
