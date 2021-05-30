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
    public static bool yourTurn=false;
    private static CardsUI handCardsUI;

    public static void SethandCardsUI(CardsUI cardUI)
    {
        handCardsUI = cardUI;
    }
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
    public static void AddHandCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var guid = Guid.NewGuid();
            var card= playerDeck.AddHandCard(guid);
            int id= handCardsUI.SetCardAndReturnUniqueID(card,UISource.GetUIInfo(card.type));
            HandCards.Add(id,guid);
        }
    }

    public static bool PlayerUseCard(int uiID)
    {
        return TryUseCard(uiID,BattleSystem.Player,BattleSystem.Enemy);
    }
    private static bool TryUseCard(int uiID,Character user,Character target)
    {
        var guid= HandCards[uiID];
        Card card= playerDeck.GetCard(guid);
        if (card==null|| user.Ep<card.cost)
        {
            return false;
        }
        card.DoAction(target);
        HandCards.Remove(uiID);
        return true;
    }


    public static void GameStart()
    {
        Tool.DeBug("GameStart");
        AddHandCards(1);
        
    }


    public static void EndTurn()
    {
        yourTurn=!yourTurn ;
        if (yourTurn)
        {
            AddHandCards(2);
        }
    }

}
