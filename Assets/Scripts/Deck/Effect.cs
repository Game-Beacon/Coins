using System;
using System.Collections.Generic;

public interface IEffect
{
    EffecID effecID { get; }
    string GetContent();
    void Cast(Character user, Character target);
    void SetNextAction(IEffect action);
}
[Serializable]
public abstract class Effect : IEffect
{
    protected IEffect nextAction;
    public int Value { get; set; }

    public abstract EffecID effecID { get; }

    public void Cast(Character user, Character target)
    {
        DoAction(user, target);
        nextAction?.Cast(user, target);
    }
    protected abstract void DoAction(Character user, Character target);
    public string GetContent()
    {
        string value = GetEffectContent();
        string value2 = nextAction?.GetContent();
        return value2 == null ? value : value + value2;
    }
    protected abstract string GetEffectContent();

    public void SetNextAction(IEffect action)
    {
        nextAction = action;
    }
}

[Serializable]
public abstract class Buff : IEffect
{
    protected IEffect nextAction;
    protected Character owener;
    protected Character enemy;

    public Buff(bool setOnUser, RoundPeriod roundPeriod)
    {
        SetOnUser = setOnUser;
        this.RoundPeriod = roundPeriod;
    }
    public RoundPeriod RoundPeriod { get; private set; }
    public abstract bool PositiveBuff { get; }
    private bool SetOnUser { get; set; }
    public bool Remove { get; set; }
    public abstract EffecID effecID { get; }
    public abstract bool IsOverlay { get; }
    public abstract int Value { get; }
    public void Overlay(int value)
    {
        OverlayAction(value);
    }
    protected abstract void OverlayAction(int value);
    public void Cast(Character user, Character enemy)
    {
        owener = SetOnUser ? user : enemy;
        this.enemy = SetOnUser ? enemy : user;
        owener.AddBuff(this);
        Cast();
        nextAction?.Cast(user, enemy);
    }
    public void DoTimeCountAction(RoundPeriod roundPeriod)
    {
        CountTime(roundPeriod);
        if (Remove)
        {
            Reverse();
        }
    }
    protected abstract void CountTime(RoundPeriod roundPeriod);
    
    protected virtual void Cast() { }
    protected virtual void Reverse() { }
    public string GetContent()
    {
        string value = Tool.AddWord(GetTitle(), GetBuffContent());
        value = Tool.AddWord(value, nextAction?.GetContent());
        return value;
    }
    public abstract string GetTitle();
    protected abstract string GetBuffContent();
    public void SetNextAction(IEffect action)
    {
        nextAction = action;
    }

    public virtual int Invoke(int value, bool use)
    {
        return value;
    }
}
public class AddDamage : Buff
{
    public AddDamage(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        this.value = value;
    }

    public override bool PositiveBuff => true;

    public override EffecID effecID => EffecID.AddDamage;

    public override bool IsOverlay => true;

    public override int Value => value;
    private int value;


    List<int> overlayValue = new List<int>();
    public override string GetTitle()
    {
        return "AddDamage";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        Remove = RoundPeriod == roundPeriod;
    }

    protected override string GetBuffContent()
    {
        return "";
    }
    public override int Invoke(int attackValue, bool use)
    {
        int tempvalue = attackValue + value;
        foreach (var value in overlayValue)
        {
            if (value == -1)
            {
                tempvalue *= 2;
            }
            else
            {
                tempvalue += value;
            }
        }
        if (use)
        {
            Remove = true;
        }
        return tempvalue;
    }

    protected override void OverlayAction(int value)
    {
        overlayValue.Add(value);
    }
}

public class AddDamageInOneGame : Buff
{
    public AddDamageInOneGame(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        this.value = value;
    }

    public override bool PositiveBuff => true;

    public override EffecID effecID => EffecID.AddDamage;

    public override bool IsOverlay => true;

    public override int Value => value;
    private int value;


    
    public override string GetTitle()
    {
        return "AddDamageInOneGame";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        
    }

    protected override string GetBuffContent()
    {
        return "";
    }
    public override int Invoke(int attackValue, bool use)
    {
        int tempvalue = attackValue + value;
        return tempvalue;
    }

    protected override void OverlayAction(int value)
    {
        this.value+=value;
    }
}
public class Rage : Buff
{
    public override EffecID effecID => EffecID.Rage;

    public override bool IsOverlay => true;

    public override int Value => value;

    public override bool PositiveBuff => true;

