using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private GameObject bounceAttack;

    [SerializeField] private GameObject trackingProjectile;

    private BossInvoker bossInvoker;

    public Action<MageFlailAction> flailAttack;

    public Action<MageBounceAction> bounceEffect;

    public Action<MageSideBouncerAction> sideBouncingAttack;

    // Start is called before the first frame update
    void Start()
    {
        bossInvoker = InvokerHolder.instance.bossInvoker;

        MageFlailAction testAction = new MageFlailAction(transform, bounceAttack, 5, movePoints, true, 2, 2f);
        //flailAttack?.Invoke(testAction);

        MageBounceAction test1 = new MageBounceAction(GameObject.Find("Player"), trackingProjectile);

        // bounceEffect?.Invoke(test1);

        MageSideBouncerAction sideBounce = new MageSideBouncerAction(20);

        sideBouncingAttack?.Invoke(sideBounce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
