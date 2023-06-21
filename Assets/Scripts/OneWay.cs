using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D effector;
    [SerializeField] private bool fallThrough = false;
    private bool switchFallThrough = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (!fallThrough)
            {
                effector.surfaceArc = 180;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            effector.surfaceArc = 0;
        }
    }

    private void FixedUpdate()
    {
        if (switchFallThrough)
        {
            switchFallThrough = false;
            effector.surfaceArc = 180;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            fallThrough = true;
            switchFallThrough = false;
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fallThrough = false;
            switchFallThrough = true;
        }
    }
}
