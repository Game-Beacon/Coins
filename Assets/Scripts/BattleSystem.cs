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
        Player.SubscribeHP((value) => 
        {
            if (value<=0)
            {
                Tool.DeBug("GamerOver");
                TestSystems.Instance.EndGame();
            }
        });
        playerDeck = new Deck(deck);
        Enemy = new Fakecharactor("enemy");
        Enemy.SubscribeHP((value) =>
        {
            if (value <= 0)
            {
                Tool.DeBug("YouAreWin");
                TestSystems.Instance.EndGame();
            }
        });
        enemyDeck = new Deck(deck);
    }

    public static void PlayerAddHandCards(int count)
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
        return TryUseCard(uiID,Player,Enemy);
    }
    private static bool TryUseCard(int uiID,Character user,Character target)
    {
        var guid= HandCards[uiID];
        Card card= playerDeck.GetCard(guid);
        if (card==null|| user.Ep<card.cost)
        {
            return false;
        }
        user.SetEP(user.Ep - card.cost);
        card.DoAction(target);
        HandCards.Remove(uiID);
        return true;
    }


    public static void GameStart()
    {
        Tool.DeBug("GameStart");
        PlayerAddHandCards(1);
    }


    public static void EndTurn()
    {
        yourTurn=!yourTurn ;
        if (yourTurn)
        {
            PlayerAddHandCards(2);
        }
    }

}
