using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public CardUIInfo[] cardUIInfos;
}
[Serializable]
public class CardUIInfo
{
    public Type type;
    public Sprite bg;
    public Sprite card;
}