using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class UISource
{
    private static CardUIInfo attack;
    private static CardUIInfo talent;
    private static CardUIInfo order;
    public static void intialize()
    {
        Addressables.LoadAssetAsync<Sprite>("picture/card_red.png").Completed += (i) => { attack.bg= i.Result; };
        Addressables.LoadAssetAsync<Sprite>("picture/card_green.png").Completed += (i) => { talent.bg = i.Result; };
        Addressables.LoadAssetAsync<Sprite>("picture/card_yellow.png").Completed += (i) => { order.bg = i.Result; };
        Addressables.LoadAssetAsync<Sprite>("picture/frame_yellow.png").Completed += (i) => { order.card= i.Result; };
        Addressables.LoadAssetAsync<Sprite>("picture/frame_green.png").Completed += (i) => { talent.card= i.Result; };
        Addressables.LoadAssetAsync<Sprite>("picture/frame_red.png").Completed += (i) => { attack.card = i.Result; };
    }
    public static CardUIInfo GetBG(Type type)
    {
        CardUIInfo cardUIInfo=new CardUIInfo();
        switch (type)
        {
            case Type.attack:
                cardUIInfo= attack;
                break;
            case Type.order:
                cardUIInfo= order;
                break;
            case Type.talent:
                cardUIInfo=talent;
                break;
            default:
                Tool.DeBugWarning("wrong type");
                break;
        }
        return cardUIInfo;
    }
}
public struct CardUIInfo
{
    public Sprite bg;
    public Sprite card;
}
public static class CardSource
{
    public static List<Card> cards { get; private set; }
    public static void Intialize()
    {
        cards= FileHandler.ReadCardsFile();
    }
}

