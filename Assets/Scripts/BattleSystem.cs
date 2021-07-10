using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public static class BattleSystem 
{
    public static bool playerTurn;

    private static Dictionary<int, Guid> playerHandCards = new Dictionary<int, Guid>();
    private static Dictionary<int, Guid> enemyHandCards = new Dictionary<int, Guid>();
    private static CardsUI handCardsUI;

    public static Character Player { get; private set; }
    public static Character Enemy { get; private set; }

    public static void Intialize()
    {
        List<Card> deck=CardInfoSource.cards;
        Player= new Fakecharactor("player");
        Player.SubscribeHP(CheckPlayerHP);
        Player.AddDeck(new Deck(deck));
        Enemy = new Fakecharactor("enemy");
        Enemy.SubscribeHP(CheckEnemyHP);
        Enemy.AddDeck(new Deck(deck));
    }
    private static void CheckPlayerHP(int value)
    {
        if (value <= 0)
        {
            Tool.DeBug("GamerOver");
            TestSystems.Instance.EndGame();
        }
    }
    private static void CheckEnemyHP(int value)
    {
        if (value <= 0)
        {
            Tool.DeBug("YouWin");
            TestSystems.Instance.EndGame();
        }
    }
    public static void GameStart(bool yourTurn=true)
    {
        Tool.DeBug("GameStart");
        playerTurn = yourTurn;
        StartTurn();
    }

    private static void DoActionStartBuff(Character character)
    {
    }
    private static void DoActionEndBuff(Character character)
    {
    }





    public static void AddHandCards(Character character)
    {
        for (int i = 0; i < character.DrawCountWhenYourTurn; i++)
        {
            var cardID = Guid.NewGuid();
            var card= character.AddHandCard(cardID);
            int uiID= handCardsUI.SetCardAndReturnUniqueID(card,UISource.GetUIInfo(card.type));
            playerHandCards.Add(uiID,cardID);
        }
    }
    public static void SethandCardsUI(CardsUI cardUI)
    {
        handCardsUI = cardUI;
    }
    public static bool PlayerUseCard(int uiID)
    {
        return TryUseCard(uiID,Player,Enemy);
    }
    private static bool TryUseCard(int uiID,Character user,Character target)
    {
        var guid= playerHandCards[uiID];
        Card card= Player.GetCard(guid);
        if (card==null|| user.Ep<card.cost)
        {
            return false;
        }
        user.SetEP(user.Ep - card.cost);
        card.DoAction(user,target);
        playerHandCards.Remove(uiID);
        return true;
    }
    public static void StartTurn()
    {
        var character = playerTurn ? Player : Enemy;
        character.StartTurn();
        AddHandCards(character);
    }
    public static void ChangeTurn()
    {
        EndTurn();
        playerTurn = !playerTurn ;
        StartTurn();
    }
    private static void EndTurn()
    {
        var character = playerTurn ? Player : Enemy;
        character.EndTurn();
    }
}
