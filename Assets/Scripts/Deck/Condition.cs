using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffecID
{
    None = 0,
    Damage,
    MagicDamage,
    RemoveArmor,
    GainArmor,
    RecoverHP,
    AddCard,
    RecoverEP,
    RemoveBuff,
    InvokeBigMagiic,
    Rage,
    AddMgicDamage,
    Frozen,
    AddDamage,
    AddDamageInOneGame,
    ChangeCard,
    Poison,
    DamageOnDrag,
    Bomb,
    CancelNextCard,
    RubNextCard,
    BigMagic
}

public enum RoundPeriod
{
    none,
    start,
    end,
    enemyStart,
    enemyEnd
}
public enum Type
{
    none,
    attack,
    order,
    talent,
}