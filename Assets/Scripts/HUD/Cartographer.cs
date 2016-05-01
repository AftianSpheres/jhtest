using UnityEngine;

enum MapCellType
{
    None,
    RoomExists,
    ActiveRoom
}

enum ConnectorType
{
    None,
    Connected,
    Fused,
    ActiveConnected,
    ActiveFused
}

/// <summary>
/// HERE THERE BE DRAGONS
/// I don't remember writing half this shit and I'm not entirely sure the Mad Programmer COBOL Alhazred isn't the actual author
/// </summary>
public class Cartographer : MonoBehaviour
{
    public WorldController world;
    public Sprite[] cellSprites;
    public Sprite[] connectorSprites;
    public SpriteRenderer[,] cells;
    public SpriteRenderer[,] horizConnections;
    public SpriteRenderer[,] vertConnections;
    public GameObject prefab;
    public Vector3 startingCellPos;
    public Vector3 UpConnectorOffset;
    public Vector3 RightConnectorOffset;
    public Vector2 cellOffset;
    public int mapHeight;
    public int mapWidth;
    public int centerHorizIndex;
    public int centerVertIndex;
    private MapCellType[,] mapCells;
    private ConnectorType[,] vertConnectors;
    private ConnectorType[,] horizConnectors;
    private SubregionType mapSubregion;

