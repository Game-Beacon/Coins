using System;
using UniRx;
public class Card
{
    public int ID;
    public Type type;
    public int Cost;
    public Effect Effect1;
    public Effect Effect2;

    public Card(string[] xmlInfo)
    {
        int.TryParse(xmlInfo[0],out ID);
        if (!Enum.TryParse<Type>(xmlInfo[1], out type))
        {
            Tool.DeBugWarning($"the cardType{xmlInfo[1]} is wrong");
        }
        int.TryParse(xmlInfo[2], out Cost);
        Effect1.action = Tool.Parse<Action>(xmlInfo[3]);
        int.TryParse(xmlInfo[4],out int value);
        Effect1.value = value;
        Effect2.action = Tool.Parse<Action>(xmlInfo[5]);
        int.TryParse(xmlInfo[4],out value);
        Effect2.value = value;
    }
    public void SetCardUI(CardUI cardUI)
    {
        CardUIInfo info= UISource.GetBG(type);
        cardUI.SetCard(this, info);
    }
    public string GetContent()
    {
        string content= GetEffectContent(Effect1) + GetEffectContent(Effect2);
        return content;
    }

    private string GetEffectContent(Effect effect)
    {
        string word = string.Empty;
        switch (effect.action)
        {
            case Action.none:
                break;
            case Action.Damage:
                word = $"對敵人造成{effect.value}點傷害";
                break;
            case Action.RemoveArmor:
                word = $"移除敵人{effect.value}點護甲";
                break;
            case Action.GainArmor:
                word = $"獲得{effect.value}點護甲";
                break;
            default:
                break;
        }
        return word;
    }
}
public struct Effect
{
    public Action action { get; set; }
    public int value { get; set; }
}

public enum Type
{
    none,
    attack,
    order,
    talent,
}
public enum Action
{
    none = 0,
    Damage,
    RemoveArmor,
    GainArmor
}

