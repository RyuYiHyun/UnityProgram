using UnityEditor;
using UnityEngine;
// ���� ����,����,����, �̵����� ���
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour
{
    /// Tile ������
    [SerializeField] public GameObject tilePrefabs;
    
    /// TileSeletionIndicator ��ü
    [SerializeField] GameObject tileSeletionIndicatorPrefab;
    public GameObject tileSeletionIndicator;
    /// ���� ����
    /// (���� ���� ���� ���� ����)
    [SerializeField] public int width = 10;    // ��  
    [SerializeField] public int depth = 10;    // ���� 
    [SerializeField] public int height = 10;   // ����

    /// Ÿ�� ��ġ ��ġ
    [HideInInspector] public Point pos;

    /// ������ Ŀ���� �ִ� ��ǥ ��ȯ
    public Transform TileCursor
    {
        get
        {
            if (tileSeletionIndicator == null)
            {
                tileSeletionIndicator = Instantiate(tileSeletionIndicatorPrefab) as GameObject;
                tileCursor = tileSeletionIndicator.transform;
            }
            else
            {
                tileCursor = tileSeletionIndicator.transform;
            }
            return tileCursor;
        }
    }
    Transform tileCursor;
    // tileSelectionIndicatorPrefab �� ��ġ�� ������ �� ȣ��ȴ�.
    public void UpdateTileCursor()
    {
        Tile tile;
        if (tiles.ContainsKey(pos))
        {
            tile = tiles[pos];
        }
        else
        {
            tile = null;
        }
        if(tile != null)
        {
            TileCursor.localPosition = tile.center;
        }
        else
        {
            TileCursor.localPosition = new Vector3(pos.x, 0, pos.y);
        }
    }

    /// �ҷ����� Data
    [SerializeField] MapData mapData;

    /// Ÿ�� ��ġ ����(��ǥ, Ÿ��)�� ��� Dictionary 
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();


    [HideInInspector] public Rect CreateArea;
    /// ���� �簢���� ��ȯ�մϴ�.
    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, depth);
        int w = UnityEngine.Random.Range(1, width - x + 1);
        int h = UnityEngine.Random.Range(1, depth - y + 1);
        return new Rect(x, y, w, h);
    }
    /*=====================================================*/
    #region Tile
    /// Ÿ�� ����
    Tile Create()
    {
        GameObject instance = Instantiate(tilePrefabs) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    /// tiles �迭�� P ��ǥ�� Ÿ�� �ִ��� Ȯ���ϰ�
    /// ������ �ش� Ÿ���� ��ȯ�մϴ�.
    /// ������ ������ �ڿ� ������ Ÿ���� ��ȯ�մϴ�.
    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
        {
            return tiles[p];
        }
            

        Tile tile = Create();
        /// Tile.Load �� Ÿ���� ��ǥ�� ������ ���� ����
        tile.Load(p, 0);
        tiles.Add(p, tile);

        return tile;
    }
    #endregion
    /*=====================================================*/
    #region Tile BuildUp Function
    /// Ÿ���� ������ ���� �����մϴ�.
    void BuildUpSingle(Point p)
    {
        Tile tile = GetOrCreate(p);
        if (tile.height < height)
            tile.BuildUp();
    }
    /// ���� Ÿ�� �Ǽ�
    public void BuildUp()
    {
        if(tilePrefabs != null)
        {
            BuildUpSingle(pos);
        }
    }
    /// ���� �簢�� ���� ���� Ÿ���� ä�������ϴ�.
    void BuildUpRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                BuildUpSingle(p);
            }
        }
    }
    /// ���� Ÿ�� �Ǽ� (����)
    public void BuildUpArea()
    {
        if (tilePrefabs != null)
        {
            CreateArea.x = Mathf.Clamp(CreateArea.x, 0, width);
            CreateArea.y = Mathf.Clamp(CreateArea.y, 0, depth);
            CreateArea.width = Mathf.Clamp(CreateArea.width, 1, width - CreateArea.x + 1);
            CreateArea.height = Mathf.Clamp(CreateArea.height, 1, depth - CreateArea.y + 1);
            BuildUpRect(CreateArea);
        }
    }
    /// ���� Ÿ�� �Ǽ� (����)
    public void BuildUpRandomArea()
    {
        if (tilePrefabs != null)
        {
            Rect area = RandomRect();
            BuildUpRect(area);
        }
    }
    #endregion
    /*=====================================================*/
    #region Tile BreakDown Function
    /// Ÿ���� �����Ű�ų� ����
    void BreakDownSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile tile = tiles[p];
        tile.BreakDown();

        if (tile.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(tile.gameObject);
        }
    }
    /// ���� Ÿ�� ö��
    public void BreakDown()
    {
        BreakDownSingle(pos);
    }
    /// ���� �簢�� ���� ���� Ÿ���� �����ŵ�ϴ�.
    void BreakDownRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                BreakDownSingle(p);
            }
        }
    }
    /// ���� Ÿ�� ö�� (����)
    public void BreakDownArea()
    {
        CreateArea.x = Mathf.Clamp(CreateArea.x, 0, width);
        CreateArea.y = Mathf.Clamp(CreateArea.y, 0, depth);
        CreateArea.width = Mathf.Clamp(CreateArea.width, 1, width - CreateArea.x + 1);
        CreateArea.height = Mathf.Clamp(CreateArea.height, 1, depth - CreateArea.y + 1);
        BreakDownRect(CreateArea);
    }
    /// ���� Ÿ�� ö�� (����)
    public void BreakDownRandomArea()
    {
        Rect area = RandomRect();
        BreakDownRect(area);
    }
    #endregion
    /*=====================================================*/
    #region Save&Load
    /// ���(����)�� ����ϴ�.
    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
        {   /// Resources ������ ������ �����
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        filePath += "/Maps";
        if (!Directory.Exists(filePath))
        {   /// Maps ������ ������ �����
            AssetDatabase.CreateFolder("Assets/Resources", "Maps");
        }
        AssetDatabase.Refresh();
    }

    /// ���� ��ġ ������ �����մϴ�.
    public void Save()
    {
        // ������ ��θ� string Ÿ������ �����մϴ�.
        string filePath = Application.dataPath + "/Resources/Maps";

        // ���丮�� filePath ��ΰ� �ִ��� Ȯ���մϴ�.
        if (!Directory.Exists(filePath))
        {
            CreateSaveDirectory();
        }

        MapData map = ScriptableObject.CreateInstance<MapData>();
        map.tiles = new List<Vector3>(tiles.Count);

        // tiles �� Ÿ���� ��ǥ���� LevelData board �� �����մϴ�.
        foreach (Tile tile in tiles.Values)
        {
            map.tiles.Add(new Vector3(tile.pos.x, tile.height, tile.pos.y));
        }
        // ������ �����մϴ�.
        string fileName = string.Format($"Assets/Resources/Maps/{name}.asset");
        AssetDatabase.CreateAsset(map, fileName);
    }

    /// MapData�� ��ǥ�� Ÿ�� ��ġ
    public void Load()
    {
        Clear();
        if (mapData == null)
        {
            return;
        }
        foreach (Vector3 vector in mapData.tiles)
        {
            Tile tile = Create();
            tile.Load(vector);
            tiles.Add(tile.pos, tile);
        }

        //int i = 0;
        //foreach (TileData data in mapData.tiles)
        //{
        //    Tile tile = Create(data.type);
        //    tile.tileType = data.type;
        //    tile.Load(data.pos);
        //    tiles.Add(tile.pos, tile);
        //    i++;
        //}
    }
    #endregion
    /*=====================================================*/
    /// Ÿ�� ������Ʈ ���� �� Ŭ����
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        tiles.Clear();
    }
}