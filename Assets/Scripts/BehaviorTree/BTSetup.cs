using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]
public class BTSetup : MonoBehaviour
{
    protected BehaviorTree LinkedBT;
    protected NPCAgent Agent;
    protected AwarenessSystem Sensors;

    void Awake()
    {
        Agent = GetComponent<NPCAgent>();
        LinkedBT= GetComponent<BehaviorTree>();
        Sensors = GetComponent<AwarenessSystem>();

        var BTRoot = LinkedBT.RootNode.Add<BTNode_Selector>("Base Logic");

        var jumpRoot = BTRoot.Add(new BTNode_Conditional("Can Jump",
            () =>
            {
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
                return BehaviorTree.ENodeStatus.Succeeded;
            }); 
            
    }


}
