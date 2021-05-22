using System;

public interface IEffect 
{
    public EffecID EffecID { get; set; }
    public int Value { get; set; }
    public abstract void DoAction(Character target);

}
public enum EffecID
{
    None = 0,
    Damage,
    RemoveArmor,
    GainArmor
}

public class Damage:IEffect
{
    public EffecID EffecID { get; set; } = EffecID.Damage;
    public int Value { get; set; }

    public void DoAction(Character target)
    {
        int tempvalue = target.Hp-Value;
        target.SetHP(tempvalue);
    }
}
public class RemoveArmor:IEffect
{
    public EffecID EffecID { get; set; } = EffecID.RemoveArmor;
    public int Value { get; set; }

    public void DoAction(Character target)
    {
        int tempvalue = target.Armor-Value;
        target.SetArmor(tempvalue);
    }
}public class GainArmor:IEffect
{
    public EffecID EffecID { get; set; } = EffecID.GainArmor;
    public int Value { get; set; }

    public void DoAction(Character target)
    {
        int tempvalue = target.Armor+Value;
        target.SetArmor(tempvalue);
    }
}