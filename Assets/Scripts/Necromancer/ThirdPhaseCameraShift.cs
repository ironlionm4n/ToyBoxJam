using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPhaseCameraShift : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera ThirdPhaseCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraChanger.instance.ChangeCam(ThirdPhaseCam);
        }
    }
}
