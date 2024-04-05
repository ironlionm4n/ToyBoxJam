using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode_Conditional : BTNodeBase
{
    protected System.Func<bool> ConditionFn;
    protected bool WasPreviouslyAbleToRun = false;

    public BTNode_Conditional(string _Name, System.Func<bool> _ConditionFn): 
        base(_Name)
    {
        ConditionFn = _ConditionFn;
        OnEnterFn = EvaluateCondition;
        OnTickFn= EvaluateCondition;

    }

    protected BehaviorTree.ENodeStatus EvaluateCondition()
    {
        bool canRun = ConditionFn != null ? ConditionFn.Invoke(): false;


        if(canRun != WasPreviouslyAbleToRun)
        {
            WasPreviouslyAbleToRun = canRun;

            foreach(var child in Children)
            {
                child.Reset();
            }
        }

        return canRun ? BehaviorTree.ENodeStatus.InProgress : BehaviorTree.ENodeStatus.Failed;
    }
}
