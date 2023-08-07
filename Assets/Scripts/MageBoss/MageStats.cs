using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

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

    public Action<float> hit;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<MageController>();
        bounceAttack = GetComponent<PlayerBounceAttack>();
        flailAttack = GetComponent<FlailAttack>();
        grappleDodgeAttack = GetComponent<GrappleDodgeAttack>();

        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Hit(10);
        }
    }


    /// <summary>
    /// Mage is hit and takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage)
    {
        UpdateHealth(-damage);

        if(currentHealth == 0)
        {
            hit?.Invoke(currentHealth);
        }
        else if (currentHealth == 30)
        {
            hit?.Invoke(currentHealth);
        }
        else if(currentHealth == 70)
        {
            hit?.Invoke(currentHealth);
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
            currentHealth += amount;

           // healthBar.value = currentHealth;

            if (currentHealth > 100)
            {
                currentHealth = 100;
                healthBar.value = 100;
            }

            if (currentHealth <= 0)
            {
                // healthBar.gameObject.SetActive(false);
                //Dead();
            }
        }
    }

    public void Dead()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
