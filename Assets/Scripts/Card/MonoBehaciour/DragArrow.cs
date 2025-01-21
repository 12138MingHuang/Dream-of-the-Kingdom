using System;
using UnityEngine;

public class DragArrow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int pointsCount;
    public float arcModifier;
    private Vector3 mousePos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        
        SetArrowPosition();
    }

    private void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position; // 卡牌位置
        Vector3 direction = mousePos - cardPosition; //从卡牌指向鼠标位置
        Vector3 normalizedDirection = direction.normalized; // 方向向量
        
        // 计算垂直于卡牌到鼠标方向的向量
        Vector3 perpendicular = new Vector3(-normalizedDirection.y, normalizedDirection.x, normalizedDirection.z);
        
        // 设置控制点的偏移量
        Vector3 offset = perpendicular * arcModifier; // 调整这个值来该表箭头弯曲的程度
        
        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset; // 控制点

        lineRenderer.positionCount = pointsCount; // 设置线段点数

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i / (pointsCount - 1f);
            Vector3 point = CalculateQuadraticBezierPoint(t, cardPosition, controlPoint, mousePos);
            lineRenderer.SetPosition(i, point);
        }
    }

    /// <summary>
    /// 计算二次贝塞尔曲线的点。二次贝塞尔曲线由三个点定义：起点、控制点和终点。
    /// </summary>
    /// <param name="t"> 介于0和1之间的参数，用于确定曲线上的点。</param>
    /// <param name="p0"> 起点。</param>
    /// <param name="p1"> 控制点。</param>
    /// <param name="p2"> 终点。</param>
    /// <returns> 曲线上的点。</returns>
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        
        Vector3 p = uu * p0; // 第一项
        p += 2 * uu * t * p1; // 第二项
        p += tt * p2; // 第三项
        
        return p;
    }
}