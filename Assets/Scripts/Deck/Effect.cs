using System;
using System.Collections.Generic;

public interface CardAction
{
    string GetContent();
    void Do(Character user, Character target);
    void SetNextAction(CardAction action);
}
[Serializable]
public abstract class Effect : CardAction
{
    protected CardAction nextAction;
    public int Value { get; set; }

    public void Do(Character user, Character target)
    {
        DoAction(user, target);
        nextAction?.Do(user, target);
    }
    protected abstract void DoAction(Character user, Character target);
    public string GetContent()
    {
        string value = GetEffectContent();
        string value2 = nextAction?.GetContent();
        return value2 == null ? value : value + value2;
    }
    protected abstract string GetEffectContent();

    public void SetNextAction(CardAction action)
    {
        nextAction = action;
    }
}

[Serializable]
public class BuffContainer : CardAction
{
    protected CardAction nextAction;
    protected Buff buff;

    public BuffContainer(Buff buff)
    {
        this.buff = buff;
    }
    public void Do(Character user, Character target)
    {
        buff?.Add(user, target);
        nextAction?.Do(user, target);
    }
    public string GetContent()
    {
        string value = Tool.AddWord( buff.GetContent(), nextAction?.GetContent());
        return value;
    }

    public void SetNextAction(CardAction action)
    {
        nextAction = action;
    }
}

[Serializable]
public abstract class Buff
{
    public Buff(bool setOnUser ,RoundPeriod roundPeriod)
    {
        SetOnUser = setOnUser;
        this.roundPeriod = roundPeriod;
    }
    public BuffID BuffID { get;protected set; }
    public RoundPeriod roundPeriod { get; private set; }
    public bool PositiveBuff { get; private set; }
    public bool Show { get; private set; }
    public int Value { get; internal set; }
    private bool SetOnUser { get; set; }
    public bool Remove { get; set; }
    public void Add(Character user, Character target)
    {
        (SetOnUser ? user : target).AddBuff(this);
    }
    public abstract void DoTimeCountAction(Character self,Character enemy);
    public string GetContent()
    {
        string value = Tool.AddWord(GetTitle(), GetBuffContent());
        return value;
    }
    public abstract string GetTitle();
    protected abstract string GetBuffContent();

    internal void SetValue(int value)
    {
        Value += value;
    }
}
public class Rage : Buff
{
    public Rage(bool setOnUser, RoundPeriod roundPeriod, int RageValue = 1) : base(setOnUser, roundPeriod) 
    {
        BuffID = BuffID.Rage;
        Value = RageValue;
    }
    public override void DoTimeCountAction(Character self, Character enemy)
    {
        self.MinusHp(Value);
        Remove = true;
    }

    public override string GetTitle()
    {
        return "Rage";
    }

    protected override string GetBuffContent()
    {
        return "";
    }
}

public class forzen : Buff
{
    public forzen(bool setOnUser, RoundPeriod roundPeriod,int value) : base(setOnUser, roundPeriod)
    {
        BuffID = BuffID.Forzen;
        Value = value;
    }

    public override void DoTimeCountAction(Character self, Character enemy)
    {
        Remove = true;
    }

    public override string GetTitle()
    {
        return "forzen";
    }

    protected override string GetBuffContent()
    {
        return "冰凍";
    }
}

public enum Action
{
    None = 0,
    Damage,
    RemoveArmor,
    GainArmor,
    RecoverHP,
    GetCard,
    RecoverEP
}
[Serializable]
public class Damage : Effect
{
    protected override void DoAction(Character user, Character target)
    {
        target.MinusHp(Value+user.AttackValue);
    }

    protected override string GetEffectContent()
    {
        return $"對敵人造成{Value}點傷害";
    }
}
[Serializable]
public class RemoveArmor : Effect
{
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

    protected override void DoAction(Character user, Character target)
    {
        user.AddCards(Value);
    }

    protected override string GetEffectContent()
    {
        return $"抽{Value}張牌";
    }
}