using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<Card> deck { get; private set; }
    private Queue<Card> tempDeck=new Queue<Card>();
    public List<Card> handCards { get; private set; } = new List<Card>();
    public void SetDeck(List<Card> deck)
    {
        this.deck = deck;
    }

    public Card AddHandCard()
    {
        CheckCards();
        Card temp= tempDeck.Dequeue();
        handCards.Add(temp);
        return temp;
    }
    public void RemoveHandCard(Card card)
    {
        handCards.Remove(card);
    }
    private void CheckCards()
    {
        if (tempDeck.Count == 0)
        {
            foreach (var item in deck)
            {
                tempDeck.Enqueue(item);
            }
        }
    }
}
