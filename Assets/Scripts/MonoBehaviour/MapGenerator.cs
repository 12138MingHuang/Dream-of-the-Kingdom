using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置")]
    public MapConfigSO mapConfigSO;
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    [Header("房间预设体")]
    public Room roomPrefab;
    [Header("连线预设体")]
    public LineRenderer linePrefab;

    private float screenHeight;
    private float screenWidth;

    private float columnWidth;

    private Vector3 generatePoint;

    /// <summary>
    /// 边框偏移
    /// </summary>
    public float Border = 1.2f;

    private List<Room> _roomList = new List<Room>();

    private List<LineRenderer> _lineList = new List<LineRenderer>();

    public List<RoomDataSO> roomDataList = new List<RoomDataSO>();

    private Dictionary<RoomType, RoomDataSO> roomDataDict = new Dictionary<RoomType, RoomDataSO>();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        columnWidth = screenWidth / mapConfigSO.roomBlueprints.Count;

        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }

    private void OnEnable()
    {
        if(mapLayout.mapRoomDataList != null && mapLayout.mapRoomDataList.Count > 0)
        {
            LoadRoom();
        }
        else
        {
            CreateRoom();
        }
    }

    /// <summary>
    /// 生成地图
    /// </summary>
    public void CreateRoom()
    {
        List<Room> previousColumnRooms = new List<Room>();

        for (int column = 0; column < mapConfigSO.roomBlueprints.Count; column++)
        {
            var roomBlueprint = mapConfigSO.roomBlueprints[column];
            var amount = Random.Range(roomBlueprint.min, roomBlueprint.max);

            float roomGapY = screenHeight / (amount + 1); // 一列房间之间的间隔
            float startHeight = screenHeight / 2 - roomGapY; // 起始高度
            generatePoint = new Vector3(-screenWidth / 2 + Border + columnWidth * column, startHeight, 0); // 生成点

            Vector3 newPosition = generatePoint;

            List<Room> currentColumnRooms = new List<Room>();

            for (int i = 0; i < amount; i++)
            {
                if (column == mapConfigSO.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - Border * 2;
                }
                else if (column != 0)
                {
                    newPosition.x = generatePoint.x + Random.Range(-Border / 2, Border / 2);
                }
                newPosition.y = startHeight - roomGapY * i;

                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                RoomType newType = GetRandomRoomType(mapConfigSO.roomBlueprints[column].roomType);
                room.SetRoomData(column, i, GetRoomData(newType));
                _roomList.Add(room);
                currentColumnRooms.Add(room);
            }

            // 判断当前列是否为第一列，如果不是，则连接上一列
            if (previousColumnRooms.Count > 0)
            {
                CreateConnections(previousColumnRooms, currentColumnRooms);
            }

            previousColumnRooms = currentColumnRooms;
        }
        
        SaveRoom();
    }
    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new HashSet<Room>();

        foreach (var room in column1)
        {
            Room targetRoom = ConnectToRandomRoom(room, column2);
            connectedColumn2Rooms.Add(targetRoom);
        }
        foreach (var room in column2)
        {
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1);
            }
        }
    }
    private Room ConnectToRandomRoom(Room room, List<Room> column2)
    {
        Room targetRoom = column2[Random.Range(0, column2.Count)];

        // 创建房间之间的连线
        var line = Instantiate(linePrefab, room.transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        _lineList.Add(line);

        return targetRoom;
    }

    /// <summary>
    /// 重新生成地图
    /// </summary>
    [ContextMenu("重新生成房间")]
    public void ReCreateRoom()
    {
        foreach (var room in _roomList)
        {
            Destroy(room.gameObject);
        }
        foreach (var line in _lineList)
        {
            Destroy(line.gameObject);
        }

        _roomList.Clear();
        _lineList.Clear();
        CreateRoom();
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType roomType)
    {
        string[] options = roomType.ToString().Split(',');
        return (RoomType)Enum.Parse(typeof(RoomType), options[Random.Range(0, options.Length)]);
    }

    private void SaveRoom()
    {
        mapLayout.mapRoomDataList = new List<MapRoomData>();
        
        // 添加所有已经生成的房间数据
        for (int i = 0; i < _roomList.Count; i++)
        {
            var room = new MapRoomData()
            {
                posX = _roomList[i].transform.position.x,
                posY = _roomList[i].transform.position.y,
                colum = _roomList[i].column,
                line = _roomList[i].line,
                roomData = _roomList[i].roomData,
                roomState = _roomList[i].state
            };
            
            mapLayout.mapRoomDataList.Add(room);
        }
        
        mapLayout.linePositionList = new List<LinePosition>();
        // 添加所有已经生成的连线数据
        for (int i = 0; i < _lineList.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = new SerializeVector3(_lineList[i].GetPosition(0)),
                endPos = new SerializeVector3(_lineList[i].GetPosition(1)),
            };
            
            mapLayout.linePositionList.Add(line);
        }
    }
    
    private void LoadRoom()
    {
        // 读取房间数据生成房间
        for (int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        {
            MapRoomData mapRoomData = mapLayout.mapRoomDataList[i];
            var newPos = new Vector3(mapRoomData.posX, mapRoomData.posY, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.state = mapRoomData.roomState;
            newRoom.SetRoomData(mapRoomData.colum, mapRoomData.line, mapRoomData.roomData);
            
            _roomList.Add(newRoom);
        }
        
        // 读取连线数据生成房间连线
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        {
            LinePosition linePosition = mapLayout.linePositionList[i];
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, linePosition.startPos.ToVector3());
            line.SetPosition(1, linePosition.endPos.ToVector3());
            
            _lineList.Add(line);
        }
    }
}
