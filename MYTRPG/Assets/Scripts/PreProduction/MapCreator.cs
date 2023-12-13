using UnityEditor;
using UnityEngine;
// 파일 저장,복사,삭제, 이동관련 기능
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour
{
    /// Tile 프리팹
    [SerializeField] public GameObject tilePrefabs;
    
    /// TileSeletionIndicator 객체
    [SerializeField] GameObject tileSeletionIndicatorPrefab;
    public GameObject tileSeletionIndicator;
    /// 맵의 범위
    /// (맵의 가로 세로 높이 관련)
    [SerializeField] public int width = 10;    // 폭  
    [SerializeField] public int depth = 10;    // 깊이 
    [SerializeField] public int height = 10;   // 높이

    /// 타일 배치 위치
    [HideInInspector] public Point pos;

    /// 현재의 커서가 있는 좌표 반환
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
    // tileSelectionIndicatorPrefab 의 위치를 변경할 때 호출된다.
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

    /// 불러오기 Data
    [SerializeField] MapData mapData;

    /// 타일 배치 정보(좌표, 타일)를 담는 Dictionary 
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();


    [HideInInspector] public Rect CreateArea;
    /// 랜덤 사각형을 반환합니다.
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
    /// 타일 생성
    Tile Create()
    {
        GameObject instance = Instantiate(tilePrefabs) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    /// tiles 배열에 P 좌표의 타일 있는지 확인하고
    /// 있으면 해당 타일을 반환합니다.
    /// 없으면 생성한 뒤에 생성한 타일을 반환합니다.
    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
        {
            return tiles[p];
        }
            

        Tile tile = Create();
        /// Tile.Load 는 타일의 좌표와 스케일 값을 조정
        tile.Load(p, 0);
        tiles.Add(p, tile);

        return tile;
    }
    #endregion
    /*=====================================================*/
    #region Tile BuildUp Function
    /// 타일을 높일지 말지 결정합니다.
    void BuildUpSingle(Point p)
    {
        Tile tile = GetOrCreate(p);
        if (tile.height < height)
            tile.BuildUp();
    }
    /// 단일 타일 건설
    public void BuildUp()
    {
        if(tilePrefabs != null)
        {
            BuildUpSingle(pos);
        }
    }
    /// 랜덤 사각형 범위 내에 타일을 채워놓습니다.
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
    /// 범위 타일 건설 (지정)
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
    /// 범위 타일 건설 (랜덤)
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
    /// 타일을 감축시키거나 삭제
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
    /// 단일 타일 철거
    public void BreakDown()
    {
        BreakDownSingle(pos);
    }
    /// 랜덤 사각형 범위 내의 타일을 감축시킵니다.
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
    /// 범위 타일 철거 (지정)
    public void BreakDownArea()
    {
        CreateArea.x = Mathf.Clamp(CreateArea.x, 0, width);
        CreateArea.y = Mathf.Clamp(CreateArea.y, 0, depth);
        CreateArea.width = Mathf.Clamp(CreateArea.width, 1, width - CreateArea.x + 1);
        CreateArea.height = Mathf.Clamp(CreateArea.height, 1, depth - CreateArea.y + 1);
        BreakDownRect(CreateArea);
    }
    /// 범위 타일 철거 (랜덤)
    public void BreakDownRandomArea()
    {
        Rect area = RandomRect();
        BreakDownRect(area);
    }
    #endregion
    /*=====================================================*/
    #region Save&Load
    /// 경로(폴더)를 만듭니다.
    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
        {   /// Resources 폴더가 없으면 만들기
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        filePath += "/Maps";
        if (!Directory.Exists(filePath))
        {   /// Maps 폴더가 없으면 만들기
            AssetDatabase.CreateFolder("Assets/Resources", "Maps");
        }
        AssetDatabase.Refresh();
    }

    /// 현재 배치 정보를 저장합니다.
    public void Save()
    {
        // 저장할 경로를 string 타입으로 저장합니다.
        string filePath = Application.dataPath + "/Resources/Maps";

        // 디렉토리의 filePath 경로가 있는지 확인합니다.
        if (!Directory.Exists(filePath))
        {
            CreateSaveDirectory();
        }

        MapData map = ScriptableObject.CreateInstance<MapData>();
        map.tiles = new List<Vector3>(tiles.Count);

        // tiles 의 타일의 좌표값을 LevelData board 에 저장합니다.
        foreach (Tile tile in tiles.Values)
        {
            map.tiles.Add(new Vector3(tile.pos.x, tile.height, tile.pos.y));
        }
        // 파일을 저장합니다.
        string fileName = string.Format($"Assets/Resources/Maps/{name}.asset");
        AssetDatabase.CreateAsset(map, fileName);
    }

    /// MapData의 좌표로 타일 배치
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
    /// 타일 오브젝트 삭제 및 클리어
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        tiles.Clear();
    }
}