using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Helps reduce total number of rendered sprites
        if (collision.tag == "Despawn")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        gameObject.GetComponentInParent<Indicator>().Destroy();
    }
}
