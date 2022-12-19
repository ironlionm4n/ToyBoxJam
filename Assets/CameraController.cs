using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float interpolant;
    [SerializeField] private Vector3 offset;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, interpolant * Time.fixedDeltaTime);
    }
}