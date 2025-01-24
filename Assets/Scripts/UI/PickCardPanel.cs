using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement cardContainer;
    private Button confirmButton;
    private CardDataSO currentCardData;
    
    public VisualTreeAsset cardTemplate;
    public CardManager cardManager;

    private List<Button> cardButtons = new List<Button>();

    [Header("广播事件")]
    public ObjectEventSO finishPickCardEvent;
    
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");
        
        confirmButton.clicked += OnConfirmButtonClicked;

        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            CardDataSO cardData = cardManager.GetNewCardData();
            InitCard(card, cardData);
            var cardButton = card.Q<Button>("Card");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardButtonClicked(cardButton, cardData);
        }
    }
    private void OnConfirmButtonClicked()
    {
        cardManager.UnlockCard(currentCardData);
        finishPickCardEvent?.RaiseEvent(null, this);
    }
    private void OnCardButtonClicked(Button button, CardDataSO data)
    {
        currentCardData = data;

        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (cardButtons[i] == button)
            {
                cardButtons[i].SetEnabled(false);
            }
            else
            {
                cardButtons[i].SetEnabled(true);
            }
        }
        
        Debug.Log(currentCardData.name);
    }

    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        currentCardData = cardData;
        
        VisualElement cardSpriteElement = card.Q<VisualElement>("CardSprite");
        Label cardCost = card.Q<Label>("EnergyCost");
        Label cardName = card.Q<Label>("CardName");
        Label cardDesc = card.Q<Label>("CardDescription");
        Label cardType = card.Q<Label>("CardType");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardName.text = cardData.name;
        cardCost.text = cardData.cost.ToString();
        cardDesc.text = cardData.description;
        cardType.text = cardData.cardType switch
        {

            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "能力",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
