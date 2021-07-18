using System;
using System.Collections.Generic;

public class Deck
{
    public Deck(List<Card> deck)
    {
        originDeck = deck;
    }

    private List<Card> originDeck;
    private Queue<Card> deckPool=new Queue<Card>();
    private Dictionary<Guid,Card> handCards = new Dictionary<Guid, Card>();

    public Card AddHandCard()
    {
        //var card = GetNextCard();
        var card = Card.TestCard();
        handCards.Add(card.guid,card);
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
            deckPool.Enqueue(item.DeepClone());
        }
    }

    public void RemoveHandCard(Guid UiGuid)
    {
        handCards.Remove(UiGuid);
    }

    public int GetCost(Guid guid)
    {
        if (handCards.ContainsKey(guid))
        {
            return handCards[guid].cost;
        }
        Tool.DeBugWarning("Card is not existed");
        return -1;
    }

    internal Card GetCard(Guid guid)
    {
        return handCards[guid];
    }
}



