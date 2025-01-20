using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    
    public void UpdateMapLayoutData(object value)
    {
        var roomVector = (Vector2Int)value;
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
    }
}
