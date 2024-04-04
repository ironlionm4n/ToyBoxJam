using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode_Sequence : BTNodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return false;
    }

    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }

    protected override void OnTickedAllChildren()
    {
        //^1 is shortcut to access last child
        LastStatus = Children[^1].LastStatus;
    }
}
