using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D effector;
    public bool fallThrough { get; private set; } = false;

    public void FallThrough()
    {
        effector.surfaceArc = 0;
        fallThrough = true;
    }

}
