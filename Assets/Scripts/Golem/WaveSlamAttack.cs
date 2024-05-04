using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSlamAttack : MonoBehaviour
{
    private GolemManager golemManager;

    private CinemachineImpulseSource impulseSource;

    private GolemHand golemRightHand;

    private GolemHand golemLeftHand;

    private GolemDebrisSpawning debrisSpawner;

    [Header("Attack Config")]
    [SerializeField] 
    private float slamSpeed = 1f;

    [SerializeField]
    private float slamPauseTime = 1f;

    [SerializeField]
    private GameObject WavePrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            GolemHand hand = child.GetComponent<GolemHand>();
            if (hand != null)
            {
                //If right hand
                if (hand.RightHand)
                {
                    golemRightHand = hand;
                }
                else
                {
                    golemLeftHand = hand;
                }
            }
        }

        impulseSource = GetComponent<CinemachineImpulseSource>();
        golemManager = GetComponent<GolemManager>();
        debrisSpawner = GetComponent<GolemDebrisSpawning>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(int attackingHand)
    {
        StartCoroutine(WaveSlam(attackingHand));
    }

    private IEnumerator WaveSlam(int attackingHand)
    {
        // wait for a random amount of time
        yield return new WaitForSeconds(Random.Range(0.6f, 1.2f));

        //2 == left and 1 == right (opposite of Clap Attack)
        if (attackingHand == 2)
        {
            golemLeftHand.WaveSlam(slamSpeed, slamPauseTime, WavePrefab);
        }
        else
        {
            golemRightHand.WaveSlam(slamSpeed, slamPauseTime, WavePrefab);
        }
    }

    public void AttackDone()
    {
        golemManager.AttackDone();
    }
}
