using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private float minX = -16.9f;
    [SerializeField] private float maxX = 17.5f;
    [SerializeField] private float minY = 1.5f;
    [SerializeField] private float maxY = 5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float interpolant;
    [SerializeField] private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector2.Lerp(transform.position, playerTransform.position + offset,interpolant * Time.deltaTime);
        
        transform.position = Vector2.Lerp(transform.position, new Vector3(Mathf.Clamp(player.transform.position.x, minX, maxX), 
            Mathf.Clamp(player.transform.position.y, minY, maxY), -10), interpolant * Time.deltaTime);
    }
}