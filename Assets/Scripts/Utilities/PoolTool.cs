using System;
using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;

    private ObjectPool<GameObject> pool;

    private void Start()
    {
        // 创建一个对象池，用于管理 GameObject 的生命周期
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab, transform),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        PreFillPool(7);
    }
    
    private void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];

        for (int i = 0; i < count; ++i)
        {
            preFillArray[i] = pool.Get();
        }

        foreach (var item in preFillArray)
        {
            pool.Release(item);
        }
    }
    
    /// <summary>
    /// 从对象池中获取一个 GameObject
    /// </summary>
    /// <returns> GameObject </returns>
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }

    /// <summary>
    /// 将 GameObject 返回对象池中
    /// </summary>
    /// <param name="obj"> GameObject </param>
    public void ReturnObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}