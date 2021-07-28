



using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattlePresenter : MonoBehaviour
{

    [SerializeField] private BattleStatus playerStatus;
    [SerializeField] private BattleStatus enemyStatus;
    [SerializeField] private HandCardsUI playerCardsUI;
    [SerializeField] private HandCardsUI enemyCardsUI;
    [SerializeField] private Button endButton;
    IDisposable playerHp;
    IDisposable playerEp;
    IDisposable enemyHp;
    IDisposable enemyEp;
    public void Intialize()
    {
        BattleSystem.SethandCardsUI(playerCardsUI, enemyCardsUI);
        playerHp = BattleSystem.Player.SubscribeHP(playerStatus.SetHp);
        playerEp = BattleSystem.Player.SubscribeEP(playerStatus.SetEneragy);
        enemyHp = BattleSystem.Enemy.SubscribeHP(enemyStatus.SetHp);
        enemyEp = BattleSystem.Enemy.SubscribeEP(enemyStatus.SetEneragy);
        endButton.onClick.AddListener(BattleSystem.ChangeTurn);
    }
    public void GameEnd()
    {
        playerHp.Dispose();
        playerEp.Dispose();
        enemyHp.Dispose();
        enemyEp.Dispose();
        endButton.onClick.RemoveListener(BattleSystem.ChangeTurn);
    }



}