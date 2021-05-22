using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class UISource
{
    private static CardUIInfo attack;
    private static CardUIInfo talent;
    private static CardUIInfo order;
    public static bool isLoading { get;private set; }

    private static int _loadingCount;
    private static int loadingCount
    {
        get
        {
            return _loadingCount;
        }
        set
        {
            _loadingCount = value;
            SetIsLoading();
        }
    }

    private static void SetIsLoading()
    {
        if (loadingCount>0)
        {
            isLoading = true;
        }
        else
        {
            isLoading = false;
        }
    }

    public static void Intialize()
    {
        LoadAddressable<Sprite>("picture/card_red.png",attack.bg);
        LoadAddressable<Sprite>("picture/card_green.png",talent.bg);
        LoadAddressable<Sprite>("picture/card_yellow.png",order.bg);
        LoadAddressable<Sprite>("picture/frame_yellow.png",order.card);
        LoadAddressable<Sprite>("picture/frame_green.png",talent.card);
        LoadAddressable<Sprite>("picture/frame_red.png",attack.card);
        // Addressables.LoadAssetAsync<Sprite>("picture/card_red.png").Completed += (i) => { attack.bg= i.Result; };
        // Addressables.LoadAssetAsync<Sprite>("picture/card_green.png").Completed += (i) => { talent.bg = i.Result; };
        // Addressables.LoadAssetAsync<Sprite>("picture/card_yellow.png").Completed += (i) => { order.bg = i.Result; };
        // Addressables.LoadAssetAsync<Sprite>("picture/frame_yellow.png").Completed += (i) => { order.card= i.Result; };
        // Addressables.LoadAssetAsync<Sprite>("picture/frame_green.png").Completed += (i) => { talent.card= i.Result; };
        // Addressables.LoadAssetAsync<Sprite>("picture/frame_red.png").Completed += (i) => { attack.card = i.Result; };
    }
    
    public  static void LoadAddressable<T>(string address,T ob)
    {
        loadingCount++;
        Addressables.LoadAssetAsync<T>(address).Completed += (i) => { ob= i.Result; loadingCount--;};
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