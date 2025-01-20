﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;

    private List<CardDataSO> drawDeck = new List<CardDataSO>(); // 抽牌堆
    private List<CardDataSO> discardDeck = new List<CardDataSO>(); // 弃牌堆
    private List<Card> handCardObjectList = new List<Card>(); // 当前手牌(每回合)

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
    }

    private void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                // TODO: 弃牌堆为空时，重新洗牌
            }

            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            Card card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            
            handCardObjectList.Add(card);
        }
    }
    
    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
}