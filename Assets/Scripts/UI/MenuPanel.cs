using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton;
    private Button quitButton;
    
    public ObjectEventSO newGameEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        quitButton = rootElement.Q<Button>("QuitGameButton");

        newGameButton.clicked += OnNewGameButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
    }
    private void OnNewGameButtonClicked()
    {
        newGameEvent?.RaiseEvent(null, this);
    }
    private void OnQuitButtonClicked() => Application.Quit();
}
