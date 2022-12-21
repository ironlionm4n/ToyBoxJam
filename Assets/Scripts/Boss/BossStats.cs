using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private AudioSource bossMusic;

    [Header("Stats")]
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private int phase = 1;
    [SerializeField] private bool defeated = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentHealth < 30)
        {
            bossMusic.pitch = 1.16f;
        }else if(currentHealth < 70)
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
