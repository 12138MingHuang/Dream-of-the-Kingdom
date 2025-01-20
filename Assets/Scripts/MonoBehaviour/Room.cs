using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    /// <summary>
    /// 房间行列号，用于定位房间在地图上的位置
    /// </summary>
    public int column;
    /// <summary>
    /// 房间行列号，用于定位房间在地图上的位置
    /// </summary>
    public int line;
    /// <summary>
    /// 房间数据，包括房间图标和房间类型等信息
    /// </summary>
    public RoomDataSO roomData;
    /// <summary>
    /// 房间状态，包括房间是否已经通过
    /// </summary>
    public RoomState state;

    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        SetRoomData(0, 0, roomData);
    }

    private void OnMouseDown()
    {
        Debug.Log($"点击了房间：{roomData.roomType}");
        loadRoomEvent.RaiseEvent(roomData, this);
    }

    /// <summary>
    /// 设置房间数据，包括行列号和房间数据
    /// </summary>
    /// <param name="column"> 列号</param>
    /// <param name="line"> 行号</param>
    /// <param name="roomData"> 房间数据</param>
    public void SetRoomData(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        
        _spriteRenderer.sprite = roomData.roomIcon;
    }
}
