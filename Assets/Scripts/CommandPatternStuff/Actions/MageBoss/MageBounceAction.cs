using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBounceAction : IAction
{
    public GameObject player {get; private set;}

    public GameObject projectile { get; private set;}

    public MageBounceAction(GameObject player, GameObject projectile)
    {
        this.player = player;
        this.projectile = projectile;
    }

    public void Execute()
    {

    }
    
    public void Stop()
    {

    }
}
