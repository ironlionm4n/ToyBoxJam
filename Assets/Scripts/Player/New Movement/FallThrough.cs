using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThrough : MonoBehaviour
{
    public LayerMask layerMask;
    private BoxCollider2D groundCheck;
    // Start is called before the first frame update
    void Start()
    {
        groundCheck = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
