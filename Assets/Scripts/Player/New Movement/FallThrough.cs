using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThrough : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, -transform.up, 0.75f);

            foreach (RaycastHit2D item in hit)
            {

                if (item && item.transform != transform)
                {
                    if (item.transform.GetComponent<PlatformEffector2D>() != null)
                    {
                        Debug.Log("Hit");
                        PlatformEffector2D hitEffector = item.transform.GetComponent<PlatformEffector2D>();
                        item.transform.GetComponent<OneWay>().FallThrough();
                    }
                }
            }
        }
    }


}
