using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemManager : MonoBehaviour
{
    private Transform player;

    public Transform Player { get { return player; } }

    [SerializeField]
    private float timeBetweenAttacks = 5f;

    [SerializeField, Tooltip("On scale of 1-10 with higher being more likely")]
    private int chanceForDoubleSlam = 2;

    private int currentChanceForDouble = 0;

    private bool attacking = false;
    private int attacksRunning = 0;
    private bool canAttack = false;

    private int phase = 2;

    private ClapAttack clapAttackManager;
    private WaveSlamAttack waveSlamAttackManager;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentChanceForDouble= chanceForDoubleSlam;
        clapAttackManager = GetComponent<ClapAttack>();
        waveSlamAttackManager = GetComponent<WaveSlamAttack>();

        StartCoroutine(InitialWait());
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack)
        {
            return;
        }


        if(!attacking)
        {
            attacking = true;
            attacksRunning++;

            int randomAttack = Random.Range(0, 11);

            if (currentChanceForDouble != 0 && randomAttack / currentChanceForDouble == 0)
            {

                Debug.Log("Double clap start");
                currentChanceForDouble = chanceForDoubleSlam; //Reset chance

                // do a double clap attack
                clapAttackManager.DoubleClap();
            }
            else
            {
                // do a clap attack

                currentChanceForDouble++; //Make it more likely each time to do a double slam next

                //1 = left and 2 = right
                int attackingHand = Random.Range(1, 3);

                clapAttackManager.SingleClap(attackingHand);

                if(phase > 1)
                {
                    attacksRunning++;
                    waveSlamAttackManager.Attack(attackingHand);
                }
            }
        }
    }

    private IEnumerator InitialWait()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        canAttack = true;
    }

    public void AttackDone()
    {
        attacksRunning--;

        if(attacksRunning <= 0)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        attacking = false;
    }
}
