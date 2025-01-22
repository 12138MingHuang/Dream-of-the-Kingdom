using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    public Vector3 deckPosition;

    public List<CardDataSO> drawDeck = new List<CardDataSO>(); // 抽牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>(); // 弃牌堆
    private List<Card> handCardObjectList = new List<Card>(); // 当前手牌(每回合)

    private void Start()
    {
        // 测试用
        InitializeDeck();
        DrawCard(3);
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
            currentCard.isAnimating = true;
            CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
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
        
        SetCardLayout(0f);
    }

    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
}