    private int value;
    public Rage(bool setOnUser, RoundPeriod roundPeriod, int RageValue = 1) : base(setOnUser, roundPeriod)
    {
        value = RageValue;
    }
    public override int Invoke(int value, bool use)
    {
        return value + this.value;
    }
    public override string GetTitle()
    {
        return "Rage";
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        owener.MinusHp(Value);
        Remove = true;
    }
    protected override string GetBuffContent()
    {
        return "";
    }

    protected override void OverlayAction(int value)
    {
        this.value += value;
    }
}
public class AddMgicDamage : Buff
{
    public override EffecID effecID => EffecID.AddMgicDamage;

    public override bool IsOverlay => true;

    public override int Value => value;

    public override bool PositiveBuff => true;

    private int value;
    public AddMgicDamage(bool setOnUser, RoundPeriod roundPeriod, int addMgicDamageValue = 1) : base(setOnUser, roundPeriod)
    {
        value = addMgicDamageValue;
    }
    public override int Invoke(int value, bool use)
    {
        return value + this.value;
    }
    public override string GetTitle()
    {
        return "AddMgicDamage";
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        Remove = true;
    }
    protected override string GetBuffContent()
    {
        return "";
    }

    protected override void OverlayAction(int value)
    {
        this.value += value;
    }
}
public class Forzen : Buff
{
    private int value { get; set; }

    public override EffecID effecID => EffecID.Frozen;

    public override bool IsOverlay => true;

    public override int Value => value;

    public override bool PositiveBuff => false;

    public Forzen(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        this.value = value;
    }

    public override string GetTitle()
    {
        return "forzen";
    }

    protected override string GetBuffContent()
    {
        return $"冰凍效果{value}";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod == RoundPeriod)
        {
            Remove = true;
        }
    }
    public override int Invoke(int attackValue, bool use)
    {
        if (attackValue >= value)
        {
            attackValue -= value;
            if (use) Remove = true;
        }
        else
        {
            if (use)
            {
                value -= attackValue;
            }
            attackValue = 0;
        }
        return attackValue;
    }
    protected override void OverlayAction(int value)
    {
        this.value += Value;
    }
}
public class DamageOnDrag : Buff
{
    private int damage { get; set; }

    public override EffecID effecID => EffecID.DamageOnDrag;

    public override bool IsOverlay => true;

    public override int Value => damage;

    public override bool PositiveBuff => false;

    public DamageOnDrag(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        this.damage = value;
    }

    public override string GetTitle()
    {
        return "DamageOnDrag";
    }

    protected override string GetBuffContent()
    {
        return $"抽牌受傷{damage}";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
    }
    public override int Invoke(int notUse, bool use)
    {
        int value = damage;

        if (use)
        {
            damage--;
            Remove = damage == 0;
        }
        return value;
    }
    protected override void OverlayAction(int value)
    {
        this.damage += Value;
    }
}
public class Bomb : Buff
{
    public Bomb(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        turns = value;
    }
    public override bool PositiveBuff => false;

    public override EffecID effecID => EffecID.Bomb;

    public override bool IsOverlay => false;

    public override int Value => turns;
    int turns;
    int damgaeHaveToDo=5;
    public override string GetTitle()
    {
        return "Bomb";
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod==RoundPeriod)
        {
            turns--;
        }
        if (turns<=0)
        {
            owener.MinusHp(damgaeHaveToDo);
            Remove = true;
        }
    }

    protected override string GetBuffContent()
    {
        return $"炸彈傷害{damgaeHaveToDo}";
    }
    public override int Invoke(int value, bool use)
    {
        if (use)
        {
            damgaeHaveToDo -= value;
            Remove = damgaeHaveToDo <= 0;
        }
        return value;
    }
    protected override void OverlayAction(int value)
    {
    }
}
public class ChangeCard : Buff
{
    public ChangeCard(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        cardID = value;
    }
    public override bool PositiveBuff => true;

    public override EffecID effecID => EffecID.ChangeCard;

    public override bool IsOverlay => true;

    public override int Value => cardID;

    int cardID;
    public override string GetTitle()
    {
        return "ChangeCard";
    }
    protected override void Cast()
    {
        owener.SetChangCard(ChangFunction);
    }
    private Card ChangFunction()
    {
        List<IEffect> list = new List<IEffect>() { new Forzen(false, RoundPeriod.end, 3) };
        return Card.TestCard(list);
    }
    protected override void Reverse()
    {
        owener.RemoveChangCard();
        Remove = true;
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod==RoundPeriod)
        {
            owener.RemoveChangCard();
            Remove = true;
        }
    }
    protected override string GetBuffContent()
    {
        return "";
    }

    public override int Invoke(int value, bool use)
    {
        return cardID;
    }
    protected override void OverlayAction(int cardID)
    {
        this.cardID = cardID;
    }
}

