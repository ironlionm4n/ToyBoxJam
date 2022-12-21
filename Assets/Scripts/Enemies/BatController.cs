using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BatController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float maxDistanceDelta;
    [Tooltip("Determines if the bat will follow a linear path or a random one")]
    [SerializeField] private bool isWaypointBat;
    private BatStates _currentState;
    private Vector3 _currentDestination;
    private int _waypointIndex;
    private bool shouldMoveToOne = true;
    
    enum BatStates
    {
        Patrolling,
        Chasing
    }

    private void Start()
    {
        _waypointIndex = Random.Range(0, waypoints.Length);
        _currentState = BatStates.Patrolling;
        _currentDestination = waypoints[_waypointIndex].position;

    }

    private void Update()
    {

        switch (_currentState)
        {
            case BatStates.Patrolling:
            {
                WaypointPatrolling();
                break;
            }
            case BatStates.Chasing:
            {
                break;
            }
            default: Debug.Break(); break;
        }
    }

    private void WaypointPatrolling()
    {
        transform.position = Vector2.Lerp(transform.position, _currentDestination, maxDistanceDelta * Time.deltaTime);
        var hasArrived = Vector2.Distance(transform.position, _currentDestination) < .1f;
        if (hasArrived)
        {
            if(isWaypointBat)
                UpdateCurrentDestination();
            else
            {
                RandomUpdateCurrentDestination();
            }
        }
    }

    private void RandomUpdateCurrentDestination()
    {
        _currentDestination = waypoints[Random.Range(0, waypoints.Length)].position;
    }

    private void UpdateCurrentDestination()
    {
        _waypointIndex++;
        
        if (_waypointIndex >= waypoints.Length) _waypointIndex = 0;
        
        _currentDestination = waypoints[_waypointIndex].position;
    }
}
