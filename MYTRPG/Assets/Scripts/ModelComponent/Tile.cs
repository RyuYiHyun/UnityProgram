using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tile : MonoBehaviour
{
    public const float stepHeight = 0.25f;
    public Vector2 offsetScale;
    public Point pos;
    public int height;

    public Renderer tileColorUI;

    private void Awake()
    {
        tileColorUI = transform.GetChild(0).GetComponent<Renderer>();
    }
    #region A*
    public GameObject content;
    public Tile ParentTile;
    public int G; // 실제 이동(실제 소모비용)
    public int H;// 휴리스틱 -> |가로| + |세로|  -> 예상비용
    public int F { get { return G + H; } } // 총비용
    #endregion
    public Vector3 center
    {
        get
        {
            return new Vector3(pos.x, height * stepHeight + 0.25f, pos.y);
        }
    }
    public Vector3 tileCenter
    {
        get
        {
            return new Vector3(pos.x, height * stepHeight + 0.1f, pos.y);
        }
    }

    private void Match()
    {
        transform.localPosition = new Vector3(pos.x, 0.0f, pos.y);//height * stepHeight / 2f
        transform.localScale = new Vector3(offsetScale.x, height * stepHeight, offsetScale.y);
    }
    // 타일을 건설
    public void BuildUp()
    {
        height++;
        Match();
    }

    // 타일을 철거한다.
    // 한번에 1층씩 철거한다. 
    public void BreakDown()
    {
        height--;
        Match();
    }

    public void Load(Point p, int h)
    {
        pos = p;
        height = h;
        Match();
    }
    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }
}
