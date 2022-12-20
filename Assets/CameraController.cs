using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float interpolant;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float fallingInterpolant = 10f;
    [SerializeField] private Rigidbody2D playerRigidbody;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, playerTransform.position + offset,interpolant * Time.deltaTime);
    }
}