using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapAttack : MonoBehaviour
{

    private GolemHand golemRightHand;

    private GolemHand golemLeftHand;

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
    private float slamSpeed = 1.2f;

    private bool attacking = false;

    private bool attackFinished = false;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!attacking)
        {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        //1 = left and 2 = right
        int attackingHand = Random.Range(1, 3);

        if(attackingHand == 1)
        {
            golemLeftHand.SingleClap(moveBackAmount, pauseTime, slamSpeed);
        }
        else
        {
            golemRightHand.SingleClap(moveBackAmount, pauseTime, slamSpeed);
        }

        yield return new WaitWhile(() => !attackFinished);

        attacking= false;
    }

    public void AttackFinished()
    {
        attackFinished = true;
    }
}
