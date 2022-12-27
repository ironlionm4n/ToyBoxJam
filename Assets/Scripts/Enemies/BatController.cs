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
    [SerializeField] private GameObject player;

    [Header("Blocking")]
    [SerializeField] private bool isBlockingBat;
    [SerializeField] private float minY = -16.7f;
    [SerializeField] private float maxY = -14.43f;
    [SerializeField] private float elapsedTime = 0;
    [SerializeField] private float blockTime = 3f;

    [Header("Death Particle System Section")]
    [SerializeField] ParticleSystem batDeathParticles;
    [SerializeField] Collider2D batCollider;
    [SerializeField] SpriteRenderer batRenderer;

    [Header("Audio Source Section")]
    [SerializeField] AudioSource batDeathAudioSource;

    private BatStates _currentState;
    private Vector3 _currentDestination;
    private int _waypointIndex;
    private bool shouldMoveToOne = true;
    
    enum BatStates
    {
        Patrolling,
        Chasing,
        Blocking
    }

    private void Start()
    {
        _waypointIndex = Random.Range(0, waypoints.Length);
        if (!isBlockingBat)
        {
            _currentState = BatStates.Patrolling;
        }
        else
        {
            _currentState= BatStates.Blocking;
        }
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
            case BatStates.Blocking:
            {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Mathf.Clamp(player.transform.position.y, minY, maxY)), blockTime);
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

    public void Dead()
    {
        //Death animation?
        StartCoroutine(PlayDeathParticles());
    }
    private IEnumerator PlayDeathParticles()
    {
        batDeathAudioSource.Play();
        batDeathParticles.Play();
        batCollider.enabled = false;
        batRenderer.enabled = false;
        while (batDeathParticles.isPlaying) yield return null;
        Destroy(gameObject);
    }
}
