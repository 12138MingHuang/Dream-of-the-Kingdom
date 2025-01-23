﻿using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI面板")]
    public GameObject gamePlayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;

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
                break;
        }
    }

    public void HideAllPanels()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
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
}