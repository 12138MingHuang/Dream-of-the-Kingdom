using System;
using UnityEngine;

[Serializable]
public class SerializeVector3
{
    public float x;
    public float y;
    public float z;

    public SerializeVector3(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    /// <summary>
    /// 序列化的Vector3转化为普通的Vector3类型
    /// </summary>
    /// <returns> Vector3 </returns>
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    
    /// <summary>
    /// 序列化的Vector3转化为普通的Vector2Int类型
    /// </summary>
    /// <returns> Vector2Int </returns>
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}