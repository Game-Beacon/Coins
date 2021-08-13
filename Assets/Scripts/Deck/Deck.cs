using System;
using System.Collections.Generic;

public class Deck
{
    public Deck(List<Card> deck)
    {
        originDeck = deck;
    }
    public Func<Card> ChangeCard { private get; set; }
    private List<Card> originDeck;
    private Queue<Card> deckPool=new Queue<Card>();
    private Dictionary<Guid,Card> handCards = new Dictionary<Guid, Card>();
    public Dictionary<Guid, Card> HandCards => handCards;
    public Card AddHandCard(int id)
    {
        var card = GetCard(id);
        handCards.Add(card.guid, card);
        return card;
    }

    private Card GetCard(int id)
    {
        if (id==-1)
        {
            var card = GetNextCard();
            return card;
        }
        else
        {
            throw new Exception();
        }
    }
    bool test;
    private Card GetNextCard()
    {
        if (deckPool.Count == 0)
        {
            ReSetDeckPool();
        }

        List<IEffect> buffs = new List<IEffect>() {};
        if (!test)
        {
            BigMagicContainer buffControler = new BigMagicContainer(true,RoundPeriod.end,1);
            buffs.Add(buffControler);
            test = true;
        }
        //List<IEffect> buffs = new List<IEffect>() { new DamageOnDrag(true, RoundPeriod.end, 2),new Forzen(true,RoundPeriod.end, 2),new Bomb(true, RoundPeriod.end, 2) ,new RemoveBuff(false)};
        Card card = ChangeCard?.Invoke() ?? Card.TestCard(buffs);
        //Card card = ChangeCard?.Invoke() ?? deckPool.Dequeue();
        return card;
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



