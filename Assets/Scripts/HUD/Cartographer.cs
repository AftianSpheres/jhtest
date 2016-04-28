using UnityEngine;

enum MapCellType
{
    None,
    RoomExists,
    ActiveRoom
}

public class Cartographer : MonoBehaviour
{
    public WorldController world;
    public Sprite[] cellSprites;
    public Sprite[] connectorSprites;
    public SpriteRenderer[,] cells;
    public GameObject prefab;
    public Vector3 startingCellPos;
    public Vector2 cellOffset;
    public int mapHeight;
    public int mapWidth;
    public int centerHorizIndex;
    public int centerVertIndex;
    private MapCellType[,] mapCells;
    private SubregionType mapSubregion;

	void Start ()
    {
        mapCells = new MapCellType[mapHeight, mapWidth];
        cells = new SpriteRenderer[mapHeight, mapWidth];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                SpriteRenderer s = Instantiate(prefab).GetComponent<SpriteRenderer>();
                s.transform.parent = transform;
                s.transform.localPosition = startingCellPos + new Vector3(x * cellOffset.x, y * cellOffset.y, 0);
                cells[y, x] = s;
                #if UNITY_EDITOR
                s.name = "MapCell " + x.ToString() + " , " + y.ToString();
                #endif
            }
        }
    }

    #if UNITY_EDITOR
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) == true)
        {
            Refresh();
            Debug.Log("Manually refreshed map attached to " + gameObject.name);
        }
	}
    #endif

    MapCellType[,] RecalcMapState ()
    {
        MapCellType[,] tempBuffer = new MapCellType[mapHeight, mapWidth];
        if (world.activeRoom != null && world.activeRoom.Subregion != SubregionType.None)
        {
            mapSubregion = world.activeRoom.Subregion;
            int i;
            for (int y_mod = -1 * centerVertIndex; y_mod <= centerVertIndex; y_mod++)
            {
                for (i = -1 * centerHorizIndex; i <= centerHorizIndex; i++)
                {
                    tempBuffer = in_RecalcMapState(y_mod, i, tempBuffer);
                }
            }
        }
        return tempBuffer;
    }

    MapCellType[,] in_RecalcMapState(int y_mod, int i, MapCellType[,] tempBuffer)
    {
        int modifiedY = (int)(world.player.CurrentWorldCoords[0] + y_mod);
        int modifiedX = (int)(world.player.CurrentWorldCoords[1] + i);
        if ((modifiedY > -1 && modifiedY < world.WorldSize_Y) && (modifiedX > -1 && modifiedX < world.WorldSize_X))
        {
            if (world.rooms[modifiedY, modifiedX] != null && world.rooms[modifiedY, modifiedX].Subregion == mapSubregion)
            {
                tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i] = MapCellType.RoomExists;
            }
            else
            {
                tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i] = MapCellType.None;
            }
        }
        else
        {
            tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i] = MapCellType.None;
        }
        return (tempBuffer);
    }

    void RedrawMap ()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (x == centerHorizIndex && y == centerVertIndex)
                {

                    mapCells[y, x] = MapCellType.ActiveRoom;
                }
                cells[y, x].sprite = cellSprites[(int)mapCells[y, x]];
            }
        }
    }

    public void Refresh()
    {
        MapCellType[,] tempBuffer = RecalcMapState();
        for (int y = 0; y < tempBuffer.GetLength(0); y++)
        {
            for (int x = 0; x < tempBuffer.GetLength(1); x++)
            {
                if (tempBuffer[y, x] != mapCells[y, x])
                {
                    mapCells = tempBuffer;
                    RedrawMap();
                    return;
                }
            }
        }
    }
}
