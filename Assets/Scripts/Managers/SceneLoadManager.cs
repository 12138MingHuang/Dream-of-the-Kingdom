using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    
    public AssetReference mapScene;
    
    public async void OnLoadRoomEvent(object data)
    {
        if (data is RoomDataSO)
        {
            RoomDataSO currentRoom = (RoomDataSO)data;
            currentScene = currentRoom.sceneToLoad;
            
            Debug.Log($"Loaded room: {currentRoom.roomType}");
        }

        // 卸载当前激活场景
        await UnLoadSceneTask();
        
        // 加载场景
        await LoadSceneTask();
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
        
        currentScene = mapScene;
        await LoadSceneTask();
    }
}