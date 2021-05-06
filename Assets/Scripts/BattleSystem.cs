using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleSystem 
{
    public static Character Player { get; private set; }
    public static Deck playerDeck;
    public static Character Enemy { get;  private set;}
    public static Deck enemyDeck;
    private static HandCardsUI handCardsUI;
    public static void Intialize()
    {
        Player= new fakecharactor();
        Enemy = new fakecharactor();
        List<Card> cards=CardSource.cards;
        playerDeck = new Deck();
        playerDeck.SetDeck(cards);
        enemyDeck = new Deck();
        enemyDeck.SetDeck(cards);
        enemyDeck.AddHandCard();
    }
    public static Dictionary<GameObject, Card> cards=new Dictionary<GameObject, Card>();
    public static void SETHandCardsUI(HandCardsUI cardsUI)
    {
        handCardsUI = cardsUI;
    }
    public static void ShowHandCards()
    {
        CardUI cardUI= handCardsUI.ReturnNouseCard();  
        Card card= playerDeck.AddHandCard();
        card.SetCardUI(cardUI);
        cards.Add(cardUI.gameObject,card);
    }

    public static void UseCard(GameObject gameObject)
    {
        Card card = cards[gameObject];
        cards.Remove(gameObject);
        PlayerDoCardEffect(card);
    }

    private static void PlayerDoCardEffect(Card card)
    {
        foreach (var item in card.effects)
        {
            
        }
    }
}
