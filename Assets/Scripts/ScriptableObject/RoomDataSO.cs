using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Map/RoomDataSO", order = 0)]
public class RoomDataSO : ScriptableObject
{
    public Sprite roomIcon;
    public RoomType roomType;
    public AssetReference sceneToLoad;
}
