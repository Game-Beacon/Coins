



using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattlePresenter: MonoBehaviour
{
    
    [SerializeField]private BattleStatus playerStatus;
    [SerializeField]private BattleStatus enemyStatus;
    [SerializeField] private HandCardsUI cardsUI;
    [SerializeField] private Button endButton;
    private void Awake()
    {
        BattleSystem.SethandCardsUI(cardsUI);
        BattleSystem.Player.SubscribeHP(playerStatus.SetHp);
        BattleSystem.Player.SubscribeEP(playerStatus.SetEneragy);
        BattleSystem.Enemy.SubscribeHP(enemyStatus.SetHp);
        BattleSystem.Enemy.SubscribeEP(enemyStatus.SetEneragy);
        endButton.onClick.AddListener(BattleSystem.EndTurn);
    }

}