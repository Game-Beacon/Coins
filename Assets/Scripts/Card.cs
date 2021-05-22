using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UniRx;
using UnityEditorInternal;
using UnityEngine.UI;

public static class TextHandler
{
    public static string GetEffectContent(IEffect effect)
    {
        string word = string.Empty;
        switch (effect.EffecID)
        {
            case EffecID.None:
                break;
            case EffecID.Damage:
                word = $"對敵人造成{effect.Value}點傷害";
                break;
            case EffecID.RemoveArmor:
                word = $"移除敵人{effect.Value}點護甲";
                break;
            case EffecID.GainArmor:
                word = $"獲得{effect.Value}點護甲";
                break;
            default:
                break;
        }
        return word;
    }
}

public class Card
{
    private int id;
    
    public Type type;
    public int cost;
    public List<IEffect> effects=new List<IEffect>();

    public Card(string[] xmlInfo)
    {
        int index = 0;
        int.TryParse(xmlInfo[index++],out id);
        type = Tool.Parse<Type>(xmlInfo[index++]);
        int.TryParse(xmlInfo[index++], out cost);
        while (index<xmlInfo.Length-1)
        {
            EffecID effecID = Tool.Parse<EffecID>(xmlInfo[index++]);
            int.TryParse(xmlInfo[index++],out int value);
            if (effecID!=EffecID.None)
            {
                IEffect tempEffect = MakeEffect(effecID,value);
                effects.Add(tempEffect);
            }
        }
    }

    private IEffect MakeEffect(EffecID effecID, int result)
    {
        IEffect effect=null;
        switch (effecID)
        {
            case EffecID.Damage:
                effect = new Damage{Value = result};
                break;
            case  EffecID.GainArmor:
                effect = new GainArmor{Value = result};
                break;
            case  EffecID.RemoveArmor :
                effect = new RemoveArmor{Value = result};
                break;
            default:
                Tool.DeBugWarning($"{effecID} is not implemented");
                break;
        }

        return effect;
    }

    public string GetContent()
    {
        string content=String.Empty;
        foreach (var effect in effects)
        {
            content += TextHandler.GetEffectContent(effect);
        }

        return content;
    }

    public void DoAction(Character target)
    {
        foreach (var effect in effects)
        {
            effect.DoAction(target);
        }
    }
}

public enum Type
{
    none,
    attack,
    order,
    talent,
}


