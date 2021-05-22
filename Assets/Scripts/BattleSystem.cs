using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public static class BattleSystem 
{
    public static Character Player { get; private set; }
    public static Deck playerDeck;
    public static Character Enemy { get;  private set;}
    public static Deck enemyDeck;
    public static Dictionary<int, Guid> HandCards = new Dictionary<int, Guid>();
    private static HandCardsUI handCardsUI;
    public static bool yourTurn=false;
    private static Action TurnAction=()=>{};
    public static void Intialize()
    {
        List<Card> deck=CardInfoSource.cards;
        Player= new Fakecharactor("player");
        playerDeck = new Deck();
        playerDeck.SetOriginDeck(deck);
        Enemy = new Fakecharactor("enemy");
        enemyDeck = new Deck();
        enemyDeck.SetOriginDeck(deck);
    }
    public static void SETHandCardsUI(HandCardsUI cardsUI)
    {
        handCardsUI = cardsUI;
    }

    public static void AddHandCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerDeck.AddHandCard(Guid.NewGuid());
        }
    }
    public static void ShowHandCards()
    {
        Tool.DeBug("Here is your HandCard");
        HandCards.Clear();
        var test = playerDeck.handCards.GetEnumerator();
        int i = 0;
        while (test.MoveNext())
        {
            var value = test.Current;
            HandCards.Add(i,value.Key);
            Tool.DeBug($"第{value.Key}張牌 :{value.Value.GetContent()}");
            i++;
        }
    }

    public static void TryUseCard(Guid uiID,Character user,Character target)
    {
        Card card= playerDeck.GetCard(uiID);
        if (user.Ep<card.cost)
        {
            Tool.DeBug($"Your Energy is not enough to use Card{card.GetContent()}");
            return;
        }
        Tool.DeBug(card.GetContent());
        card.DoAction(target);
    }


    public static void GameStart()
    {
        
        Tool.DeBug("GameStart");
        AddHandCards(1);
        ShowHandCards();

        
        Observable.EveryUpdate()
            .Subscribe(DetectInput);
    }

    static void DetectInput(long i)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndTurn();
            return;
        }
        TurnAction();
    }

    static void UseCard()
    {
        
        int card = -1;
        string s = Input.inputString;
        if (int.TryParse(s,out int num))
        {
            card = num;
        }

        if (card>=0)
        {
            if (BattleSystem.HandCards.ContainsKey(card))
            {
                Tool.DeBug("UseCard");
                BattleSystem.TryUseCard(BattleSystem.HandCards[card],BattleSystem.Player,BattleSystem.Enemy);
            }
        }
    }

    static void EndTurn()
    {
        BattleSystem.yourTurn=!BattleSystem.yourTurn ;
        if (BattleSystem.yourTurn)
        {
            BattleSystem.ShowHandCards();
            TurnAction = UseCard;
        }
        else
        {
            TurnAction = () => { };
        }
    }

}
