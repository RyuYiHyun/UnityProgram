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

    // ���õ� Ÿ�� �ε�������(���ӿ�����Ʈ)�� ��ġ�� �����մϴ�.
    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
            return;
        pos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
    }
}
