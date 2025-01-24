using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RestPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton;
    private Button backToMapButton;

    public Effect restEffect;

    private CharacterBase player;
    public ObjectEventSO loadMapEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>("RestButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");

        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
        
        restButton.clicked += OnRestButtonClicked;
        backToMapButton.clicked += OnBackToMapButtonClicked;
    }
    private void OnBackToMapButtonClicked()
    {
        loadMapEvent?.RaiseEvent(null, this);
    }
    private void OnRestButtonClicked()
    {
        restEffect.Execute(player, null);
        restButton.SetEnabled(false);
    }

    private void OnDisable()
    {
        restButton.clicked -= OnRestButtonClicked;
        backToMapButton.clicked -= OnBackToMapButtonClicked;
    }
}
