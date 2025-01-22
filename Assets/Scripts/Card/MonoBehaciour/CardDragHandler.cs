using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Card currentCard;
    private bool canMove;
    private bool canExecute;
    
    public GameObject arrowPrefab;
    private GameObject currentArrow;
    
    private CharacterBase targetCharacter;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }

    private void OnDisable()
    {
        canExecute = false;
        canMove = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (currentCard.cardData.cardType)
        {

            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            case CardType.Defense:
            case CardType.Abilities:
                canMove = true;
                break;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            
            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
        else
        {
            if(eventData.pointerEnter == null) return;

            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(currentArrow != null)
            Destroy(currentArrow);
        
        if (canExecute)
        {
            currentCard.ExecuteCardEffects(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(), targetCharacter);
        }
        else
        {
            currentCard.RestCardTransform();
            currentCard.isAnimating = false;
        }
    }
}