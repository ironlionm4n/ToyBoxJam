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
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - pivotPoint.transform.position;
        rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if(rotZ < -30 && rotZ > -140)
        {
            if((mousePosition.x) < (transform.position.x))
            {
                rotZ = -150;
            } else if ((mousePosition.x) > (transform.position.x))
            {
                rotZ = -30;
            }
        }

        pivotPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ-90);
        
        playerStats.SetCanFire(CheckIfShootingIntoGround()); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(coinSpawnPoint.GetComponent<Transform>().position, Vector2.down);
    }

    private bool CheckIfShootingIntoGround()
    {
        var ray = Physics2D.Raycast(coinSpawnPoint.GetComponent<Transform>().position, Vector2.down, .1f, groundLayer);
        
        if(ray.transform != null)
            if (ray.transform.name.Equals("Ground") || ray.transform.name.Equals("OneWayPlatforms")) return false;
        
        return true;
    }
}
