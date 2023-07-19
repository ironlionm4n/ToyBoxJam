using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{

    private DirectionEnum direction;

    [SerializeField] private float moveSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if(direction == DirectionEnum.right)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if(direction == DirectionEnum.left)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if(direction == DirectionEnum.up)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurnPoint"))
        {
            RollerTurnPoint rtp = collision.GetComponent<RollerTurnPoint>();

            direction = rtp.Direction;
        }
    }
}
