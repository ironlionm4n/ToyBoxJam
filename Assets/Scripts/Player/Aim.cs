using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Aim : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private GameObject coinSpawnPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerStats playerStats;
    
    [Header("Variables")]
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private float rotZ;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float circleCastRadius;
    
    public Vector3 CoinSpawnPointUp => coinSpawnPoint.transform.up;
    public Transform CoinSpawnPointTransform => coinSpawnPoint.transform;

    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - pivotPoint.transform.position;
        rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        /*if(rotZ < -30 && rotZ > -140)
        {
            if((mousePosition.x) < (transform.position.x))
            {
                rotZ = -150;
            } else if ((mousePosition.x) > (transform.position.x))
            {
                rotZ = -30;
            }
        }*/

        //Debug.DrawRay(coinSpawnPoint.transform.position, coinSpawnPoint.transform.up, Color.cyan);
        pivotPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ-90);
        playerStats.SetCanFire(CheckIfShootingIntoGround()); 
    }

    private bool CheckIfShootingIntoGround()
    {
        var position = coinSpawnPoint.transform.position;
        var direction = transform.position - position;
        var midPoint = (transform.position + position) / 2;
        var ray = Physics2D.CircleCast(position, circleCastRadius, direction.normalized, direction.magnitude, groundLayer);
        var midPointRay = Physics2D.CircleCast(midPoint, circleCastRadius, -direction.normalized, direction.magnitude * .75f, groundLayer);
        if(ray.collider || midPointRay.collider)
        {
            Debug.DrawRay(position, direction, Color.magenta);
            return false;
        }
        Debug.DrawRay(position, direction, Color.yellow);    
        return true;
    }
}
