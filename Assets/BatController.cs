using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float maxDistanceDelta;
    private BatStates _currentState;
    private Vector3 _currentDestination;
    private int _waypointIndex;

    enum BatStates
    {
        Patrolling,
        Chasing
    }

    private void Start()
    {
        _waypointIndex = 0;
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
            UpdateCurrentDestination();
        }
    }

    private void UpdateCurrentDestination()
    {
        _waypointIndex++;
        if (_waypointIndex >= waypoints.Length)
        {
            _waypointIndex = 0;
            _currentDestination = waypoints[_waypointIndex].position;
        }
        else
        {
            _currentDestination = waypoints[_waypointIndex].position;
        }
    }
}
