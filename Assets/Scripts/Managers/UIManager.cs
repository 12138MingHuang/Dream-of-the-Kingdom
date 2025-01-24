using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI面板")]
    public GameObject gamePlayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restPanel;

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;

        switch (currentRoom.roomData.roomType)
        {

            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gamePlayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                restPanel.SetActive(true);
                break;
        }
    }

    public void HideAllPanels()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    
    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }
    
    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}