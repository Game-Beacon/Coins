using System;

public interface IEffect 
{
    public EffecID EffecID { get; set; }
    public int Value { get; set; }
    public abstract void DoAction(Character user, Character target);

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

public class Damage:IEffect
{
    public EffecID EffecID { get; set; } = EffecID.Damage;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        target.MinusHp(Value);
    }
}
public class RemoveArmor:IEffect
{
    public EffecID EffecID { get; set; } = EffecID.RemoveArmor;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor-Value;
        target.SetArmor(tempvalue);
    }
}
public class GainArmor:IEffect
{
    public EffecID EffecID { get; set; } = EffecID.GainArmor;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor+Value;
        target.SetArmor(tempvalue);
    }
}
public class RecoverHP : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.RecoverHP;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        target.AddHp(Value);
    }
}
public class RecoverEP : IEffect
{
    public EffecID EffecID { get; set; } = EffecID.RecoverHP;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        target.AddHp(Value);
    }
}
public class GetCard: IEffect
{
    public EffecID EffecID { get; set; } = EffecID.GetCard;
    public int Value { get; set; }

    public void DoAction(Character user, Character target)
    {
        BattleSystem.AddHandCards(user);
    }
}