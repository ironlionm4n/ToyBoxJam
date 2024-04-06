using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCBrain : MonoBehaviour
{
    private Transform player;

    public Transform Player { get { return player; } }

    //Keeps track of each states priority for the state machine
    private Dictionary<NPCStates, float> statePriorities = new Dictionary<NPCStates, float>();

    public Dictionary<NPCStates, float> StatePriorities { get { return statePriorities; } }

    private List<StateObject> stateQueue = new List<StateObject>();

    private StateObject currentState;

    private StateObject defaultState = new StateObject(NPCStates.Idle, 0);

    private NPCFollow followCommands;

    private NPCJump jumpCommands;

    private NPCIdle idleCommands;

    StateObject idleState;
    StateObject jumpingState;
    StateObject fallingState;
    StateObject movingState;
    StateObject abilityState;
    StateObject dodgingState;

    private void Awake()
    {
       IntializePriorities();

        player = GameObject.Find("Player").transform;
        currentState = defaultState; 

        followCommands = GetComponent<NPCFollow>();
        jumpCommands = GetComponent<NPCJump>();
        idleCommands = GetComponent<NPCIdle>();

        
        idleState = new StateObject(NPCStates.Idle, statePriorities.GetValueOrDefault(NPCStates.Idle));
        
        fallingState = new StateObject(NPCStates.Falling, statePriorities.GetValueOrDefault(NPCStates.Falling));
        movingState = new StateObject(NPCStates.Moving, statePriorities.GetValueOrDefault(NPCStates.Moving));
        abilityState = new StateObject(NPCStates.AbilityUsed, statePriorities.GetValueOrDefault(NPCStates.AbilityUsed));
        dodgingState = new StateObject(NPCStates.Dodging, statePriorities.GetValueOrDefault(NPCStates.Dodging));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleRequest(StateObject requestedState, bool completed)
    {


        //If the request has been completed
        if(completed)
        {
            Debug.Log(requestedState.NPCStates + " " + completed);
            //Remove the old state and switch to the next in queue
            if(stateQueue.Contains(requestedState))
            {
                stateQueue.Remove(requestedState);

                if (stateQueue.Count > 0)
                {
                    ChangeState(stateQueue[0]);
                }
                else
                {
                    ChangeState(defaultState);
                }
            }
            else
            {
                Debug.LogWarning("WARNING: Attempted to remove state not currently in the state queue");
            }

            return;
        }

        if(stateQueue.Contains(requestedState))
        {
            //Don't queue up the same request multiple times
            return;
        }

        Debug.Log(requestedState.NPCStates + " " + completed);

        stateQueue.Add(requestedState);

        Debug.Log(stateQueue.Count);

        stateQueue.OrderByDescending(x => x.Priority).FirstOrDefault();

        ChangeState(stateQueue[0]);
    }

    private void ChangeState(StateObject newState)
    {
        if(newState == currentState)
        {
            //Don't need to change to same state
            return;
        }

        currentState = newState;

        switch (newState.NPCStates)
        {
            case NPCStates.Following:
                //followCommands.StartFollowing();
                jumpCommands.StopJumping();
                break;

            case NPCStates.Idle:
                idleCommands.Idle();
                break;

            case NPCStates.Jumping:
                followCommands.StopFollowing();
                //jumpCommands.StartJumping();
                break;

            case NPCStates.Moving: 
                break;

            case NPCStates.Dodging:
                break;

            case NPCStates.AbilityUsed: 
                break;
        }
    }

    private void IntializePriorities()
    {
        //Idle has the lowest priority out of any and should be always overwritten
        statePriorities.Add(NPCStates.Idle, 0.0f);

        //Following is next lowest and should be overwritten by many other requests
        statePriorities.Add(NPCStates.Following, 0.1f);

        //Jumping should overwrite slowing so the AI can get to player but not much else
        statePriorities.Add(NPCStates.Jumping, 0.2f);

        //Slightly above Jumping, this state means the AI is moving to a specific location to complete a task (i.e. moving somewhere to jump up to player)
        statePriorities.Add(NPCStates.Moving, 0.25f);

        //Third highest priority, not much should overwrite this in normal circumnstances
        statePriorities.Add(NPCStates.Falling, 0.3f);

        //Second highest priority, for added realism
        statePriorities.Add(NPCStates.Dodging, 0.4f);

        //HIGHEST PRIORITY FOR GAME FEEL
        statePriorities.Add(NPCStates.AbilityUsed, 1f);
    }

    private int FindStateInQueue(StateObject state)
    {
        int index = -1;
        NPCStates stateIdentifier = state.NPCStates;

        for(int i = 0; i < stateQueue.Count; i++)
        {
            if (stateQueue[0].NPCStates == stateIdentifier)
            {
                index = i;
            }
        }

        Debug.Log(state.NPCStates + " " + index);

        return index;
    }
}
