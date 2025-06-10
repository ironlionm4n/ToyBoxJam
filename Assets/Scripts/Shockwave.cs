using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] private float Strength;
    [SerializeField] private float UpForceModifier = 0.5f;
    [SerializeField] private float Range = 10f;
    [SerializeField] private bool DisablePlayerMovement = false;
    [SerializeField] private float KnockbackTime = 1f;

    void Start()
    {
        //ShockwaveStart();
    }
    
    public void ShockwaveStart()
    {
        if(ShockwaveFeeler.instance != null)
        {
            ShockwaveFeeler.instance.ShockwaveFelt(Strength,UpForceModifier, transform.position, Range, DisablePlayerMovement, KnockbackTime);
        }
    }
}
