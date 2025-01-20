using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList;
    public List<LinePosition> linePositionList;
}

[System.Serializable]
public class MapRoomData
{
    public float posX;
    public float posY;
    public int colum;
    public int line;
    public RoomDataSO roomData;
    public RoomState roomState;
}

[System.Serializable]
public class LinePosition
{
    /// <summary>
    /// 起点坐标
    /// </summary>
    public SerializeVector3 startPos;
    /// <summary>
    /// 终点坐标
    /// </summary>
    public SerializeVector3 endPos;
}
