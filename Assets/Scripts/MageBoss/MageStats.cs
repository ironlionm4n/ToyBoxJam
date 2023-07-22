using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageStats : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    public bool defeated {  get; private set; }

    public float health { get; private set; } = 100;

    public float currentHealth { get; private set; }

    public int phase { get; private set; }

    MageController controller;

    private PlayerBounceAttack bounceAttack;

    private FlailAttack flailAttack;

    private GrappleDodgeAttack grappleDodgeAttack;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<MageController>();
        bounceAttack = GetComponent<PlayerBounceAttack>();
        flailAttack = GetComponent<FlailAttack>();
        grappleDodgeAttack = GetComponent<GrappleDodgeAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Mage is hit and takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage)
    {
        UpdateHealth(-damage);

        if(health < 30)
        {
            StopCurrentAttack();
        }
        else if(health < 70)
        {
            StopCurrentAttack();
        }
    }

    /// <summary>
    /// Updates mage health bar
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateHealth(float amount)
    {
        if (!defeated)
        {
            healthBar.value = currentHealth + amount;
            currentHealth = healthBar.value;

            if (currentHealth > 100)
            {
                currentHealth = 100;
                healthBar.value = 100;
            }

            if (currentHealth <= 0)
            {
                healthBar.gameObject.SetActive(false);
                //Dead();
            }
        }
    }

    /// <summary>
    /// Stop current attack and adds slight delay before starting next one
    /// </summary>
    public void StopCurrentAttack()
    {
        phase++;

        if(phase == 1)
        {
            
        }

        if(phase == 2)
        {

        }
    }

    private IEnumerator HandleAttackChange(IAttack attackToStop, IAttack attackToStart, IAction startAction, float cooldownPeriod)
    {

        attackToStop.StopAttack();

        yield return new WaitForSeconds(cooldownPeriod);

        attackToStart.Attack(startAction);
    }
}
