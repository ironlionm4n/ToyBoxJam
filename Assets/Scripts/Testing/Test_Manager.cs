using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Manager : MonoBehaviour
{
    [SerializeField] private Test_Scriptable testing;

    private void Start()
    {
        testing.test.Attack();
    }
}
