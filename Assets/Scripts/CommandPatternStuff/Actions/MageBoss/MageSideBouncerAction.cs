using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSideBouncerAction : IAction
{
    public float survivalTime {  get; private set; }

    public MageSideBouncerAction(float survivalTime)
    {
        this.survivalTime = survivalTime;
    }

    public void Execute()
    {

    }

    public void Stop()
    {

    }
}
