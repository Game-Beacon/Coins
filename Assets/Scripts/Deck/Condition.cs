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
    GetCard,
    RecoverEP,
    AddCard,
    AddAssignCard,
    RemoveBuff,
    RemoveRage,
    Rage,
    AddMgicDamage,
    Frozen,
    AddDamage,
    ChangeCard,
    Poison,
    DamageOnDrag,
    Bomb
}
public enum BuffID
{
    none,
    Rage,
    Forzen
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