	void Start ()
    {
        mapCells = new MapCellType[mapHeight, mapWidth];
        cells = new SpriteRenderer[mapHeight, mapWidth];
        vertConnectors = new ConnectorType[mapHeight + 1, mapWidth];
        vertConnections = new SpriteRenderer[mapHeight + 1, mapWidth];
        horizConnectors = new ConnectorType[mapHeight, mapWidth + 1];
        horizConnections = new SpriteRenderer[mapHeight, mapWidth + 1];
        for (int y = 0; y <= mapHeight; y++)
        {
            for (int x = 0; x <= mapWidth; x++)
            {
                if (x < mapWidth && y < mapHeight)
                {
                    SpriteRenderer s = default(SpriteRenderer);
                    s = Instantiate(prefab).GetComponent<SpriteRenderer>();
                    s.transform.parent = transform;
                    s.transform.localPosition = startingCellPos + new Vector3(x * cellOffset.x, y * cellOffset.y, 0);
                    cells[y, x] = s;
                    #if UNITY_EDITOR
                    s.name = "MapCell " + x.ToString() + " , " + y.ToString();
                    #endif
                }


                // connectors

                SpriteRenderer c;

                if (y < mapHeight)
                {
                    c = Instantiate(prefab).GetComponent<SpriteRenderer>();
                    c.transform.parent = transform;
                    c.transform.localPosition = startingCellPos + new Vector3((x * cellOffset.x) - 1, y * cellOffset.y, 0);
                    horizConnections[y, x] = c;
                    #if UNITY_EDITOR
                    c.name = "HorizConnector: " + x.ToString() + " , " + y.ToString();
                    #endif
                }
                if (x < mapWidth)
                {
                    c = Instantiate(prefab).GetComponent<SpriteRenderer>();
                    c.transform.parent = transform;
                    c.transform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
                    c.transform.localPosition = startingCellPos + new Vector3(x * cellOffset.x, (y - 1) * cellOffset.y, 0);
                    vertConnections[y, x] = c;
                    #if UNITY_EDITOR
                    c.name = "VertConnector: " + x.ToString() + " , " + y.ToString();
                    #endif
                }
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

    MapCellType[,] RecalcMapState (out ConnectorType[,] horizConnectors, out ConnectorType[,] vertConnectors)
    {
        MapCellType[,] tempBuffer = new MapCellType[mapHeight, mapWidth];
        horizConnectors = new ConnectorType[mapHeight, mapWidth + 1];
        vertConnectors = new ConnectorType[mapHeight + 1, mapWidth];
        if (world.activeRoom != null && world.activeRoom.Subregion != SubregionType.None)
        {
            mapSubregion = world.activeRoom.Subregion;
            int i;
            for (int y_mod = -1 * centerVertIndex; y_mod <= centerVertIndex + 1; y_mod++)
            {
                for (i = -1 * centerHorizIndex; i <= centerHorizIndex + 1; i++)
                {
                    tempBuffer = in_RecalcMapState_Cells(y_mod, i, tempBuffer);
                    horizConnectors = in_RecalcMapState_Connectors(y_mod, i, horizConnectors, true);
                    vertConnectors = in_RecalcMapState_Connectors(y_mod, i, vertConnectors);
                }
            }
        }
        return tempBuffer;
    }

    ConnectorType[,] in_RecalcMapState_Connectors(int y_mod, int i, ConnectorType[,] tempBuffer, bool chkHorizontal = false) 
    {
        int hb = 0;
        int vb = 0;
        if (chkHorizontal == true)
        {
            vb = 1;
        }
        else
        {
            hb = 1;
        }
        if (y_mod <= centerVertIndex + vb + 1 && i <= centerHorizIndex + hb + 1)
        {
            int modifiedY = (int)(world.player.CurrentWorldCoords[0] + y_mod);
            int modifiedX = (int)(world.player.CurrentWorldCoords[1] + i);
            if (modifiedX > 0 && modifiedY > 0 && modifiedX <= world.WorldSize_X && modifiedY <= world.WorldSize_Y)
            {
                RoomController room = world.rooms[modifiedY - 1, modifiedX - 1];
                RoomController roomAbove = default(RoomController);
                if (modifiedY < world.WorldSize_Y)
                {
                    roomAbove = world.rooms[modifiedY, modifiedX - 1];
                }
                RoomController roomRight = default(RoomController);
                if (modifiedX < world.WorldSize_X)
                {
                    roomRight = world.rooms[modifiedY - 1, modifiedX];
                }
                if (chkHorizontal == true && room != null && room.Subregion == mapSubregion)
                {
                    if (room.rightSideConnections.Length > 0 && roomRight != null)
                    {
                        for (int i2 = 0; i2 < room.rightSideConnections.Length; i2++)
                        {
                            if (modifiedY - 1 + i2 >= world.WorldSize_Y)
                            {
                                break;
                            }
                            roomRight = world.rooms[modifiedY - 1 + i2, modifiedX];
                            if (centerVertIndex + y_mod + i2 - vb < 0 || centerVertIndex + y_mod + i2 - vb >= tempBuffer.GetLength(0))
                            {
                                break;
                            }
                            else if (room == roomRight)
                            {
                                if (modifiedY == world.player.CurrentWorldCoords[0] + vb && (modifiedX == world.player.CurrentWorldCoords[1] + hb || modifiedX - 1 == world.player.CurrentWorldCoords[1] + hb))
                                {
                                    tempBuffer[centerVertIndex + y_mod + i2 - vb, centerHorizIndex + i] = ConnectorType.ActiveFused;
                                }
                                else
                                {
                                    tempBuffer[centerVertIndex + y_mod + i2 - vb, centerHorizIndex + i] = ConnectorType.Fused;
                                }
                            }
                            else if (room.rightSideConnections[i2] == true)
                            {
                                if (modifiedY == world.player.CurrentWorldCoords[0] + vb && (modifiedX == world.player.CurrentWorldCoords[1] + hb || modifiedX - 1 == world.player.CurrentWorldCoords[1] + hb))
                                {
                                    tempBuffer[centerVertIndex + y_mod + i2 - vb, centerHorizIndex + i] = ConnectorType.ActiveConnected;
                                }
                                else
                                {
                                    tempBuffer[centerVertIndex + y_mod + i2 - vb, centerHorizIndex + i] = ConnectorType.Connected;
                                }
                            }
                            else
                            {
                                tempBuffer[centerVertIndex + y_mod + i2 - vb, centerHorizIndex + i] = ConnectorType.None;
                            }
                        }
                    }
                }
                else if (room != null && room.Subregion == mapSubregion)
                {
                    if (room.topSideConnections.Length > 0 && roomAbove != null)
                    {
                        for (int i2 = 0; i2 < room.topSideConnections.Length; i2++)
                        {
                            if (modifiedX - 1 + i2 >= world.WorldSize_X)
                            {
                                break;
                            }
                            roomAbove = world.rooms[modifiedY, modifiedX - 1 + i2];
                            if (centerHorizIndex + i + i2 - hb < 0 || centerHorizIndex + i + i2 - hb >= tempBuffer.GetLength(1))
                            {
                                break;
                            }
                            else if (room == roomAbove)
                            {
                                if (modifiedX == world.player.CurrentWorldCoords[1] + hb && (modifiedY == world.player.CurrentWorldCoords[0] + vb || modifiedY - 1 == world.player.CurrentWorldCoords[0] + vb))
                                {
                                    tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i + i2 - hb] = ConnectorType.ActiveFused;
                                }
                                else
                                {
                                    tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i + i2 - hb] = ConnectorType.Fused;
                                }
                            }
                            else if (room.topSideConnections[i2] == true)
                            {
                                if (modifiedX == world.player.CurrentWorldCoords[1] + hb && (modifiedY == world.player.CurrentWorldCoords[0] + vb || modifiedY - 1 == world.player.CurrentWorldCoords[0] + vb))
                                {
                                    tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i + i2 - hb] = ConnectorType.ActiveConnected;
                                }
                                else
                                {
                                    tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i + i2 - hb] = ConnectorType.Connected;
                                }
                            }
                            else
                            {
                                tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i + i2 - hb] = ConnectorType.None;
                            }
                        }
                    }
                }
                else if (centerVertIndex + y_mod - vb > -1 && centerVertIndex + y_mod - vb < tempBuffer.GetLength(0) && centerHorizIndex + i - hb > -1 && centerHorizIndex + i - hb < tempBuffer.GetLength(1))
                {
                    tempBuffer[centerVertIndex + y_mod - vb, centerHorizIndex + i - hb] = ConnectorType.None;
                    
                }
            }
        }
        return tempBuffer;
    }

    MapCellType[,] in_RecalcMapState_Cells(int y_mod, int i, MapCellType[,] tempBuffer)
    {
        if (y_mod <= centerVertIndex && i <= centerHorizIndex)
        {
            int modifiedY = (int)(world.player.CurrentWorldCoords[0] + y_mod);
            int modifiedX = (int)(world.player.CurrentWorldCoords[1] + i);
            if ((modifiedY > -1 && modifiedY < world.WorldSize_Y) && (modifiedX > -1 && modifiedX < world.WorldSize_X))
            {
                if (world.rooms[modifiedY, modifiedX] != null && world.rooms[modifiedY, modifiedX].Subregion == mapSubregion)
                {
                    if (y_mod == 0 && i == 0)
                    {
                        tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i] = MapCellType.ActiveRoom;
                    }
                    else
                    {
                        tempBuffer[centerVertIndex + y_mod, centerHorizIndex + i] = MapCellType.RoomExists;
                    }
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
        }
        return (tempBuffer);
    }

    void RedrawMap ()
    {
        for (int y = 0; y <= mapHeight; y++)
        {
            for (int x = 0; x <= mapWidth; x++)
            {
                if (x < mapWidth && y < mapHeight)
                {
                    cells[y, x].sprite = cellSprites[(int)mapCells[y, x]];
                }
                if (x < mapWidth)
                {
                    vertConnections[y, x].sprite = connectorSprites[(int)vertConnectors[y, x]];
                }
                if (y < mapHeight)
                {
                    horizConnections[y, x].sprite = connectorSprites[(int)horizConnectors[y, x]];
                }
            }
        }
    }

    public void Refresh()
    {
        ConnectorType[,] horizConBuffer;
        ConnectorType[,] vertConBuffer;
        MapCellType[,] cellsBuffer = RecalcMapState(out horizConBuffer, out vertConBuffer);
        for (int y = 0; y < cellsBuffer.GetLength(0); y++)
        {
            for (int x = 0; x < cellsBuffer.GetLength(1); x++)
            {
                if (cellsBuffer[y, x] != mapCells[y, x])
                {
                    horizConnectors = horizConBuffer;
                    vertConnectors = vertConBuffer;
                    mapCells = cellsBuffer;
                    RedrawMap();
                    return;
                }
            }
        }
    }
}
