using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class that holds all the invokers used for the command pattern
/// </summary>
public class InvokerHolder : MonoBehaviour
{
    public static InvokerHolder instance;

    public BossInvoker bossInvoker { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("More than one invoker holder, destroying " + gameObject.name);
            Destroy(gameObject);
        }

        bossInvoker = new BossInvoker();
    }
}
