using System;
using System.Collections.Generic;

public class Deck
{
    List<Card> originDeck;
    private Queue<Card> deckPool=new Queue<Card>();
    private Dictionary<Guid,Card> handCards = new Dictionary<Guid, Card>();
    public Dictionary<int, Guid> HandCards = new Dictionary<int, Guid>();
    public Deck(List<Card> deck)
    {
        originDeck = deck;
    }
    public Card AddHandCard(Guid uiGuid)
    {
        var card = GetNextCard();
        handCards.Add(uiGuid,card);
        return card;
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
