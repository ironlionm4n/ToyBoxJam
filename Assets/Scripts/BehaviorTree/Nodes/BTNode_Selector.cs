using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode_Selector : BTNodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return true;
    }

    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return false;
    }

    protected override void OnTickedAllChildren()
    {
        //^1 is shortcut to access last child
        LastStatus = Children[^1].LastStatus;
    }
}
