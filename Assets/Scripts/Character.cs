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
    public void SetBuffUI(BuffUIList  buffUIList)
    {
        buffControler.SetBuffUI(buffUIList);
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
        for (int i = 0; i < DrawCountWhenYourTurn; i++)
        {
            AddCards(-1);
        }
    }
    public void AddRandomCards(int value)
    {

    }

    public void AddCards(params int[] ids)
    {
        foreach (var id in ids)
        {
            var card = deck.AddHandCard(id);
            int value = buffControler.GetDragCardDamage(true);
            MinusHp(value);
            cardsUI.SetCardAndReturnUniqueID(card, UISource.GetUIInfo(card.type));
        }
    }
    public void AddAssignCards(int id)
    {
        var card = deck.AddHandCard(id);
        int value = buffControler.GetDragCardDamage(true);
        MinusHp(value);
        cardsUI.SetCardAndReturnUniqueID(card, UISource.GetUIInfo(card.type));
    }



    public void CountDamage(int value)
    {
        buffControler.CheckBomb(value);
    }
    public void TryUseCard(Guid guid, Character target)
    {
        var isUseAble = CheckUseAble(guid);
        if (isUseAble)
        {
            UseCard(guid, target);
        }
    }
    List<Buff> DoSomethingToOther = new List<Buff>();
    private void UseCard(Guid guid, Character target)
    {
        Card card = GetCard(guid);
        BattleSystem.AddUsedCost(card.cost);
        SetEP(Ep - card.cost);

        if (DoSomethingToOther.Count!=0)
        {
            if (DoSomethingToOther[0].ID == EffecID.CancelNextCard)
            {

            }
            else if (DoSomethingToOther[0].ID == EffecID.RubNextCard)
            {
                card.DoAction(target, this);
            }
            DoSomethingToOther.RemoveAt(0);
        }
        else
        {
            card.DoAction(this, target);
        }
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
    public int GetAttackValue(int value, EffecID effecID,bool use=true)
    {
        if (effecID==EffecID.Damage)
        {
            return buffControler.GetAttackValue(value,use);
        }
        else
        {
            return buffControler.GetMagicAttackValue(value, use);
        }
    }

    internal void EndTurn()
    {
        buffControler.DoBuffTimeCount(RoundPeriod.end);
        buffControler.DoBuffTimeCount(RoundPeriod.enemyStart);
    }

    internal void InvokeBigMagiic()
    {
        buffControler.InvokeBigMagiic();
    }

    internal void StartTurn()
    {
        buffControler.DoBuffTimeCount(RoundPeriod.enemyEnd);
        buffControler.DoBuffTimeCount(RoundPeriod.start);
    }

    internal int GetDefendValue()
    {
        return 0;
    }

    internal void SetChangCard(Func<Card> changFunction)
    {
        deck.ChangeCard = changFunction;
    }
    internal void RemoveChangCard()
    {
        deck.ChangeCard = null;
    }

    internal void RemoveBuff(bool removeGoodBuff)
    {
        buffControler.RemoveBuff(removeGoodBuff);
    }

    internal void RemoveBuff(EffecID rage)
    {
        buffControler.RemoveBuff(rage);
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

