using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class UISource
{
    private static Dictionary<Type, CardUIInfo> cardUIInfos = new Dictionary<Type, CardUIInfo>();
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
            Debug.Log("Loading");
        }
        else
        {
            isLoading = false;
            Debug.Log("LoadingComplete");
        }
    }

    public static void Intialize()
    {
        LoadAddressable<CardData>("Data/CardData.asset",SetCardData);
    }
    
    public  static void LoadAddressable<T>(string address,Action<T> action)
    {
        loadingCount++;
        Addressables.LoadAssetAsync<T>(address).Completed += (i) =>
        {
            action(i.Result);
            loadingCount--;
        };
    }

    static void SetCardData(CardData cardData)
    {
        foreach (var VARIABLE in cardData.cardUIInfos)
        {
            if (!cardUIInfos.ContainsKey(VARIABLE.type))
            {
                cardUIInfos.Add(VARIABLE.type,VARIABLE);
            }
        }
    }

    public static CardUIInfo GetUIInfo(Type type)
    {
        return cardUIInfos[type];
    }
}