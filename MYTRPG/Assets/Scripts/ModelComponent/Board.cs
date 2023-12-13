using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    // ���� �˻��� Ÿ���� �ֺ� Ÿ���� ������ ��
    // ����ϴ� ����
    Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };
    // ����/�̼��ÿ� ���� Ÿ�� ����
    Color selectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(0, 0, 0, 0);
    Color pathTileColor = new Color(1, 0, 0, 1);
    public void Load(MapData data)
    {
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab, transform) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
    }

    // �ش� ��ǥ�� Ÿ���� �ִ��� �˻�
    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    public void ClearPathData()
    {
        DeSelectTilesAll();
        foreach (Tile tile in tiles.Values)
        {
            tile.G = 0;
            tile.H = 0;
            tile.ParentTile = null;
        }
    }

    public void SelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            Renderer tileRender = tiles[i].tileColorUI;
            tileRender.material.SetColor("_Color", selectedTileColor);
        }
    }

    public void PathTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            Renderer tileRender = tiles[i].tileColorUI;
            tileRender.material.SetColor("_Color", pathTileColor);
        }
    }

    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
        {
            Renderer tileRender = tiles[i].tileColorUI;
            tileRender.material.SetColor("_Color", defaultTileColor);
        }
    }

    public void DeSelectTilesAll()
    {
        foreach (Tile tile in tiles.Values)
        {
            Renderer tileRender = tile.tileColorUI;
            //ColorUtility.TryParseHtmlString("#F1C8A4", out Color color);
            tileRender.material.SetColor("_Color", defaultTileColor);
        }
    }


    /*====*/
}
