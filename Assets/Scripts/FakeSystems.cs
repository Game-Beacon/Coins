using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSystems : MonoBehaviour
{
    private void Awake()
    {
        UISource.intialize();
        CardSource.Intialize();
        BattleSystem.Intialize();
        StartCoroutine(Intialize());
    }


    IEnumerator Intialize()
    {
        yield return new WaitForSeconds(2);
        BattleSystem.ShowHandCards();
    }




}
public class fakecharactor : Character { }
