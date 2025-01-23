using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    
    public AssetReference mapScene;
    
    private Vector2Int currentRoomVector;
    private Room currentRoom;

    [Header("广播")]
    public ObjectEventSO afterRoomLoadedEvent;
    public ObjectEventSO updateRoomEvent;

    private void Start()
    {
        currentRoomVector = Vector2Int.one * -1;
    }

    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = (Room)data;
            RoomDataSO currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.line);
            currentScene = currentData.sceneToLoad;
            
            Debug.Log($"Loaded room: {currentData.roomType}");
        }

        // 卸载当前激活场景
        await UnLoadSceneTask();
        
        // 加载场景
        await LoadSceneTask();
        
        afterRoomLoadedEvent.RaiseEvent(currentRoom, this);
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    private async Awaitable LoadSceneTask()
    {
        var scene = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await scene.Task;
        
        if(scene.Status == AsyncOperationStatus.Succeeded)
            SceneManager.SetActiveScene(scene.Result.Scene);
    }

    /// <summary>
    /// 卸载当前激活场景
    /// </summary>
    private async Awaitable UnLoadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// 加载地图场景，并卸载当前激活场景
    /// </summary>
    public async void LoadMap()
    {
        await UnLoadSceneTask();

        if (currentRoomVector != Vector2Int.one * -1)
        {
            updateRoomEvent?.RaiseEvent(currentRoomVector, this);
        }
        
        currentScene = mapScene;
        await LoadSceneTask();
    }
}