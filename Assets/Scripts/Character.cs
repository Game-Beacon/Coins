using System;
using System.Collections;
using UniRx;
using UnityEngine.Rendering.VirtualTexturing;

public abstract class Character
{
    protected ReactiveProperty<int> hp=new ReactiveProperty<int>(100);
    protected ReactiveProperty<int> ep=new ReactiveProperty<int>(100);
    public IDisposable SubscribeHP(Action<int> fuc)
    {
        return hp.Subscribe(fuc);
    }

    
    public IDisposable SubscribeEP(Action<int> fuc)
    {
        return ep.Subscribe(fuc);
    }

    public void SetHP(int value)
    {
        hp.Value = value;
    }
}
