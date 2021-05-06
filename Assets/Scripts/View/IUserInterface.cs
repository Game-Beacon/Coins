using System;
using UnityEngine;

public abstract class IUserInterface :MonoBehaviour
{
    protected GameObject _RootUI;
    protected bool _bActive=false;
    
    public bool IsVisable()
    {
        return _bActive;
    }
    public  virtual void RootClick()
    {
        _bActive = !_bActive;
        _RootUI.SetActive(_bActive);
    }
}

