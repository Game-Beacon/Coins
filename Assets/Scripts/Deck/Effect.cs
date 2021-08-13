using System;
using System.Collections.Generic;

public interface IEffect
{
    EffecID ID { get; }
    string GetContent();
    void Cast(Character user, Character target);
    void SetNextAction(IEffect action);
}
[Serializable]
public abstract class Effect : IEffect
{
    protected IEffect nextAction;

    public abstract EffecID ID { get; }

    public void Cast(Character user, Character target)
    {
        DoAction(user, target);
        nextAction?.Cast(user, target);
    }
    protected abstract void DoAction(Character user, Character target);
    public string GetContent()
    {
        string value = GetEffectContent();
        string value2 = nextAction?.GetContent();
        return value2 == null ? value : value + value2;
    }
    protected abstract string GetEffectContent();

    public void SetNextAction(IEffect action)
    {
        nextAction = action;
    }
}
[Serializable]
public class Damage : Effect
{
    int damage;
    public Damage(int value, bool magicDamgae = false)
    {
        damage = value;
        if (magicDamgae)
        {
            damageID = EffecID.MagicDamage;
        }
        else
        {
            damageID = EffecID.Damage;
        }
    }
    public override EffecID ID => damageID;
    EffecID damageID;
    protected override void DoAction(Character user, Character target)
    {
        int value = user.GetAttackValue(damage, ID) + target.GetDefendValue();
        BattleSystem.CountDamage(user, value);
        target.MinusHp(value);
    }

    protected override string GetEffectContent()
    {
        if (damageID == EffecID.MagicDamage)
        {
            return $"造成{damage}點魔法傷害";
        }
        else
        {
            return $"造成{damage}點傷害";
        }
    }
}

[Serializable]
public class RemoveArmor : Effect
{
    int removeArmor;
    public RemoveArmor(int value)
    {
        removeArmor = value;
    }

    public override EffecID ID => EffecID.RemoveArmor;

    protected override void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor - removeArmor;
        target.SetArmor(tempvalue);
    }

    protected override string GetEffectContent()
    {
        return $"移除敵人{removeArmor }點護甲";
    }
}
[Serializable]
public class GainArmor : Effect
{
    int armor;
    public GainArmor(int value)
    {
        armor = value;
    }

    public override EffecID ID => EffecID.GainArmor;

    protected override void DoAction(Character user, Character target)
    {
        int tempvalue = target.Armor + armor;
        target.SetArmor(tempvalue);
    }

    protected override string GetEffectContent()
    {
        return $"獲得{armor}點護甲";
    }
}
[Serializable]
public class RecoverHP : Effect
{
    int heal;
    public RecoverHP(int value)
    {
        heal = value;
    }

    public override EffecID ID => EffecID.RecoverHP;

    protected override void DoAction(Character user, Character target)
    {
        user.AddHp(heal);
    }

    protected override string GetEffectContent()
    {
        return $"回復{heal}點血量";
    }
}
[Serializable]
public class RecoverEP : Effect
{
    int recoverEP;
    public override EffecID ID => EffecID.RecoverEP;

    protected override void DoAction(Character user, Character target)
    {
        user.SetEP(user.Ep + recoverEP);
    }

    protected override string GetEffectContent()
    {
        return $"回復{recoverEP}點魔力";
    }
}
[Serializable]
public class AddCard : Effect
{
    int addCardNumber;
    public AddCard(List<int> ids)
    {
        this.ids = ids;
        addCardNumber = ids.Count;
    }
    public override EffecID ID => EffecID.AddCard;
    List<int> ids;
    protected override void DoAction(Character user, Character target)
    {
        int[] array = new int[ids.Count];
        ids.CopyTo(array);
        user.AddCards(array);
    }

    protected override string GetEffectContent()
    {
        return $"抽{addCardNumber}張牌";
    }
}

public class RemoveBuff : Effect
{
    public RemoveBuff(bool removeGoodBuff)
    {
        random = true;
        this.removeGoodBuff = removeGoodBuff;
    }
    Action action;
    public RemoveBuff(EffecID effecID)
    {
        removeID = effecID;
    }
    public override EffecID ID => EffecID.RemoveBuff;
    EffecID removeID;
    bool random;
    bool removeGoodBuff;
    protected override void DoAction(Character user, Character target)
    {
        if (random)
        {
            user.RemoveBuff(removeGoodBuff);
        }
        else
        {
            user.RemoveBuff(removeID);
        }
    }

