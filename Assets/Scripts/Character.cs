using System;
using UniRx;
public abstract class Character
{
    protected Character(Value hp, Value ep,int drawCountWhenYourTurn)
    {
        maxHP.Value = hp.MaxValue;
        this.hp.Value = hp.NowValue;
        maxEP.Value = ep.NowValue;
        this.ep.Value = ep.NowValue;
        drawCount.Value = drawCountWhenYourTurn;
    }

    protected ReactiveProperty<int> maxHP { get; private set; } = new ReactiveProperty<int>();
    protected ReactiveProperty<int> hp { get; private set; } = new ReactiveProperty<int>();
    protected ReactiveProperty<int> maxEP { get; private set; } = new ReactiveProperty<int>();

    protected ReactiveProperty<int> ep { get; private set; }=new ReactiveProperty<int>();
    protected ReactiveProperty<int> drawCount { get; private set; } = new ReactiveProperty<int>(1);
    protected Deck deck;

    protected ReactiveProperty<int> armor { get; private set; }=new ReactiveProperty<int>();


    public int Hp => hp.Value;
    public int Ep => ep.Value;
    public int DrawCountWhenYourTurn => drawCount.Value;
    public int Armor => armor.Value;


    public bool IsDead { get; private set; } = false;
    
    public IDisposable SubscribeHP(Action<int> fuc)
    {
        return hp.Subscribe(fuc);
    }
    public IDisposable SubscribeArmor(Action<int> fuc)
    {
        return armor.Subscribe(fuc);
    }
    public IDisposable SubscribeEP(Action<int> fuc)
    {
        return ep.Subscribe(fuc);
    }

    internal Card AddHandCard(Guid guid)
    {
        return deck.AddHandCard(guid);
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
    internal Card GetCard(Guid guid)
    {
        return deck.GetCard(guid);
    }

    internal void EndTurn()
    {
        //throw new NotImplementedException();
    }

    internal void StartTurn()
    {
        //throw new NotImplementedException();
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
    public string name;
    public Fakecharactor():base(new Value(100), new Value(100),1) { }
    public Fakecharactor(string name):base(new Value(100,100), new Value(100, 100),1) 
    {
        this.name = name;
    }
}

