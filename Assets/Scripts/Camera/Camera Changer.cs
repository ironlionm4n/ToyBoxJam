using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    public static CameraChanger instance;

    [SerializeField] CinemachineVirtualCamera CurrentCam;
    [SerializeField] CinemachineVirtualCamera NextCam;

    private void Awake()
    {
        instance = this;
    }
    public void ChangeCam(CinemachineVirtualCamera newCam)
    {
        NextCam = newCam;
        CurrentCam.Priority = 0;
        NextCam.Priority = 1;
        CurrentCam = NextCam;
    }
}