    protected override string GetEffectContent()
    {
        if (random)
        {
            return "移除buff";
        }
        else
        {
            return $"移除{removeID}buff";
        }
    }
}
public class InvokeBigMagiic : Effect
{
    public override EffecID ID => EffecID.InvokeBigMagiic;

    protected override void DoAction(Character user, Character target)
    {
        user.InvokeBigMagiic();
    }

    protected override string GetEffectContent()
    {
        throw new NotImplementedException();
    }
}



[Serializable]
public abstract class Buff : IEffect
{
    protected IEffect nextAction;
    public Character owener { get; private set; }
    protected Character enemy;
     
    public Buff(bool targetIsUser, RoundPeriod roundPeriod)
    {
        this.targetIsUser = targetIsUser;
        RoundPeriod = roundPeriod;
    }
    public RoundPeriod RoundPeriod { get; private set; }
    public abstract bool PositiveBuff { get; }
    private bool targetIsUser { get; set; }
    public bool Remove { get; set; }
    public abstract EffecID ID { get; }
    public abstract bool IsOverlay { get; }
    public abstract int Value { get; }
    public Guid Guid { get; set; }

    public void Overlay(int value)
    {
        OverlayAction(value);
    }
    protected abstract void OverlayAction(int value);
    public void Cast(Character user, Character enemy)
    {
        owener = targetIsUser ? user : enemy;
        this.enemy = targetIsUser ? enemy : user;
        owener.AddBuff(this);
        CastAction();
        nextAction?.Cast(user, enemy);
    }
    public void DoTimeCountAction(RoundPeriod roundPeriod)
    {
        CountTime(roundPeriod);
        if (Remove)
        {
            Reverse();
        }
    }
    protected abstract void CountTime(RoundPeriod roundPeriod);
    protected virtual void CastAction() { }
    protected virtual void Reverse() { }
    public string GetContent()
    {
        string value = Tool.AddWord(GetTitle(), GetBuffContent());
        value = Tool.AddWord(value, nextAction?.GetContent());
        return value;
    }
    public abstract string GetTitle();
    protected abstract string GetBuffContent();
    public void SetNextAction(IEffect action)
    {
        nextAction = action;
    }
    public virtual int Invoke(int value, bool use)
    {
        return value;
    }
}
public class AddDamage : Buff
{
    public AddDamage(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        this.addDamageValue = value;
    }

    public override bool PositiveBuff => true;

    public override EffecID ID => EffecID.AddDamage;

    public override bool IsOverlay => true;

    public override int Value => addDamageValue;
    protected int addDamageValue;


    List<int> overlayValue = new List<int>();
    public override string GetTitle()
    {
        return "AddDamage";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        Remove = RoundPeriod == roundPeriod;
    }

    protected override string GetBuffContent()
    {
        return "增加本回合下次傷害";
    }
    public override int Invoke(int attackValue, bool use)
    {
        int tempvalue = attackValue + addDamageValue;
        foreach (var value in overlayValue)
        {
            if (value == -1)
            {
                tempvalue *= 2;
            }
            else
            {
                tempvalue += value;
            }
        }
        if (use)
        {
            Remove = true;
        }
        return tempvalue;
    }

    protected override void OverlayAction(int value)
    {
        overlayValue.Add(value);
    }
}

public class AddDamageInOneGame : AddDamage
{
    public AddDamageInOneGame(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod,value)
    {
    }
    public override EffecID ID => EffecID.AddDamageInOneGame;

    public override bool IsOverlay => true;

    public override string GetTitle()
    {
        return "AddDamageInOneGame";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {

    }

    protected override string GetBuffContent()
    {
        return "";
    }
    public override int Invoke(int attackValue, bool use)
    {
        int tempvalue = attackValue + addDamageValue;
        return tempvalue;
    }

    protected override void OverlayAction(int value)
    {
        this.addDamageValue += value;
    }
}
public class Rage : Buff
{
    public override EffecID ID => EffecID.Rage;

    public override bool IsOverlay => true;

    public override int Value => value;

    public override bool PositiveBuff => true;

    private int value;
    public Rage(bool setOnUser, RoundPeriod roundPeriod, int RageValue = 1) : base(setOnUser, roundPeriod)
    {
        value = RageValue;
    }
    public override int Invoke(int value, bool use)
    {
        return value + this.value;
    }
    public override string GetTitle()
    {
        return "Rage";
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        owener.MinusHp(Value);
        Remove = true;
    }
    protected override string GetBuffContent()
    {
        return "";
    }

