using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private GameObject homingSpawn;
    [SerializeField] private GameObject fourWaySpawn;
    [SerializeField] private GameObject indicatorSpawn;
    [SerializeField] private BossAnimations animations;

    [Header("Attacks")]
    [SerializeField] private GameObject HomingAttack;
    [SerializeField] private GameObject FourWayAttack;
    [SerializeField] private GameObject IndicatorAttack;

    [Header("Homing")]
    [SerializeField] private float timeBetweenHomingShots = 8f;
    [SerializeField] private float homeShotTimer = 6f;

    [Header("Four Way")]
    [SerializeField] private float timeBetweenFourWayAttacks = 20f;
    [SerializeField] private float fourWayTimer = 19f;

    [Header("Indicator")]
    [SerializeField] private float timeBetweenIndicatorAttacks = 5f;
    [SerializeField] private float indicatorTimer = 4f;

    [Header("Stats")]
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private int phase = 1;
    [SerializeField] private bool defeated = false;
    [SerializeField] private bool attacking = false;


    // Start is called before the first frame update
    void OnEnable()
    {
        //Test Spawning
        //Instantiate(IndicatorAttack, indicatorSpawn.transform.position, Quaternion.identity);
        //Instantiate(FourWayAttack, fourWaySpawn.transform.position, Quaternion.identity);
        //Instantiate(HomingAttack, homingSpawn.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Changes music based on boss health
        if(currentHealth <= 30)
        {
            bossMusic.pitch = 1.16f;
            phase = 3;
        }
        else if(currentHealth <= 70)
        {
            bossMusic.pitch = 1.05f;
            phase = 2;
        }

        switch(phase)
        {
            case 1:
                homeShotTimer += Time.deltaTime;
                break;

            case 2:
                homeShotTimer += Time.deltaTime;
                fourWayTimer += Time.deltaTime;
                break;

            case 3:
                homeShotTimer += Time.deltaTime;
                fourWayTimer += Time.deltaTime;
                indicatorTimer += Time.deltaTime;
                break;
        }

        if(homeShotTimer > timeBetweenHomingShots)
        {
            homeShotTimer = 0;
            Instantiate(HomingAttack, homingSpawn.transform.position, Quaternion.identity);
            animations.Attack();
        }

        if(fourWayTimer > timeBetweenFourWayAttacks)
        {
            fourWayTimer = 0;
            Instantiate(FourWayAttack, fourWaySpawn.transform.position, Quaternion.identity);
            animations.Attack();
        }

        if(indicatorTimer > timeBetweenIndicatorAttacks)
        {
            indicatorTimer = 0;
            Instantiate(IndicatorAttack, indicatorSpawn.transform.position, Quaternion.identity);
            animations.Attack();
        }

    }

    public void Dead()
    {
        defeated = true;
        animations.Dead();
    }

    public void Hit(float damage)
    {
        currentHealth -= damage;

        //Checks for phases
    }
}
