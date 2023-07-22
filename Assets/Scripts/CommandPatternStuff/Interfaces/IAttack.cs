
using System.Collections.Generic;
using UnityEngine;

public interface IAttack 
{
    public void Attack(IAction action);

    public void StopAttack();
}
