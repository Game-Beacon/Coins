﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class CardFactory
{
    
    public static IEffect MakeEffect(EffecID effecID, int value)
    {
        IEffect effect=null;
        switch (effecID)
        {
            case EffecID.Damage:
                effect = new Damage(value);
                break;
            case EffecID.MagicDamage:
                effect = new Damage(value,true);
                break;
            case EffecID.GainArmor:
                effect = new GainArmor(value);
                break;
            case EffecID.RemoveArmor :
                effect = new RemoveArmor(value);
                break;
            case EffecID.RecoverHP:
                effect = new RecoverHP(value);
                break;
            case EffecID.AddCard:
                List<int> ids = new List<int>();
                for (int i = 0; i < value; i++)
                {
                    ids.Add(-1);
                }
                effect = new AddCard(ids);
                break;
            default:
                Tool.DeBugWarning($"{effecID} is not implemented");
                break;
        }
        return effect;
    }
}



[Serializable]
public class Card
{
    private int id;
    public Type type;
    public int cost;
    public IEffect cardAction;
    public Guid guid { get; private set; } = new Guid();
    public Card() { }
    public Card(string[] xmlInfo)
    {
        int index = 0;
        int.TryParse(xmlInfo[index++],out id);
        type = Tool.Parse<Type>(xmlInfo[index++]);
        int.TryParse(xmlInfo[index++], out cost);


        var effecID = Tool.Parse<EffecID>(xmlInfo[index++]);
        int.TryParse(xmlInfo[index++], out int value);


        cardAction=CardFactory.MakeEffect(effecID, value); ;
        IEffect tempEffect = cardAction;
        IEffect nextEffect ;
        while (index<xmlInfo.Length-1)
        {
            effecID = Tool.Parse<EffecID>(xmlInfo[index++]);
            int.TryParse(xmlInfo[index++],out value);
            if (effecID!= EffecID.None)
            {
                nextEffect = CardFactory.MakeEffect(effecID,value);
                tempEffect.SetNextAction(nextEffect);
                tempEffect = nextEffect;
            }
        }
    }
    public static Card TestCard(List<IEffect> buffs)
    {
        IEffect head = CardFactory.MakeEffect(EffecID.MagicDamage,5);
        IEffect nowEffect = head;
        foreach (var buff in buffs)
        {
            setAction(buff);
        }
        return new Card()
        {
            guid = Guid.NewGuid(),
            id = 1,
            type = Type.attack,
            cost = 5,
            cardAction = head
        };
        void setAction(IEffect effect)
        {
            nowEffect.SetNextAction(effect);
            nowEffect = effect;
        }
    }
    public string GetContent()
    {
        return cardAction.GetContent();
    }
    public void ReSetGuid()
    {
        guid = Guid.NewGuid();
    }
    public void DoAction(Character user, Character target)
    {
        cardAction.Cast(user,target);
    }

    public Card DeepClone()
    {
        Card card;
        using (Stream objectStream = new MemoryStream())
        {
            //序列化物件格式
            IFormatter formatter = new BinaryFormatter();
            //將自己所有資料序列化
            formatter.Serialize(objectStream, this);
            //複寫資料流位置，返回最前端
            objectStream.Seek(0, SeekOrigin.Begin);
            //再將objectStream反序列化回去 
            card= formatter.Deserialize(objectStream) as Card;
        }
        card.ReSetGuid();
        return card;

    }
}




