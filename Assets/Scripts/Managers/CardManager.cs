using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    // 游戏中所有可能出现的卡牌
    public List<CardDataSO> cardDataList;
    
    [Header("卡牌库")]
    public CardLibrarySO newGameCardLibrary; // 新游戏时初始化的卡牌库
    public CardLibrarySO currentCardLibrary; // 当前玩家使用的卡牌库

    private int previousIndex;

    private void Awake()
    {
        InitializeCardDataList();

        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentCardLibrary.cardLibraryList.Add(item);
        }
    }

    private void OnDisable()
    {
        currentCardLibrary.cardLibraryList.Clear();
    }

    #region 获取项目卡牌
    /// <summary>
    /// 初始化获得所有项目卡牌资源
    /// </summary>
    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("Failed to load card data.");
        }
    }
    #endregion

    /// <summary>
    /// 获取卡牌对象，从对象池中获取
    /// </summary>
    /// <returns> 卡牌对象 </returns>
    public GameObject GetCardObject()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }
    /// <summary>
    /// 丢弃卡牌，将其返回对象池中
    /// </summary>
    /// <param name="cardObj"> 卡牌对象 </param>
    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReturnObjectToPool(cardObj);
    }

    public CardDataSO GetNewCardData()
    {
        int randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, cardDataList.Count);
        } while (previousIndex == randomIndex);
        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    public void UnlockCard(CardDataSO cardData)
    {
        CardLibraryEntry newCard = new CardLibraryEntry
        {
            cardData = cardData,
            amount = 1
        };

        if (currentCardLibrary.cardLibraryList.Contains(newCard))
        {
            var target = currentCardLibrary.cardLibraryList.Find(x => x.cardData == cardData);
            target.amount++;
        }
        else
        {
            currentCardLibrary.cardLibraryList.Add(newCard);
        }
    }
}