using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Boss : MonoBehaviour
{
    // state variables
    [SerializeField] protected float BossHealth;
    protected float currentHealth;


    [SerializeField] protected int currentPhase = 1;
    [SerializeField] protected bool defeated;
    protected bool attacking;

    [SerializeField] protected List<float> PhaseThresholds = new List<float>();

    // list of all possible attacks
    [SerializeField] protected List<IAttack> Attacks;

    // ui
    [SerializeField] protected Slider healthBar;

    public void AddAttack(IAttack attack)
    {
        Attacks.Add(attack);
    }

    public virtual void Hit(float damage)
    {
        //bossHitParticles.Play();
        //bossHitAudioSource.Play();
        UpdateHealth(-damage);
    }

    public void UpdateHealth(float amount)
    {
        if (!defeated)
        {
            currentHealth = currentHealth + amount;
            healthBar.value = currentHealth;

            if (currentHealth <= 0)
            {
                healthBar.gameObject.SetActive(false);
                Dead();
                return;
            }

            SwapPhase();
        }
    }

    /// <summary>
    /// swap boss phase based on current boss health
    /// </summary>
    protected virtual void SwapPhase()
    {
        if (currentPhase <= PhaseThresholds.Count && currentHealth <= PhaseThresholds[currentPhase-1])
        {
            currentPhase++;
        }
    }

    protected virtual void Dead()
    {
        defeated = true;
    }

}
