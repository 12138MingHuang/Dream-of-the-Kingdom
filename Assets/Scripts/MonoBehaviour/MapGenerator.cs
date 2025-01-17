using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public MapConfigSO mapConfigSO;
    public Room roomPrefab;
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
    
    private Dictionary<RoomType, RoomDataSO> roomDataDict  = new Dictionary<RoomType, RoomDataSO>();

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

    private void Start()
    {
        CreateRoom();
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
}
