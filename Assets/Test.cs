using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class Test : MonoBehaviour
{
    public Transform[] cards;
    public Transform card;
    public int index;
    [ContextMenu("testUI_SetSiblingIndex")]
    public void SetSiblingIndex()
    {
        card.SetSiblingIndex(index);
    }
}

