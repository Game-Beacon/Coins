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
        UISource.intialize();
        CardSource.Intialize();
        cards=CardSource.cards;
        inputUnirx = Observable.EveryUpdate()
            .Subscribe(DetectMouseButton);
    }
    int index=-1;
    private void DetectMouseButton(long obj)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (index>=cards.Count-1)
            {
                index = -1;
            }
            cards[++index].SetCardUI(uI);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (index<=0)
            {
                index = cards.Count;
            }
            cards[--index].SetCardUI(uI);
        }

    }
    private void OnDestroy()
    {
        inputUnirx.Dispose();
    }
}
