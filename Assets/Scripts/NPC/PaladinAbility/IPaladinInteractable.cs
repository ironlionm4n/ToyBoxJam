using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPaladinInteractable
{
    public EPaladinGrappleTypes GrappleType { get; set; }

    public void InteractionStarted();

    public void InteractionFinished();

    public Vector2 GetPosition();

    public Transform GetTransform();
   
}
