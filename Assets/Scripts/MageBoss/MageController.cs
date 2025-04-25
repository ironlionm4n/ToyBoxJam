using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private GameObject bounceAttack;

    [SerializeField] private float attackCooldown;

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

    IAttack flailingAttack;

    IAttack playerBounceAttack;

    IAttack grappleDodgeAttack;

    public Action<int, float> phaseChange;

    public int phase { get; private set; } = 1;

    private void Awake()
    {
        flailingAttack = GetComponent<FlailAttack>();
        flailAttackAction = new MageFlailAction(transform, bounceAttack, 5, movePoints, true, 2, 2f);

        playerBounceAttack = GetComponent<PlayerBounceAttack>();
        rollerAttack = new MageBounceAction(GameObject.Find("Player"), roller);

        grappleDodgeAttack = GetComponent<GrappleDodgeAttack>();
        sideBounce = new MageSideBouncerAction(20);
    }

    private void Start()
    {
        StartCoroutine(HandleAttackChange(null, flailingAttack, flailAttackAction, 1f));
    }

    public void OnEnable()
    {
        GetComponent<MageStats>().hit += HandleHit;
    }

    public void OnDisable()
    {
        GetComponent<MageStats>().hit -= HandleHit;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HandleHit(float health)
    {
        if(health == 30)
        {
            phase = 3;

            StartCoroutine(HandleAttackChange(playerBounceAttack, grappleDodgeAttack, sideBounce, attackCooldown));
        }
        else if(health == 70)
        {
            phase = 2;

            StartCoroutine(HandleAttackChange(flailingAttack, playerBounceAttack, rollerAttack, attackCooldown));
        }
        else if(health == 0)
        {
            phase = 4;
            StartCoroutine(HandleAttackChange(grappleDodgeAttack, null, null, 0));
        }

        phaseChange?.Invoke(phase, attackCooldown);
    }

    private IEnumerator HandleAttackChange(IAttack attackToStop, IAttack attackToStart, IAction startAction, float cooldownPeriod)
    {

        attackToStop?.StopAttack();

        yield return new WaitWhile(() => attackToStop != null && attackToStop.GetIsActive());

        yield return new WaitForSeconds(cooldownPeriod);

        attackToStart?.Attack(startAction);
    }
}
