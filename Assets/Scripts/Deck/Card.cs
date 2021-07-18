using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UniRx;
using UnityEditorInternal;
using UnityEngine.UI;

public static class CardFactory
{
    
    public static CardAction MakeEffect(Action effecID, int result)
    {
        CardAction effect=null;
        switch (effecID)
        {
            case Action.Damage:
                effect = new Damage{Value = result};
                break;
            case  Action.GainArmor:
                effect = new GainArmor{Value = result};
                break;
            case  Action.RemoveArmor :
                effect = new RemoveArmor{Value = result};
                break;
            case Action.RecoverHP:
                effect = new RecoverHP { Value = result };
                break;
            case Action.GetCard:
                effect = new AddHandCard{ Value = result };
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
    public CardAction cardAction;
    //public List<CardAction> effects=new List<CardAction>();
    public Guid guid { get; private set; } = new Guid();
    public Card() { }
    public Card(string[] xmlInfo)
    {
        int index = 0;
        int.TryParse(xmlInfo[index++],out id);
        type = Tool.Parse<Type>(xmlInfo[index++]);
        int.TryParse(xmlInfo[index++], out cost);


        Action effecID = Tool.Parse<Action>(xmlInfo[index++]);
        int.TryParse(xmlInfo[index++], out int value);


        cardAction=CardFactory.MakeEffect(effecID, value); ;
        CardAction tempEffect = cardAction;
        CardAction nextEffect ;
        while (index<xmlInfo.Length-1)
        {
            effecID = Tool.Parse<Action>(xmlInfo[index++]);
            int.TryParse(xmlInfo[index++],out value);
            if (effecID!=Action.None)
            {
                nextEffect = CardFactory.MakeEffect(effecID,value);
                tempEffect.SetNextAction(nextEffect);
                tempEffect = nextEffect;
            }
        }
    }

    public static Card TestCard()
    {
        CardAction head = CardFactory.MakeEffect(Action.Damage,5);
        CardAction nowEffect = head;
        setAction(new BuffContainer(new forzen(false, RoundPeriod.end,3)));
        return new Card()
        {
            guid = Guid.NewGuid(),
            id = 1,
            type = Type.attack,
            cost = 1,
            cardAction = head
        };
        void setAction(CardAction effect)
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
        cardAction.Do(user,target);
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




