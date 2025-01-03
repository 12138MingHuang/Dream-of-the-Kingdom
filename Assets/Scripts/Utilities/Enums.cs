public enum RoomType
{
    /// <summary>
    /// 普通敌人
    /// </summary>
    MinorEnemy,
    /// <summary>
    /// 精英敌人
    /// </summary>
    EliteEnemy,
    /// <summary>
    /// 商店
    /// </summary>
    Shop,
    /// <summary>
    /// 宝箱
    /// </summary>
    Treasure,
    /// <summary>
    /// 休息室
    /// </summary>
    RestRoom,
    /// <summary>
    /// boss
    /// </summary>
    Boss
}

public enum RoomState
{
    /// <summary>
    /// 未探索
    /// </summary>
    Locked,
    /// <summary>
    /// 已探索
    /// </summary>
    Visited,
    /// <summary>
    /// 可到达
    /// </summary>
    Attainable
}