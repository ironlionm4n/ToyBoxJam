using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Script : Test_Interface
{
    // Start is called before the first frame update
    public void Attack()
    {
        Debug.Log("Attacking");
    }

    // Update is called once per frame
    public void EndAttack()
    {
        Debug.Log("Stopping");
    }
}
