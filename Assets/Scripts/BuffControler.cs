using System;
using System.Collections.Generic;

class BuffControler
{
    private Character user;
    private List<Buff> buffs = new List<Buff>();


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
        var index = buffs.FindIndex((buff)=>buff.effecID==EffecID.Bomb);
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
        var index = buffs.FindIndex((buff) => buff.effecID == effecID);
        if (index != -1)
        {
            value = buffs[index].Invoke(value, use);
            if (use)
            {
                ShowBuff(buffs[index]);
                RemoveBuff();
            }
        }
        return value;
    }

    public BuffControler(Character user,Character enemy)
    {
        this.user = user;
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
            ShowRemoveBuff(item);
        }
        buffs = buffs.FindAll(buff => buff.Remove == false);
    }

    internal void Add(Buff buff)
    {
        var index= buffs.FindIndex((buffInList)=>buffInList.effecID== buff.effecID);
        if (index==-1 || !buffs[index].IsOverlay)
        {
            buffs.Add(buff);
            ShowBuff(buff);
        }
        else
        {
            buffs[index].Overlay(buff.Value);
            ShowBuff(buffs[index]);
        }
    }
    private void ShowRemoveBuff(Buff buff)
    {
        string value = buff.PositiveBuff ? "GoodBuff" : "BadBuff";
        Tool.DeBug($"{user.name}:Remove{value}:{buff.GetTitle()}");
    }
    private void ShowBuff(Buff buff)
    {
        string value = buff.PositiveBuff ? "GoodBuff" : "BadBuff";
        Tool.DeBug($"{user.name}:{value}:{buff.GetTitle()}{buff.GetContent()}");
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
        var index= buffs.FindIndex((buff) => buff.effecID == rage);
        if (index!=-1)
        {
            buffs[index].Remove = true;
            RemoveBuff();
        }
    }
}

