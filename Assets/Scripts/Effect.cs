using System;
using System.Collections.Generic;

public interface IEffect
{
    string GetContent();
    void DoAction(Character user, Character target);
}
[Serializable]
public abstract class Effect : IEffect
{
    protected IEffect baseEffect;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        Action(user,target);
        baseEffect?.DoAction(user, target);
    }
    protected abstract void Action(Character user, Character target);
    public string GetContent()
    {
        string value= GetEffectContent();
        string value2= baseEffect?.GetContent();
        return value2==null? value:value + value2;
    }
    protected abstract string GetEffectContent();
}
public enum EffecID
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
    protected override void Action(Character user, Character target)
    {
        target.MinusHp(Value);
    }

    protected override string GetEffectContent()
    {
        return$"對敵人造成{Value}點傷害";
    }
}
[Serializable]
public class RemoveArmor : Effect
{
    protected override void Action(Character user, Character target)
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
    protected override void Action(Character user, Character target)
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

    protected override void Action(Character user, Character target)
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
    protected override void Action(Character user, Character target)
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

    protected override void Action(Character user, Character target)
    {
        user.AddCards(Value);
    }

    protected override string GetEffectContent()
    {
        return $"抽{Value}張牌";
    }
}