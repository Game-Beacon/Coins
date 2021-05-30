using System;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    List<Card> originDeck;
    private Queue<Card> deckPool=new Queue<Card>();
    public Dictionary<Guid,Card> handCards = new Dictionary<Guid, Card>();
    public void SetOriginDeck(List<Card> deck)
    {
        originDeck = deck;
    }

    public Card AddHandCard(Guid uiGuid)
    {
        var nextCard = GetNextCard();
        handCards.Add(uiGuid,nextCard);
        return nextCard;
    }

    private Card GetNextCard()
    {
        if (deckPool.Count == 0)
        {
            ReSetDeckPool();
        }
        return deckPool.Dequeue();
    }

    private void ReSetDeckPool()
    {
        foreach (var item in originDeck)
        {
            deckPool.Enqueue(item);
        }
    }

    public void RemoveHandCard(Guid UiGuid)
    {
        handCards.Remove(UiGuid);
    }

    public Card GetCard(Guid uiID)
    {
        return handCards[uiID];
    }
}
