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

    [Header("Attacks")]
    [SerializeField] private GameObject HomingAttack;
    [SerializeField] private GameObject FourWayAttack;
    [SerializeField] private GameObject IndicatorAttack;

    [Header("Stats")]
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private int phase = 1;
    [SerializeField] private bool defeated = false;


    // Start is called before the first frame update
    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Changes music based on boss health
        if(currentHealth <= 30)
        {
            bossMusic.pitch = 1.16f;
        }
        else if(currentHealth <= 70)
        {
            bossMusic.pitch = 1.05f;
        }
    }

    public void Dead()
    {
        defeated = true;
    }

    public void Hit(float damage)
    {
        currentHealth -= damage;

        //Checks for phases
    }
}
