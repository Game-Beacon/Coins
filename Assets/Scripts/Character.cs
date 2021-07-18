using System;
using System.Collections.Generic;
using UniRx;
public abstract class Character
{
    protected Character(Value hp, Value ep, int drawCountWhenYourTurn)
    {
        maxHP.Value = hp.MaxValue;
        this.hp.Value = hp.NowValue;
        maxEP.Value = ep.NowValue;
        this.ep.Value = ep.NowValue;
        drawCount.Value = drawCountWhenYourTurn;
    }
    public string name;

    protected ReactiveProperty<int> maxHP { get; private set; } = new ReactiveProperty<int>();
    protected ReactiveProperty<int> hp { get; private set; } = new ReactiveProperty<int>();
    protected ReactiveProperty<int> maxEP { get; private set; } = new ReactiveProperty<int>();
    protected ReactiveProperty<int> ep { get; private set; } = new ReactiveProperty<int>();
    protected ReactiveProperty<int> drawCount { get; private set; } = new ReactiveProperty<int>(1);
    protected ReactiveProperty<int> armor { get; private set; } = new ReactiveProperty<int>();

    BuffControler buffControler;

    private CardsUI cardsUI;
    private Deck deck;
    private Character enemy;
    public int Hp => hp.Value;
    public int Ep => ep.Value;
    public int DrawCountWhenYourTurn => drawCount.Value;
    public int Armor => armor.Value;
    public bool IsDead { get; private set; } = false;
    public int AttackValue
    {
        get
        {
            return buffControler.GetAttackValue();
        }
    }
    public IDisposable SubscribeHP(Action<int> fuc)
    {
        return hp.Subscribe(fuc);
    }
    public IDisposable SubscribeEP(Action<int> fuc)
    {
        return ep.Subscribe(fuc);
    }

    public void SetBuffController(Character enemy)
    {
        buffControler = new BuffControler(this, enemy);
    }

    internal void AddBuff(Buff buff)
    {
        buffControler.Add(buff);
    }

    public IDisposable SubscribeArmor(Action<int> fuc)
    {
        return armor.Subscribe(fuc);
    }


    public void SetUI(CardsUI ui)
    {
        cardsUI = ui;
    }

    public void AddOneTurnHandCards()
    {
        AddCards(DrawCountWhenYourTurn);
    }
    public void AddCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var card = deck.AddHandCard();
            cardsUI.SetCardAndReturnUniqueID(card, UISource.GetUIInfo(card.type));
        }
    }

    public void TryUseCard(Guid guid, Character target)
    {
        var isUseAble = CheckUseAble(guid);
        if (isUseAble)
        {
            UseCard(guid, target);
        }
    }

    private void UseCard(Guid guid, Character target)
    {
        Card card = GetCard(guid);
        SetEP(Ep - card.cost);
        card.DoAction(this, target);
        deck.RemoveHandCard(guid);
        cardsUI.RecycleCard(guid);
    }

    private Card GetCard(Guid guid)
    {
        return deck.GetCard(guid);
    }

    public void SetEP(int value)
    {
        ep.Value = value;
    }
    public void MinusHp(int value)
    {
        if (value<=0)
        {
            return;
        }

        int tempHp = hp.Value-value;
        if (tempHp<=0)
        {
            IsDead = true;
        }
        hp.Value = tempHp < 0 ? 0 : tempHp;
    }
    public void AddHp(int value)
    {
        if (value <= 0)
        {
            return;
        }
        int tempHp = hp.Value + value;
        hp.Value = tempHp > maxHP.Value ? maxHP.Value : tempHp;
    }

    public void SetArmor(int value)
    {
        armor.Value = value;
    }

    internal void AddDeck(Deck deck)
    {
        this.deck = deck;
    }
    private  bool CheckUseAble(Guid guid)
    {
        var cost=deck.GetCost(guid);
        if (cost<0)
        {
            return false;
        }
        return Ep >= cost;
    }


    internal void EndTurn()
    {
        buffControler.DoBuffTimeCount(RoundPeriod.end);
        buffControler.DoBuffTimeCount(RoundPeriod.enemyStart);
    }

    

    internal void StartTurn()
    {
        buffControler.DoBuffTimeCount(RoundPeriod.enemyEnd);
        buffControler.DoBuffTimeCount(RoundPeriod.start);
    }
}



class BuffControler
{
    private Character user;
    private Character enemy;
    private List<Buff> buffs = new List<Buff>();

    public int GetAttackValue()
    {
        int value = 0;
        if (buffs.Exists(buff => buff.BuffID == BuffID.Rage))
        {
            value+= buffs.Find(buff => buff.BuffID == BuffID.Rage).Value;
        }
        if (buffs.Exists(buff => buff.BuffID == BuffID.Forzen))
        {
            value -= buffs.Find(buff => buff.BuffID == BuffID.Forzen).Value;
        }
        return value;
    }


    public BuffControler(Character user,Character enemy)
    {
        this.user = user;
        this.enemy = enemy;
    }
    public void DoBuffTimeCount(RoundPeriod roundPeriod)
    {
        buffs.ForEach((buff) =>
        {
            if (buff.roundPeriod == roundPeriod)
            {
                buff.DoTimeCountAction(user, enemy);
            }
        });
        buffs = buffs.FindAll(buff=>buff.Remove==false);
    }


    internal void Add(Buff buff)
    {
        switch (buff.BuffID)
        {

            case BuffID.Rage:
                if (buffs.Exists(buff=>buff.BuffID==BuffID.Rage))
                {
                    buffs.Find(buff => buff.BuffID == BuffID.Rage).SetValue(buff.Value);
                }
                else
                {
                    buffs.Add(buff);
                    ShowBuff(buff);
                }
                break;
            case BuffID.Forzen:
                if (buffs.Exists(buff => buff.BuffID == BuffID.Forzen))
                {
                    buffs.Find(buff => buff.BuffID == BuffID.Forzen).SetValue(buff.Value);
                }
                else
                {
                    buffs.Add(buff);
                    ShowBuff(buff);
                }
                break;
            default:
                Tool.DeBug("wrong");
                break;
        }
    }
    private void ShowBuff(Buff buff)
    {
        string value = buff.PositiveBuff ? "GoodBuff" : "BadBuff";
        Tool.DeBug($"{value}:{buff.GetTitle()}");
    }

}

public struct Value
{
    public Value(int max,int now)
    {
        MaxValue = max;
        NowValue = now;
    }
    public Value(int max)
    {
        MaxValue = max;
        NowValue = max;
    }
    public int MaxValue { get; }
    public int NowValue { get;}
}

public class Fakecharactor : Character
{
    public Fakecharactor():base(new Value(100), new Value(100),1) { }
    public Fakecharactor(string name):base(new Value(100,100), new Value(100, 100),3) 
    {
        this.name = name;
    }
}

