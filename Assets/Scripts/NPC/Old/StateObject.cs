using System.Collections;
using System.Collections.Generic;

public class StateObject 
{
    private NPCStates npcState;
    private float priority;

    public NPCStates NPCStates { get { return npcState; } }
    public float Priority { get { return priority; } }  

    public StateObject(NPCStates state, float prior)
    {
        npcState= state;
        priority = prior;
    }
}
