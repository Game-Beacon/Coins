using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public static class BattleSystem 
{
    public static bool playerTurn;

    private static Dictionary<int, Guid> playerHandCards = new Dictionary<int, Guid>();

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
    public static void SethandCardsUI(CardsUI playerUI, CardsUI enemyUI)
    {
        Player.SetUI(playerUI);
        Enemy.SetUI(enemyUI);
    }
    public static void PlayerUseCard(int uiID)
    {
        Player.TryUseCard(uiID, Enemy);
    }
    public static void EnemyUseCard(int uiID)
    {
        Enemy.TryUseCard(uiID, Player);
    }
    public static void StartTurn()
    {
        var character = playerTurn ? Player : Enemy;
        character.StartTurn();
        character.AddOneTurnHandCards();
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
