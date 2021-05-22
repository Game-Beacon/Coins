using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class TestSystems : MonoBehaviour
{
    private void Awake()
    {
        UISource.Intialize();
        CardInfoSource.Intialize();
        BattleSystem.Intialize();
        StartCoroutine(IntializedScene());
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
public class Fakecharactor : Character
{
    public string name;

    public Fakecharactor()
    {
    }
    public Fakecharactor(string name)
    {
        this.name = name;
    }

    
}
