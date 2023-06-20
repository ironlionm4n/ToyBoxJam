using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D effector;
    [SerializeField] private bool fallThrough = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FallThrough()
    {
        if (!fallThrough)
        {
            fallThrough = true;
            StartCoroutine(GoDown());
        }
    }

    public IEnumerator GoDown()
    {
        effector.surfaceArc = 0;

        yield return new WaitForSeconds(0.5f);

        effector.surfaceArc = 180;

        yield return null;

        fallThrough = false;
    }
}
