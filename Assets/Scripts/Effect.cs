using System;
using System.Collections.Generic;

public interface IEffect
{
    EffecID EffecID { get; set; }
    int Value { get; set; }
    void DoAction(Character user, Character target);

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
public class Damage : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.Damage;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        target.MinusHp(Value);
    }
}
[Serializable]
public class RemoveArmor : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.RemoveArmor;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor - Value;
        target.SetArmor(tempvalue);
    }
}
[Serializable]
public class GainArmor : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.GainArmor;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor + Value;
        target.SetArmor(tempvalue);
    }
}
[Serializable]
public class RecoverHP : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.RecoverHP;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        user.AddHp(Value);
    }
}
[Serializable]
public class RecoverHPBuff
{
    List<IEffect> effects = new List<IEffect>();

    public void DoAction(Character user, Character target)
    {
        foreach (var item in effects)
        {
            item.DoAction(user,target);
        }
    }
}




[Serializable]
public class RecoverEP : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.RecoverHP;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        user.AddHp(Value);
    }
}
[Serializable]
public class AddHandCard : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.GetCard;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        user.AddCards(Value);
    }
}