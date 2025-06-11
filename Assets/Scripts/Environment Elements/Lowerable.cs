using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lowerable : MonoBehaviour
{
    [Header("Lowering")]
    [SerializeField] protected float MoveSpeed = 5f;
    [SerializeField] protected Transform MovePosition;

    [Header("Rising")]
    /// <summary>
    /// Determines if the object will rise after the player stops lowering it
    /// </summary>
    [SerializeField] protected bool permanantLowering = true;
    [SerializeField] protected float RiseSpeed = 5f;

    protected Vector2 startPosition;
    protected Vector2 movePosition;
    protected bool moving = false;

    private void Start()
    {
        startPosition = transform.position;
        movePosition = MovePosition.position;
    }

    private void Update()
    {
        if (moving && transform.position.y > movePosition.y)
        {
            transform.position += (Vector3)Vector2.down * MoveSpeed * Time.deltaTime;
        }
        else if(!moving)
        {
            if (!permanantLowering && transform.position.y < startPosition.y) 
            {
                transform.position += (Vector3)Vector2.up * RiseSpeed * Time.deltaTime;
            }
        }
    }

    public void SetLowering(bool isLowering)
    {
        moving = isLowering;
    }
}
