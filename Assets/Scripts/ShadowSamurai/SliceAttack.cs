using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceAttack : MonoBehaviour
{
    [SerializeField]
    private float sliceYIncSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + sliceYIncSpeed * Time.deltaTime, transform.localScale.z);

        if(transform.localScale.y >= 14)
        {
            Destroy(gameObject);
        }
    }
}