    protected override void OverlayAction(int value)
    {
        this.value += value;
    }
}
public class AddMgicDamage : Buff
{
    public override EffecID ID => EffecID.AddMgicDamage;

    public override bool IsOverlay => true;

    public override int Value => value;

    public override bool PositiveBuff => true;

    private int value;
    public AddMgicDamage(bool setOnUser, RoundPeriod roundPeriod, int addMgicDamageValue = 1) : base(setOnUser, roundPeriod)
    {
        value = addMgicDamageValue;
    }
    public override int Invoke(int value, bool use)
    {
        return value + this.value;
    }
    public override string GetTitle()
    {
        return "AddMgicDamage";
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        Remove = true;
    }
    protected override string GetBuffContent()
    {
        return "";
    }

    protected override void OverlayAction(int value)
    {
        this.value += value;
    }
}
public class Forzen : Buff
{
    private int value { get; set; }

    public override EffecID ID => EffecID.Frozen;

    public override bool IsOverlay => true;

    public override int Value => value;

    public override bool PositiveBuff => false;

    public Forzen(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        this.value = value;
    }

    public override string GetTitle()
    {
        return "forzen";
    }

    protected override string GetBuffContent()
    {
        return $"冰凍效果{value}";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod == RoundPeriod)
        {
            Remove = true;
        }
    }
    public override int Invoke(int attackValue, bool use)
    {
        if (attackValue >= value)
        {
            attackValue -= value;
            if (use) Remove = true;
        }
        else
        {
            if (use)
            {
                value -= attackValue;
            }
            attackValue = 0;
        }
        return attackValue;
    }
    protected override void OverlayAction(int value)
    {
        this.value += Value;
    }
}
public class DamageOnDrag : Buff
{
    private int damage { get; set; }

    public override EffecID ID => EffecID.DamageOnDrag;

    public override bool IsOverlay => true;

    public override int Value => damage;

    public override bool PositiveBuff => false;

    public DamageOnDrag(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        this.damage = value;
    }

    public override string GetTitle()
    {
        return "DamageOnDrag";
    }

    protected override string GetBuffContent()
    {
        return $"抽牌受傷{damage}";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
    }
    public override int Invoke(int notUse, bool use)
    {
        int value = damage;

        if (use)
        {
            damage--;
            Remove = damage == 0;
        }
        return value;
    }
    protected override void OverlayAction(int value)
    {
        this.damage += Value;
    }
}
public class Bomb : Buff
{
    public Bomb(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        turns = value;
    }
    public override bool PositiveBuff => false;

    public override EffecID ID => EffecID.Bomb;

    public override bool IsOverlay => false;

    public override int Value => turns;
    int turns;
    int damgaeHaveToDo = 5;
    public override string GetTitle()
    {
        return "Bomb";
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod == RoundPeriod)
        {
            turns--;
        }
        if (turns <= 0)
        {
            owener.MinusHp(damgaeHaveToDo);
            Remove = true;
        }
    }

    protected override string GetBuffContent()
    {
        return $"炸彈傷害{damgaeHaveToDo}";
    }
    public override int Invoke(int value, bool use)
    {
        if (use)
        {
            damgaeHaveToDo -= value;
            Remove = damgaeHaveToDo <= 0;
        }
        return value;
    }
    protected override void OverlayAction(int value)
    {
    }
}
public class ChangeCard : Buff
{
    public ChangeCard(bool setOnUser, RoundPeriod roundPeriod, int value) : base(setOnUser, roundPeriod)
    {
        cardID = value;
    }
    public override bool PositiveBuff => true;

    public override EffecID ID => EffecID.ChangeCard;

    public override bool IsOverlay => true;

    public override int Value => cardID;

    int cardID;
    public override string GetTitle()
    {
        return "ChangeCard";
    }
    protected override void CastAction()
    {
        owener.SetChangCard(ChangFunction);
    }
    private Card ChangFunction()
    {
        List<IEffect> list = new List<IEffect>() { new Forzen(false, RoundPeriod.end, 3) };
        return Card.TestCard(list);
    }
    protected override void Reverse()
    {
        owener.RemoveChangCard();
        Remove = true;
    }
    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod == RoundPeriod)
        {
            owener.RemoveChangCard();
            Remove = true;
        }
    }
    protected override string GetBuffContent()
    {
        return "";
    }

    public override int Invoke(int value, bool use)
    {
        return cardID;
    }
    protected override void OverlayAction(int cardID)
    {
        this.cardID = cardID;
    }
}

