using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
public class CardPreview : MonoBehaviour
{
    public CardUI uI;
    private IDisposable inputUnirx;
    List<Card> cards;
    void Awake()
    {
        UISource.Intialize();
        CardInfoSource.Intialize();
        cards=CardInfoSource.cards;
    }
    int index=-1;
    
    private void OnDestroy()
    {
        inputUnirx.Dispose();
    }
}
