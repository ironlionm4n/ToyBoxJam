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
    [SerializeField] protected bool PermanantLowering = true;
    [SerializeField] protected float RiseSpeed = 5f;
    [SerializeField] protected float TimeBeforeRise = 0f;

    protected Vector2 startPosition;
    protected Vector2 movePosition;
    protected bool moving = false;
    protected float timer = 0f;

    private void Start()
    {
        startPosition = transform.localPosition;
        movePosition = MovePosition.localPosition;
    }

    private void Update()
    {
        if (moving && !(transform.position.y == movePosition.y))
        {
            timer = TimeBeforeRise;
            transform.localPosition = new Vector2(transform.localPosition.x, 
                Mathf.MoveTowards(transform.localPosition.y, movePosition.y, MoveSpeed * Time.deltaTime));
        }
        else if(!moving && !(transform.position.y == startPosition.y))
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                return;
            }

            if (!PermanantLowering) 
            {
                transform.localPosition = new Vector2(transform.localPosition.x,
                Mathf.MoveTowards(transform.localPosition.y, startPosition.y, RiseSpeed * Time.deltaTime));
            }
        }
    }

    public void SetLowering(bool isLowering)
    {
        moving = isLowering;
    }
}
