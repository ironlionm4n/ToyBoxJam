using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;

    private int _currentHeartCount = 3;
    public int CurrentHeartCount => _currentHeartCount;

    public void ReduceHeartCount()
    {
        _currentHeartCount--;
        Debug.Log(_currentHeartCount);
    }
}
