using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private GameObject[] checkpoints;
    
    private GameObject _currentCheckpoint;

    public GameObject CurrentCheckpoint => _currentCheckpoint;
}
