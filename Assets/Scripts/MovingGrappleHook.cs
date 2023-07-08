using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingGrappleHook : MonoBehaviour
{
    [SerializeField] private Transform startingPoint;
    [SerializeField] private Transform endPosition;


    [SerializeField]private float moveTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Starts moving the hook towards the ending position
    /// </summary>
    public void PlayerSnapped()
    {
        transform.DOKill();

        //Calculates total distance from starting to end position
        var fullDistance = Vector2.Distance(startingPoint.position, endPosition.position);

        //Calculate velocity of object given the full distance and move time
        var velocity = fullDistance / moveTime;

        //Calculates how much further the object needs to move
        var remainingDistance = Vector2.Distance(transform.position, endPosition.position);

        //Calculate move time
        var time = remainingDistance / velocity;

        transform.DOMove(endPosition.position, time).SetEase(Ease.Linear);
    }

    /// <summary>
    /// Starts moving the hook back to the starting position
    /// </summary>
    public void PlayerUnSnapped()
    {
        transform.DOKill();

        //Calculates total distance from starting to end position
        var fullDistance = Vector2.Distance(startingPoint.position, endPosition.position);

        //Calculate velocity of object given the full distance and move time
        var velocity = fullDistance / moveTime;

        //Calculates how much further the object needs to move
        var remainingDistance = Vector2.Distance(transform.position, startingPoint.position);

        //Calculate move time
        var time = remainingDistance / velocity;

        transform.DOMove(startingPoint.position, time).SetEase(Ease.Linear);
    }
}
