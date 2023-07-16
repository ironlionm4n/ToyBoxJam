using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private GameObject bounceAttack;

    private BossInvoker bossInvoker;

    public Action<MageFlailAction> action;

    // Start is called before the first frame update
    void Start()
    {
        bossInvoker = InvokerHolder.instance.bossInvoker;

        MageFlailAction testAction = new MageFlailAction(transform, bounceAttack, 5, movePoints, true, 2, 2f);
        action?.Invoke(testAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
