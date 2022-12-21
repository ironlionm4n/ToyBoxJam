using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
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
