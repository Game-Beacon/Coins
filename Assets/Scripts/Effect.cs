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
        return$"��ĤH�y��{Value}�I�ˮ`";
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
        return $"�����ĤH{Value}�I�@��";
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
        return $"��o{Value}�I�@��";
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
        return $"�^�_{Value}�I��q";
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
        return $"�^�_{Value}�I�]�O";
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
        return $"��{Value}�i�P";
    }
}