
using System;

[Flags]
public enum RoomType
{
    /// <summary>
    /// 普通敌人
    /// </summary>
    MinorEnemy = 1,
    /// <summary>
    /// 精英敌人
    /// </summary>
    EliteEnemy = 2,
    /// <summary>
    /// 商店
    /// </summary>
    Shop = 4,
    /// <summary>
    /// 宝箱
    /// </summary>
    Treasure = 8,
    /// <summary>
    /// 休息室
    /// </summary>
    RestRoom = 16,
    /// <summary>
    /// boss
    /// </summary>
    Boss = 32,
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

public enum CardType
{
    /// <summary>
    /// 攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 防御
    /// </summary>
    Defense,
    /// <summary>
    /// 能力
    /// </summary>
    Abilities
}