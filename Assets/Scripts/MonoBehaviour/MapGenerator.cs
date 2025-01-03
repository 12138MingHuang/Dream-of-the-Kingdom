using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public MapConfigSO mapConfigSO;
    public Room roomPrefab;
    
    private float screenHeight;
    private float screenWidth;
    
    private float columnWidth;
    
    private Vector3 generatePoint;
    
    /// <summary>
    /// 边框偏移
    /// </summary>
    public float Border = 1.2f;
    
    private List<Room> _roomList = new List<Room>();

    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        
        columnWidth = screenWidth / mapConfigSO.roomBlueprints.Count;
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
        for (int column = 0; column < mapConfigSO.roomBlueprints.Count; column++)
        {
            var roomBlueprint = mapConfigSO.roomBlueprints[column];
            var amount = Random.Range(roomBlueprint.min, roomBlueprint.max);

            float roomGapY = screenHeight / (amount + 1); // 一列房间之间的间隔
            float startHeight = screenHeight / 2 - roomGapY; // 起始高度
            generatePoint = new Vector3(-screenWidth / 2 + Border + columnWidth * column, startHeight, 0); // 生成点
            
            Vector3 newPosition = generatePoint;
            
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
                _roomList.Add(room);
            }
        }
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
        _roomList.Clear();
        CreateRoom();
    }
}
