using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class TestSystems : MonoBehaviour
{
    public static TestSystems Instance=>instance;
    private static TestSystems instance;
    [SerializeField] private BattlePresenter battlePresenter;
    private void Start()
    {
        if (instance!= null)
        {
            Tool.DeBugWarning("system should only one");
        }
        else
        {
            instance = this;
        }
        UISource.Intialize();
        CardInfoSource.Intialize();
        BattleSystem.Intialize();
        battlePresenter.Intialize();
        StartCoroutine(IntializedScene());
    }
    public void EndGame()
    {
        battlePresenter.GameEnd();
    }
    IEnumerator IntializedScene()
    {
        while (UISource.isLoading)
        {
            yield return null;
        }
        BattleSystem.GameStart();
    }
}
