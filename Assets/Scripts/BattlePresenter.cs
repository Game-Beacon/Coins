



using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class BattlePresenter: MonoBehaviour
{
    
    [SerializeField]private BattleStatus playerStatus;
    [SerializeField]private BattleStatus enemyStatus;
    [SerializeField] private HandCardsUI cardsUI;

    private void Awake()
    {
        BattleSystem.Player.SubscribeHP(playerStatus.SetHp);
        BattleSystem.Player.SubscribeEP(playerStatus.SetEneragy);
        BattleSystem.Enemy.SubscribeHP(enemyStatus.SetHp);
        BattleSystem.Enemy.SubscribeEP(enemyStatus.SetEneragy);
    }

}