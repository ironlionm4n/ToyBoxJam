using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClapAttack : MonoBehaviour
{
    private GolemManager golemManager;

    private CinemachineImpulseSource impulseSource;

    private GolemHand golemRightHand;

    private GolemHand golemLeftHand;

    private GolemDebrisSpawning debrisSpawner;

    [Header("Attack Config")]

    [SerializeField]
    private float timeBetweenAttacks = 5f;

    [SerializeField, Tooltip("On scale of 1-10 with higher being more likely")]
    private int chanceForDoubleSlam = 2;

    private int currentChanceForDouble = 0;

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

        impulseSource = GetComponent<CinemachineImpulseSource>();
        golemManager = GetComponent<GolemManager>();
        debrisSpawner = GetComponent<GolemDebrisSpawning>();

        currentChanceForDouble = chanceForDoubleSlam;
    }

    // Start is called before the first frame update
    void Start()
    {
        //GolemManager assigns player in awake
        player = golemManager.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if(!attacking)
        {
            attacking = true;
            attackFinished = false;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);


        int randomAttack = Random.Range(0, 11);

        if (currentChanceForDouble != 0 && randomAttack / currentChanceForDouble == 0)
        {
            Debug.Log("Double clap start");
            currentChanceForDouble = chanceForDoubleSlam; //Reset chance

            golemRightHand.DoubleClap(moveBackAmount, pauseTime, doubleSlamSpeed, player);
            golemLeftHand.DoubleClap(moveBackAmount, pauseTime, doubleSlamSpeed, player);
        }
        else
        {
            currentChanceForDouble++; //Make it more likely each time to do a double slam next

            //1 = left and 2 = right
            int attackingHand = Random.Range(1, 3);

            if (attackingHand == 1)
            {
                golemLeftHand.SingleClap(moveBackAmount, pauseTime, singleSlamMoveSpeed, singleSlamStunTime);
            }
            else
            {
                golemRightHand.SingleClap(moveBackAmount, pauseTime, singleSlamMoveSpeed, singleSlamStunTime);
            }
        }

        yield return new WaitWhile(() => !attackFinished);

        attacking= false;
        shaking = false;
    }

    public void AttackFinished()
    {
        attackFinished = true;
    }

    public void DoubleClap()
    {
        if (!shaking)
        {
            shaking = true;
            CameraShakeManager.instance.CameraShake(impulseSource);
            debrisSpawner.StartDebris();
        }
    }
}