public class Poison : Buff
{
    public Poison(bool setOnUser, RoundPeriod roundPeriod, int damagePercent,int turnNumber) : base(setOnUser, roundPeriod)
    {
        this.damagePercent = damagePercent;
        this.turnNumber = turnNumber;
    }
    public override bool PositiveBuff => false;

    public override EffecID effecID => EffecID.Poison;

    public override bool IsOverlay => true;

    public override int Value => turnNumber;
    private int turnNumber;
    private int damagePercent;

    public override string GetTitle()
    {
        return "Poison";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod==RoundPeriod)
        {
            int damage = owener.Hp * 5 / 100;
            owener.MinusHp(damage);
            turnNumber--;
        }
        if (turnNumber<=0)
        {
            Remove = true;
        }
    }

    protected override string GetBuffContent()
    {
        return $"造成{damagePercent}%傷害";
    }

    protected override void OverlayAction(int value)
    {
        turnNumber = value;
    }
}
[Serializable]
public class Damage : Effect
{
    public override EffecID effecID => EffecID.Damage;

    protected override void DoAction(Character user, Character target)
    {
        int value = user.GetAttackValue(Value) + target.GetDefendValue();
        BattleSystem.CountDamage(user,value);
        target.MinusHp(value);
    }

    protected override string GetEffectContent()
    {
        return $"造成{Value}點傷害";
    }
}

[Serializable]
public class MagicDamage : Effect
{
    public override EffecID effecID => EffecID.MagicDamage;

    protected override void DoAction(Character user, Character target)
    {
        int value = user.GetMagicAttackValue(Value) + target.GetDefendValue();
        BattleSystem.CountDamage(user, value);
        target.MinusHp(value);
    }

    protected override string GetEffectContent()
    {
        return $"造成{Value}點魔法傷害";
    }
}
[Serializable]
public class RemoveArmor : Effect
{
    public override EffecID effecID => EffecID.RemoveArmor;

    protected override void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor - Value;
        target.SetArmor(tempvalue);
    }

    protected override string GetEffectContent()
    {
        return $"移除敵人{Value}點護甲";
    }
}
[Serializable]
public class GainArmor : Effect
{
    public override EffecID effecID => EffecID.GainArmor;

    protected override void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor + Value;
        target.SetArmor(tempvalue);
    }

    protected override string GetEffectContent()
    {
        return $"獲得{Value}點護甲";
    }
}
[Serializable]
public class RecoverHP : Effect
{
    public override EffecID effecID => EffecID.RecoverHP;

    protected override void DoAction(Character user, Character target)
    {
        user.AddHp(Value);
    }

    protected override string GetEffectContent()
    {
        return $"回復{Value}點血量";
    }
}
[Serializable]
public class RecoverEP : Effect
{
    public override EffecID effecID => EffecID.RecoverEP;

    protected override void DoAction(Character user, Character target)
    {
        user.AddHp(Value);
    }

    protected override string GetEffectContent()
    {
        return $"回復{Value}點魔力";
    }
}
[Serializable]
public class AddHandCard : Effect
{
    public override EffecID effecID => EffecID.AddCard;

    protected override void DoAction(Character user, Character target)
    {
        user.AddCards(Value);
    }

    protected override string GetEffectContent()
    {
        return $"抽{Value}張牌";
    }
}
public class AddAssignCard : Effect
{
    public AddAssignCard(List<int> ids)
    {
        this.ids = ids;
        Value = ids.Count;
    }
    public override EffecID effecID => EffecID.AddAssignCard;
    List<int> ids;
    protected override void DoAction(Character user, Character target)
    {
        foreach (var id in ids)
        {
            user.AddAssignCards(id);
        }
    }

    protected override string GetEffectContent()
    {
        return $"抽{Value}張牌";
    }
}

public class RemoveBuff : Effect
{
    public RemoveBuff(bool removeGoodBuff)
    {
        this.removeGoodBuff = removeGoodBuff;
    }
    public override EffecID effecID => EffecID.RemoveBuff;

    bool removeGoodBuff;
    protected override void DoAction(Character user, Character target)
    {
        user.RemoveBuff(removeGoodBuff);
    }

    protected override string GetEffectContent()
    {
        return "移除buff";
    }
}

public class RemoveRage : Effect
{
    public override EffecID effecID => EffecID.RemoveRage;

    bool removeGoodBuff;
    protected override void DoAction(Character user, Character target)
    {
        user.RemoveBuff(EffecID.Rage);
    }

    protected override string GetEffectContent()
    {
        return "移除Ragebuff";
    }
}