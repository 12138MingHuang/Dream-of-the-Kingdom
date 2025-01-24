using System;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    private bool isPlayerTurn = false;
    private bool isEnemyTurn = false;

    public bool battleEnd = true;

    private float timeCounter;
    public float enemyTurnDuration;
    public float playerTurnDuration;

    public GameObject playerObj;
    
    [Header("事件广播")]
    public ObjectEventSO playerTurnBeginEvent;
    public ObjectEventSO enemyTurnBeginEvent;
    public ObjectEventSO enemyTurnEndEvent;

    private void Update()
    {
        if(battleEnd) return;

        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0;
                EnemyTurnEnd();
                isPlayerTurn = true;
            }
        }

        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0;
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }

    [ContextMenu("游戏开始")]
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnemyTurn = false;
        battleEnd = false;
        timeCounter = 0;
    }

    public void PlayerTurnBegin()
    {
        playerTurnBeginEvent?.RaiseEvent(null, this);
    }
    
    public void EnemyTurnBegin()
    {
        isEnemyTurn = true;
        enemyTurnBeginEvent?.RaiseEvent(null, this);
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn = false;
        enemyTurnEndEvent?.RaiseEvent(null, this);
    }

    public void OnRoomLoadedEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.roomData.roomType)
        {

            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                playerObj.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                playerObj.SetActive(false);
                break;
            case RoomType.RestRoom:
                playerObj.SetActive(true);
                break;
        }
    }

    public void StopTurnBaseSystem(object obj)
    {
        battleEnd = true;
        playerObj.SetActive(false);
    }

    public void NewGame()
    {
        playerObj.GetComponent<Player>().NewGame();
    }
}
