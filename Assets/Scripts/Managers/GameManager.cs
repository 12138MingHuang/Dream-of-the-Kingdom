using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    public List<Enemy> aliveEnemyList = new List<Enemy>();
    
    [Header("事件广播")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;
    
    public void UpdateMapLayoutData(object value)
    {
        var roomVector = (Vector2Int)value;
        if(mapLayout.mapRoomDataList.Count == 0)
            return;
        // 更新当前房间的数据(解锁相邻房间, 并标记为已访问
        var currentRoom = mapLayout.mapRoomDataList.Find(room => room.colum == roomVector.x && room.line == roomVector.y);
        currentRoom.roomState = RoomState.Visited;
        // 更新相邻房间的数据
        var neighborRooms = mapLayout.mapRoomDataList.FindAll(room => room.colum == currentRoom.colum);

        foreach (var neighborRoom in neighborRooms)
        {
            if(neighborRoom.line != roomVector.y)
                neighborRoom.roomState = RoomState.Locked;
        }

        foreach (var link in currentRoom.linkToRooms)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(room => room.colum == link.x && room.line == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }
        
        aliveEnemyList.Clear();
    }

    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }
    
    public void OnCharacterDeadEvent(object character)
    {
        if (character is Player)
        {
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        
        if(character is Enemy)
        {
            aliveEnemyList.Remove(character as Enemy);
            if(aliveEnemyList.Count == 0)
            {
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO?.RaiseEvent(null, this);
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}