public class Poison : Buff
{
    public Poison(bool setOnUser, RoundPeriod roundPeriod, int damagePercent, int turnNumber) : base(setOnUser, roundPeriod)
    {
        this.damagePercent = damagePercent;
        this.turnNumber = turnNumber;
    }
    public override bool PositiveBuff => false;

    public override EffecID ID => EffecID.Poison;

    public override bool IsOverlay => true;

    public override int Value => turnNumber;
    private int turnNumber;
    private int damagePercent;

    public override string GetTitle()
    {
        return "Poison";
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (roundPeriod == RoundPeriod)
        {
            int damage = owener.Hp * 5 / 100;
            owener.MinusHp(damage);
            turnNumber--;
        }
        if (turnNumber <= 0)
        {
            Remove = true;
        }
    }

    protected override string GetBuffContent()
    {
        return $"造成{damagePercent}%傷害";
    }

    protected override void OverlayAction(int value)
    {
        turnNumber = value;
    }
}
public class BigMagicContainer : Buff
{
    public BigMagicContainer(bool setOnUser, RoundPeriod roundPeriod,int id) : base(setOnUser, roundPeriod)
    {
        bigMagic = AddBigMagic(id);
    }
    public override bool PositiveBuff => true;

    public override EffecID ID => EffecID.BigMagic;

    public override bool IsOverlay => true;

    public override int Value => bigMagic.endTurn;

    BigMagic bigMagic;
    public override string GetTitle()
    {
        return bigMagic.Title();
    }

    protected override void CountTime(RoundPeriod roundPeriod)
    {
        if (RoundPeriod==roundPeriod)
        {
            bigMagic.countTimeAction(roundPeriod, BattleSystem.GetUseMagic());
        }
        if (bigMagic.endTurn<=0)
        {
            Remove = true;
        }
    }
    protected override void CastAction()
    {
        bigMagic.SetOwner(owener,enemy);
    }
    protected override string GetBuffContent()
    {
        return bigMagic.Content();
    }

    protected override void OverlayAction(int value)
    {
        bigMagic = AddBigMagic(value);
        bigMagic.SetOwner(owener,enemy);
    }

    public override int Invoke(int value, bool use)
    {
        if (use)
        {
            bigMagic.DoDamage();
        }
        return 0;
    }

    static BigMagic AddBigMagic(int value)
    {
        return new  ForzenBigMagic();
    } 

     abstract class BigMagic
    {
        protected Character owener;
        protected Character enemy;
        public int damageOnEnd;

        public void SetOwner(Character owener,Character enemy)
        {
            this.owener = owener;
            this.enemy = enemy;
        }
        public abstract int endTurn { get; set; }

        internal abstract string Content();

        internal void countTimeAction(RoundPeriod roundPeriod,int value)
        {
            damageOnEnd += value;
            endTurn--;
            countTimeAction(value);
            if (endTurn<=0)
            {
                DoDamage();
            }
        }

        public void DoDamage()
        {
            new Damage(damageOnEnd, true).Cast(owener,enemy);
        }

        protected abstract void countTimeAction(int value);
        internal abstract string Title();
    }

    class ForzenBigMagic : BigMagic
    {
        public override int endTurn { get; set; } = 5;

        protected override void countTimeAction(int value)
        {
            new Forzen(false, RoundPeriod.end, value).Cast(owener,enemy);
        }

        internal override string Content()
        {
            return "ForzenBigMagic";
        }

        internal override string Title()
        {
            return "ForzenBigMagicContent";
        }
    }
    class FireBigMagic : BigMagic
    {
        public override int endTurn { get; set; } = 5;

        protected override void countTimeAction(int value)
        {
            new DamageOnDrag(false, RoundPeriod.end, value).Cast(owener, enemy);
        }

        internal override string Content()
        {
            return "FireBigMagic";
        }

        internal override string Title()
        {
            return "FireBigMagicContent";
        }
    }
    class AddMgicDamageBigMagic : BigMagic
    {
        public override int endTurn { get; set; } = 5;

        protected override void countTimeAction(int value)
        {
            new AddMgicDamage(false, RoundPeriod.end, value).Cast(owener, enemy);
        }

        internal override string Content()
        {
            return "AddMgicDamageBigMagic";
        }

        internal override string Title()
        {
            return "AddMgicDamageBigMagicContent";
        }
    }



}

