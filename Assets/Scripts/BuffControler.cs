using System;
using System.Collections.Generic;

class BuffControler
{
    private Character user;
    private List<Buff> buffs = new List<Buff>();
    BuffUIList buffUI;
    public int GetAttackValue(int attackValue,bool use)
    {
        int value = GetBuffInvokeValue(attackValue, EffecID.Rage, use);
        value = GetBuffInvokeValue(value, EffecID.Frozen, use);
        value = GetBuffInvokeValue(value, EffecID.AddDamage, use);
        if (use)
        {
            GetBuffInvokeValue(value,EffecID.Bomb,use);
            RemoveBuff();
        }
        return value;
    }
    public int GetMagicAttackValue(int attackValue, bool use)
    {
        int value= GetBuffInvokeValue(attackValue,EffecID.AddMgicDamage,use);
        value = GetBuffInvokeValue(value, EffecID.Frozen, use);
        if (use)
        {
            RemoveBuff();
        }
        return value;
    }
    int bombNeedToBeCanceled=5;
    public void CheckBomb(int value)
    {
        if (bombNeedToBeCanceled <= value)
        {
            RemoveBome();
        }
    }

    private void RemoveBome()
    {
        var index = buffs.FindIndex((buff)=>buff.ID==EffecID.Bomb);
        if (index!=-1)
        {
            buffs[index].Remove=true;
            RemoveBuff();
        }
    }

    public int GetDragCardDamage(bool use)
    {
        return GetBuffInvokeValue(0,EffecID.DamageOnDrag,use);
    }
    private int GetBuffInvokeValue(int value, EffecID effecID,bool use)
    {
        var index = buffs.FindIndex((buff) => buff.ID == effecID);
        if (index != -1)
        {
            value = buffs[index].Invoke(value, use);
            if (use)
            {
                buffUI.Invoke(buffs[index]);
                RemoveBuff();
            }
        }

        return value;
    }

    public BuffControler(Character user,Character enemy)
    {
        this.user = user;
    }
    public void SetBuffUI(BuffUIList buffUIList)
    {
        buffUI = buffUIList;
        //buffUI = new DefaultBuffUI();
    }
    public void DoBuffTimeCount(RoundPeriod roundPeriod)
    {
        buffs.ForEach((buff)=>buff.DoTimeCountAction(roundPeriod));
        RemoveBuff();
    }

    private void RemoveBuff()
    {
        var removeBuffs= buffs.FindAll(buff => buff.Remove);
        foreach (var item in removeBuffs)
        {
            buffUI.RemoveBuff(item);
        }
        buffs = buffs.FindAll(buff => buff.Remove == false);
    }

    internal void Add(Buff buff)
    {
        var index= buffs.FindIndex((buffInList)=>buffInList.ID== buff.ID);
        if (index==-1 || !buffs[index].IsOverlay)
        {
            buffs.Add(buff);
            buffUI.AddBuff(buff);
        }
        else
        {
            buffs[index].Overlay(buff.Value);
            buffUI.AddBuff(buffs[index]);
        }
    }

    internal void RemoveBuff(bool removeGoodBuff)
    {
        var list= buffs.FindAll((buff)=>buff.PositiveBuff==removeGoodBuff);
        var random = new Random();
        if (list.Count>0)
        {
            int index = random.Next(list.Count);
            list[index].Remove = true;
            RemoveBuff();
        }
    }

    internal void RemoveBuff(EffecID rage)
    {
        var index= buffs.FindIndex((buff) => buff.ID == rage);
        if (index!=-1)
        {
            buffs[index].Remove = true;
            RemoveBuff();
        }
    }

    internal void InvokeBigMagiic()
    {
        var index = buffs.FindIndex((buff) => buff.ID == EffecID.BigMagic);
        if (index != -1)
        {
            buffs[index].Invoke(0,true);
            buffs[index].Remove = true;
            RemoveBuff();
        }
    }
}

public interface BuffUIList
{
    void AddBuff(Buff buff);
    void RenewBuff(Buff buff);
    void Invoke(Buff buff);
    void RemoveBuff(Buff buff);
}
public class DefaultBuffUI : BuffUIList
{
    public void AddBuff(Buff buff)
    {
        Debug(buff);
    }

    public void Invoke(Buff buff)
    {
        Debug(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        Debug(buff);
    }

    public void RenewBuff(Buff buff)
    {
        Debug(buff);
    }
    void Debug(Buff buff)
    {
        string value = buff.PositiveBuff ? "GoodBuff" : "BadBuff";
        Tool.DeBug($"{buff.owener.name}:{value}:{buff.GetTitle()}{buff.GetContent()}");
    }
}

