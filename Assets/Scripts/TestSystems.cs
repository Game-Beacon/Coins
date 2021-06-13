using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class TestSystems : MonoBehaviour
{
    public static TestSystems Instance;
    [SerializeField] private BattlePresenter battlePresenter;
    private void Start()
    {
        Instance = this;
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
