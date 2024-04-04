using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]
public class BTSetup : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private float follow_range = 8f;

    protected BehaviorTree LinkedBT;
    protected NPCAgent Agent;
    protected AwarenessSystem Sensors;

    void Awake()
    {
        Agent = GetComponent<NPCAgent>();
        LinkedBT= GetComponent<BehaviorTree>();
        Sensors = GetComponent<AwarenessSystem>();

        var BTRoot = LinkedBT.RootNode;

        var followRoot = BTRoot.Add<BTNode_Sequence>("Wander");
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
    }


}
