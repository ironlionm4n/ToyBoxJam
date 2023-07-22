using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private GameObject bounceAttack;

    [SerializeField] private GameObject roller;

    public Action<MageFlailAction> flailAttack;

    public Action stopFlailAttack;

    public Action<MageBounceAction> bounceEffect;

    public Action stopBounceEffect;

    public Action<MageSideBouncerAction> sideBouncingAttack;

    public Action stopSideBouncingAttack;

    MageFlailAction flailAttackAction;

    MageBounceAction rollerAttack;

    MageSideBouncerAction sideBounce;

    private void Awake()
    {
        flailAttackAction = new MageFlailAction(transform, bounceAttack, 5, movePoints, true, 2, 2f);

        rollerAttack = new MageBounceAction(GameObject.Find("Player"), roller);

        sideBounce = new MageSideBouncerAction(20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFlailAttack()
    {
        flailAttack.Invoke(flailAttackAction);
    }

    public void StartRollerAttack()
    {
        bounceEffect.Invoke(rollerAttack);
    }

    public void StartSideBounceAttack()
    {
        sideBouncingAttack.Invoke(sideBounce);
    }

    public void StopFlailAttack()
    {
        stopFlailAttack.Invoke();
    }

    public void StopRollerAttack()
    {
        stopBounceEffect.Invoke();
    }

    public void StopSideBouncingAttack()
    {
        stopSideBouncingAttack.Invoke();
    }
}
