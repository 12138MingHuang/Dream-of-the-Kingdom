using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText;
    public TextMeshPro descText;
    public TextMeshPro typeText;
    
    public CardDataSO cardData;

    public Vector3 orginalPosition;
    public Quaternion originalRotation;
    public int orginalLayerOrder;
    
    public bool isAnimating;

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
    
    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        orginalPosition = position;
        originalRotation = rotation;
        orginalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating) return;
        
        transform.position = orginalPosition + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 20;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating) return;
        
        RestCardTransform();
    }
    
    public void RestCardTransform()
    {
        transform.SetPositionAndRotation(orginalPosition, originalRotation);
    }
}