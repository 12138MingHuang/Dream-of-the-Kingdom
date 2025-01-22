using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardDeck : MonoBehaviour
{
    public Player player;
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    public Vector3 deckPosition;

    public List<CardDataSO> drawDeck = new List<CardDataSO>(); // 抽牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>(); // 弃牌堆
    private List<Card> handCardObjectList = new List<Card>(); // 当前手牌(每回合)

    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;
    
    private void Start()
    {
        // 测试用
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        drawDeck.Clear();

        foreach (CardLibraryEntry entry in cardManager.currentCardLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }
        
        ShuffleDeck();
    }

    public void NewTurnDrawCards()
    {
        DrawCard(4);
    }

    private void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            
            drawCountEvent?.RaiseEvent(drawDeck.Count, this);

            Card card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            card.transform.position = deckPosition;
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
            currentCard.UpdateCardState(); // 更新卡牌能量状态
            currentCard.isAnimating = true;
            // currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete = () => currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };

            // 设置卡牌顺序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();

        drawCountEvent?.RaiseEvent(drawDeck.Count, this);
        discardCountEvent?.RaiseEvent(discardDeck.Count, this);
        
        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = UnityEngine.Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 弃牌逻辑, 事件函数
    /// </summary>
    /// <param name="obj"> 弃牌对象 </param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        
        cardManager.DiscardCard(card.gameObject);
        
        discardCountEvent?.RaiseEvent(discardDeck.Count, this);
        
        SetCardLayout(0f);
    }

    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }
        
        handCardObjectList.Clear();
    }

    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
}
