using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]
public class BTSetup : MonoBehaviour
{
    protected BehaviorTree LinkedBT;
    protected NPCAgent Agent;
    protected AwarenessSystem Sensors;

    private KeyCode abilityKey = KeyCode.Q;

    private bool usingAbility = false;

    void Awake()
    {
        Agent = GetComponent<NPCAgent>();
        LinkedBT= GetComponent<BehaviorTree>();
        Sensors = GetComponent<AwarenessSystem>();

        var BTRoot = LinkedBT.RootNode.Add<BTNode_Selector>("Base Logic");

        var abilityRoot = BTRoot.Add(new BTNode_Conditional("Using Ability",
            () =>
            {
                if (!usingAbility)
                {
                    Agent.StopAbility();
                }

                return usingAbility;
            }));

        abilityRoot.Add<BTNode_Action>("Use Ability",
            () =>
            {
                Agent.UseAbility();
                return BehaviorTree.ENodeStatus.InProgress;
            });

        var jumpRoot = BTRoot.Add(new BTNode_Conditional("Can Jump",
           () =>
           {
               //Debug.Log(Agent.CheckIfNeedToJump());
               return Agent.CheckIfNeedToJump();
           }));

        jumpRoot.Add<BTNode_Action>("Perform Jump",
            () =>
            {
                Agent.StartJumping();
                return BehaviorTree.ENodeStatus.InProgress;
            },
            () =>
            {
                return !Agent.CheckIfNeedToJump() ? BehaviorTree.ENodeStatus.Succeeded : BehaviorTree.ENodeStatus.InProgress;
            });


        var followRoot = BTRoot.Add (new BTNode_Conditional("Can Follow",
            () =>
            {
                return Agent.CheckIfNeedFollow();
            }));

        followRoot.Add<BTNode_Action>("Perform Follow",
            () =>
            {
                Agent.StartFollowing();
                return BehaviorTree.ENodeStatus.InProgress;
            },
            () =>
            {
                return !Agent.CheckIfNeedFollow() ? BehaviorTree.ENodeStatus.Succeeded : BehaviorTree.ENodeStatus.InProgress;
            });

      

        var idleRoot = BTRoot.Add<BTNode_Sequence>("Idle");
        idleRoot.Add<BTNode_Action>("Idle Action",
             () =>
             {
                 return BehaviorTree.ENodeStatus.InProgress;
             },
            () =>
            {
                if (Agent.CheckIfNeedToJump() || Agent.CheckIfNeedFollow() || usingAbility)
                {
                    return BehaviorTree.ENodeStatus.Succeeded;
                }

                return BehaviorTree.ENodeStatus.InProgress;
            }); 
            
    }


    private void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            usingAbility = !usingAbility;
            LinkedBT.RootNode.Reset();
        }
    }

}
