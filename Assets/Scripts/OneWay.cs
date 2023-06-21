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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            effector.surfaceArc = 179;
            fallThrough = false;
        }
    }
}
