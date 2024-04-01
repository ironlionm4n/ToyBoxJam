using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    GolemManager golemManager;

    Transform player;

    private bool lookAtPlayer = true;

    private void Start()
    {
        golemManager = transform.parent.GetComponent<GolemManager>();
        player = golemManager.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer)
        {
            transform.right = player.position - transform.position;
        }
    }

    public void StartRotating()
    {
        lookAtPlayer= true;
    }

    public void StopRotating()
    {
        lookAtPlayer= false;
    }
}
