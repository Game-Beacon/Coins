using System.Collections.Generic;
using System;
using UnityEngine;
public class ImplementBuffList : MonoBehaviour, BuffUIList
{
    [SerializeField] private BuffUnit prefabUnit;

    private Dictionary<Guid, BuffUnit> usingUnits=new Dictionary<Guid, BuffUnit>();
    private LinkedList<BuffUnit> notUsingUnits=new LinkedList<BuffUnit>();
    BuffUnit notUsingUnit
    {
        get
        {
            BuffUnit unit;
            if (notUsingUnits.Count>0)
            {
                unit = notUsingUnits.First.Value;
                unit.SetActive(true);
                notUsingUnits.RemoveFirst();
            }
            else
            {
                unit = Instantiate(prefabUnit, transform);
            }
            unit.transform.SetSiblingIndex(-1);
            usingUnits.Add(unit.Guid,unit);
            return unit;
        }
    }

    public void AddBuff(Buff buff)
    {
        var unit = notUsingUnit;
        buff.Guid= unit.Set(buff);
    }

    public void Invoke(Buff buff)
    {
        BuffUnit unit = SetBuff(buff);
        unit.Invoke();
    }

    private BuffUnit SetBuff(Buff buff)
    {
        var unit = usingUnits[buff.Guid];
        unit.Set(buff);
        return unit;
    }

    public void RemoveBuff(Buff buff)
    {
        var unit= usingUnits[buff.Guid];
        notUsingUnits.AddLast(unit);
        unit.SetActive(false);

        usingUnits.Remove(buff.Guid);
    }

    public void RenewBuff(Buff buff)
    {
        SetBuff(buff);
    }
}
