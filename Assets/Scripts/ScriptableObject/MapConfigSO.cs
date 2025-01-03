using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO", order = 0)]
public class MapConfigSO : ScriptableObject
{
    [Header("房间列数")]
    public List<RoomBlueprint> roomBlueprints = new List<RoomBlueprint>();
}

[System.Serializable]
public class RoomBlueprint
{
    [Header("该列的房间最小值(开区间)")]
    public int min;
    [Header("该列的房间最大值(闭区间)")]
    public int max;
    [Header("房间类型")]
    public RoomType roomType;
}
