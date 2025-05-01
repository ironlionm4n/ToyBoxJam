using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneProjectile : MonoBehaviour
{
    [SerializeField] private Vector3 MoveDir;
    [SerializeField] private float MoveSpeed;

    [SerializeField] private float SpinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += MoveDir * MoveSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(SpinSpeed / 2, 0, SpinSpeed));
    }
}
