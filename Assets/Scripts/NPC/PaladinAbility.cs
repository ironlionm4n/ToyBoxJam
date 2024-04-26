using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaladinAbility : NPCAbility
{
    [SerializeField]
    private float maxDistanceForTarget = 6f;

    [SerializeField]
    private Transform currentTarget;

    private LineRenderer grappleLine;

    private List<IPaladinInteractable> interactables;



    // Start is called before the first frame update
    void Start()
    {
        interactables = FindPossibleGrappableObjects();
    }

    // Update is called once per frame
    void Update()
    {
        // sort the list so the closest interactable is first
        IPaladinInteractable closestIneractable = interactables.OrderByDescending(x => Vector2.Distance(x.GetPosition(), transform.position))
            .Reverse()
            .FirstOrDefault();

        // check if the closest interactable is within the max distance
        if(Vector2.Distance(closestIneractable.GetPosition(), transform.position) < maxDistanceForTarget)
        {
            currentTarget = closestIneractable.GetTransform();
        }
        else
        {
            currentTarget = null;
        }

    }

    public override void UseAbility()
    {
        
    }

    public override void AbilityComplete()
    {
        base.AbilityComplete();
    }

    public List<IPaladinInteractable> FindPossibleGrappableObjects()
    {
        // TODO - Call this method on every scene load so we get a new list of all possible interactables

        IEnumerable<IPaladinInteractable> paladinInteractables = FindObjectsOfType<MonoBehaviour>()
            .OfType<IPaladinInteractable>();

        return new List<IPaladinInteractable>(paladinInteractables);
    }
}
