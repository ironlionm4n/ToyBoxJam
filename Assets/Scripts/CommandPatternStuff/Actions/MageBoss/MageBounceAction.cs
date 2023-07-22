using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBounceAction : IAction
{
    public GameObject player {get; private set;}

    public GameObject roller { get; private set;}

    public MageBounceAction(GameObject player, GameObject roller)
    {
        this.player = player;
        this.roller = roller;
    }

    public void Execute()
    {

    }
    
    public void Stop()
    {

    }
}
