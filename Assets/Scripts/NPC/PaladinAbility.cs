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
    private SpringJoint2D springJoint;

    private List<IPaladinInteractable> interactables;

    private bool usingAbility = false;

    // Start is called before the first frame update
    void Start()
    {
        springJoint = GetComponent<SpringJoint2D>();

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

        if (usingAbility && currentTarget != null)
        {
            if (currentTarget.GetComponent<IPaladinInteractable>().GrappleType == EPaladinGrappleTypes.Moveable)
            {
                Vector2 dirToHook = currentTarget.GetChild(0).transform.position - transform.position;

                //TODO - Rewrite to use velocity?

                transform.position += (Vector3)(transform.right * -dirToHook * 10 * Time.deltaTime);
            }
        }

    }

    public override void UseAbility()
    {
        if(currentTarget == null)
        {
            return;
        }

        springJoint.enabled = true;


        springJoint.connectedBody = currentTarget.GetComponent<Rigidbody2D>();

        usingAbility = true;
    }

    public override void AbilityComplete()
    {
        base.AbilityComplete();

        springJoint.connectedBody = null;
        springJoint.enabled = false;

        usingAbility = false;
    }

    public List<IPaladinInteractable> FindPossibleGrappableObjects()
    {
        // TODO - Call this method on every scene load so we get a new list of all possible interactables

        IEnumerable<IPaladinInteractable> paladinInteractables = FindObjectsOfType<MonoBehaviour>()
            .OfType<IPaladinInteractable>();

        return new List<IPaladinInteractable>(paladinInteractables);
    }
}
