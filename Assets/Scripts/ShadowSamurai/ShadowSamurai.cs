using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowSamurai : MonoBehaviour
{
    private int attacksQueued = 0;

    private int phase = 1;

    // attack scripts
    private LightFlickerAttack lightFlickerAttack;

    private void Awake()
    {
        lightFlickerAttack = GetComponent<LightFlickerAttack>();

    }

    private void Start()
    {
        //StartAttacks();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            StartAttacks();
        }
    }

    public void StartAttacks()
    {
        if(phase == 1)
        {
            lightFlickerAttack.StartFlickerAttack();
        }
        else if (phase == 2)
        {

        }
        else
        {

        }

    }

    public void AttackComplete()
    {
        attacksQueued--;
    }
}
