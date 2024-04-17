using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    GolemManager golemManager;
    GolemHand golemHand;

    Transform player;

    private bool lookAtPlayer = true;
    private float turnSpeed = 5f;

    private void Start()
    {
        golemManager = transform.parent.GetComponent<GolemManager>();
        golemHand = GetComponent<GolemHand>();

        player = golemManager.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer)
        {
            //transform.right = player.position - transform.position;

            // Aiming
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (golemHand.RightHand)
            {
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, 0, -1, 0) * rotation, turnSpeed * Time.deltaTime);
            }
            else
            {
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
            }
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

    public void ResetRotation()
    {
        transform.DORotate(Vector2.zero, 0.2f);
    }
}
