using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinMoveable : MonoBehaviour, IPaladinInteractable
{
    public EPaladinGrappleTypes GrappleType { get;  set; }

    private Transform hookPoint;

    public Transform HookPoint { get {  return hookPoint; } }


    private void Awake()
    {
        hookPoint = transform.GetChild(0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractionStarted()
    {

    }

    public void InteractionFinished()
    {

    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
