using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class Test : MonoBehaviour
{
    public void TestDrag()
    {
        transform.position = Input.mousePosition;
    }
    
}

