using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClapAttack : MonoBehaviour
{
    private GolemManager golemManager;

    private CinemachineImpulseSource doubleClapImpulseSource;

    private GolemHand golemRightHand;

    private GolemHand golemLeftHand;

    private GolemDebrisSpawning debrisSpawner;

    [Header("Attack Config")]

    [SerializeField]
    private float moveBackAmount = 2f;

    [SerializeField]
    private float pauseTime = 0.5f;

    [SerializeField]
    private float singleSlamMoveSpeed = 10f;

    [SerializeField]
    private float singleSlamStunTime = 0.5f;

    [SerializeField]
    private float doubleSlamSpeed = 1.2f;

    private bool attacking = false;

    private bool attackFinished = false;

    private bool shaking = false;

    private Transform player;

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            GolemHand hand = child.GetComponent<GolemHand>();
            if(hand != null)
            {
                //If right hand
                if (hand.RightHand)
                {
                    golemRightHand= hand;
                }
                else
                {
                    golemLeftHand= hand;
                }
            }
        }

        doubleClapImpulseSource = GetComponent<CinemachineImpulseSource>();
        golemManager = GetComponent<GolemManager>();
        debrisSpawner = GetComponent<GolemDebrisSpawning>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //GolemManager assigns player in awake
        player = golemManager.Player;
    }

    public void AttackFinished()
    {
        golemManager.AttackDone();
    }

    public void DoubleClap()
    {
        golemRightHand.DoubleClap(moveBackAmount, pauseTime, doubleSlamSpeed, player);
        golemLeftHand.DoubleClap(moveBackAmount, pauseTime, doubleSlamSpeed, player);
    }

    public void SingleClap(int attackingHand)
    {
        if (attackingHand == 1)
        {
            golemLeftHand.SingleClap(moveBackAmount, pauseTime, singleSlamMoveSpeed, singleSlamStunTime);
        }
        else
        {
            golemRightHand.SingleClap(moveBackAmount, pauseTime, singleSlamMoveSpeed, singleSlamStunTime);
        }
    }

    public void DoubleClapShake()
    {
        if (!shaking)
        {
            shaking = true;
            CameraShakeManager.instance.CameraShake(doubleClapImpulseSource);
            debrisSpawner.StartDebris();
        }
    }
}
