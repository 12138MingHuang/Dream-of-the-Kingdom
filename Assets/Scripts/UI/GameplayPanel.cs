using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Label energyAmountLabel;
    private Label drawAmountLabel;
    private Label discardAmountLabel;
    private Label turnLabel;
    private Button endTurnButton;

    [Header("事件广播")]
    public ObjectEventSO playerTurnEndEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        
        endTurnButton = rootElement.Q<Button>("TurnButton");
        energyAmountLabel = rootElement.Q<Label>("EnergyAmount");
        drawAmountLabel = rootElement.Q<Label>("DrawAmount");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
        turnLabel = rootElement.Q<Label>("TurnLabel");
        
        endTurnButton.clicked += OnEndTurnButtonClicked;
        
        energyAmountLabel.text = "0";
        drawAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        turnLabel.text = "游戏开始";
    }
    private void OnEndTurnButtonClicked()
    {
        playerTurnEndEvent?.RaiseEvent(null, this);
    }

    private void OnDisable()
    {
        endTurnButton.clicked -= OnEndTurnButtonClicked;
    }


    public void UpdateDrawDeckAmount(int amount)
    {
        drawAmountLabel.text = $"{amount}";
    }

    public void UpdateDiscardDeckAmount(int amount)
    {
        discardAmountLabel.text = $"{amount}";
    }
}