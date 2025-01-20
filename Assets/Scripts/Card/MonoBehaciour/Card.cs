using System;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText;
    public TextMeshPro descText;
    public TextMeshPro typeText;
    
    public CardDataSO cardData;

    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO cardData)
    {
        this.cardData = cardData;
        cardSprite.sprite = cardData.cardImage;
        costText.text = cardData.cost.ToString();
        descText.text = cardData.description;
        typeText.text = cardData.cardType switch
        {

            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "能力",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}