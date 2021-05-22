using System;
using System.Collections;
using UniRx;
using UnityEngine.Rendering.VirtualTexturing;

public abstract class Character
{
    protected ReactiveProperty<int> hp { get; private set; } = new ReactiveProperty<int>(100);
    protected ReactiveProperty<int> ep { get; private set; }=new ReactiveProperty<int>(100);
    protected ReactiveProperty<int> armor { get; private set; }=new ReactiveProperty<int>(0);

    public int Hp => hp.Value;
    public int Ep => ep.Value;

    public bool IsDead { get; private set; } = false;
    
    public int Armor => armor.Value;
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

    public void SetHP(int value)
    {
        hp.Value = value;
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
    public void SetArmor(int value)
    {
        armor.Value = value;
    }
}
