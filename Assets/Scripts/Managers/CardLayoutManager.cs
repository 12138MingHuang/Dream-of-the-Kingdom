using System;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    public Vector3 centerPoint;
    [Header("弧形参数")]
    public float angleBetweenCards = 7f;
    public float radius = 17f;
    
    [SerializeField]
    private List<Vector3> cardPositions = new List<Vector3>();
    private List<Quaternion> cardRotations = new List<Quaternion>();

    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    /// <summary>
    /// 获取卡片位置和旋转信息
    /// </summary>
    /// <param name="index"> 卡片索引</param>
    /// <param name="totalCards"> 卡片总数</param>
    /// <returns> 卡片位置和旋转信息</returns>
    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculateCardPosition(totalCards, isHorizontal);

        return new CardTransform(cardPositions[index], cardRotations[index]);
    }
    
    private void CalculateCardPosition(int numberOfCards, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        
        if (horizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);
            
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);
                
                var pos = new Vector3(xPos, centerPoint.y, 0f);
                var rotation = Quaternion.identity;
                
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            float cardAngle = (numberOfCards - 1) * angleBetweenCards / 2;

            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(cardAngle - i * angleBetweenCards);
                var rotation = Quaternion.Euler(0f, 0f, cardAngle - angleBetweenCards * i);
                
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0f);
    }
}