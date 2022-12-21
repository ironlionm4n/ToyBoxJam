using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Homing : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D rigidbody;

    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float chaseTime = 10f;
    [SerializeField] private float chaseTimer = 0f;
    [SerializeField] private float currentSize = 5f;
    [SerializeField] private float sizeChangeAmount = 0.1f;
    [SerializeField] private float rotateSpeed = 200f;




    private void OnEnable()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(chaseTimer >= chaseTime)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (Vector2)player.transform.position - rigidbody.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rigidbody.angularVelocity= -rotateAmount * rotateSpeed;

        rigidbody.velocity = transform.up * speed;



        transform.localScale = new Vector3(currentSize, currentSize);

        chaseTimer += Time.deltaTime;
        currentSize -= sizeChangeAmount/chaseTime;
    }
}
