using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStats : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private GameObject homingSpawn;
    [SerializeField] private GameObject fourWaySpawn;
    [SerializeField] private GameObject indicatorSpawn;
    [SerializeField] private BossAnimations animations;
    [SerializeField] private Slider healthBar;
    [SerializeField] ParticleSystem bossHitParticles;
    [SerializeField] AudioSource bossHitAudioSource;
    [SerializeField] private PlayerMovement pmove;

    [Header("Attacks")]
    [SerializeField] private GameObject HomingAttack;
    [SerializeField] private GameObject FourWayAttack;
    [SerializeField] private GameObject IndicatorAttack;

    [Header("Homing")]
    [SerializeField] private float timeBetweenHomingShots = 4f;
    [SerializeField] private float homeShotTimer = 3f;

    [Header("Four Way")]
    [SerializeField] private float timeBetweenFourWayAttacks = 20f;
    [SerializeField] private float fourWayTimer = 19f;
    [SerializeField] private bool fourWaySpawned = false;
    private Spinner currentSpinner;

    [Header("Indicator")]
    [SerializeField] private float timeBetweenIndicatorAttacks = 2f;
    [SerializeField] private float indicatorTimer = 1f;

    [Header("Stats")]
    [SerializeField] private float currentHealth = 0f;
    [SerializeField] private int phase = 1;
    [SerializeField] private bool defeated = false;
    [SerializeField] private bool attacking = false;

    [Header("Cutscene Management")]
    [SerializeField] private bool inCutscene = false;
    [SerializeField] private CutsceneManager cutscene;

    public bool InCutscene { get { return inCutscene; } set { inCutscene = value; } }


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
        if (inCutscene || defeated) { return; }

        //Changes music based on boss health
        if (currentHealth <= 30)
        {
            bossMusic.pitch = 1.16f;
            phase = 3;
        }
        else if (currentHealth <= 70)
        {
            bossMusic.pitch = 1.05f;
            phase = 2;
        }

        switch (phase)
        {
            case 1:
                homeShotTimer += Time.deltaTime;
                break;

            case 2:
                //homeShotTimer += Time.deltaTime;
                fourWayTimer += Time.deltaTime;
                break;

            case 3:
                currentSpinner.DeactivateSpinner();
                homeShotTimer += Time.deltaTime;
                //fourWayTimer += Time.deltaTime;
                indicatorTimer += Time.deltaTime;
                break;
        }

        if (homeShotTimer > timeBetweenHomingShots)
        {
            homeShotTimer = 0;
            Instantiate(HomingAttack, homingSpawn.transform.position, Quaternion.identity);
            animations.Attack();
        }

        if (fourWayTimer > timeBetweenFourWayAttacks && !fourWaySpawned)
        {
            fourWayTimer = 0;
            currentSpinner = Instantiate(FourWayAttack, fourWaySpawn.transform.position, Quaternion.identity).GetComponent<Spinner>();
            animations.Attack();
            fourWaySpawned = true;
        }

        if (indicatorTimer > timeBetweenIndicatorAttacks)
        {
            indicatorTimer = 0;
            Instantiate(IndicatorAttack, new Vector3(indicatorSpawn.transform.position.x + pmove.GetHorizontalMoveDirection() * 3, indicatorSpawn.transform.position.y), Quaternion.identity);
            animations.Attack();
        }

    }

    public void Dead()
    {
        defeated = true;
       // animations.Dead();
        cutscene.BossDead();
    }

    public void Hit(float damage)
    {
        bossHitParticles.Play();
        bossHitAudioSource.Play();
        UpdateHealth(-damage);
    }

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
                Dead();
            }
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

}
