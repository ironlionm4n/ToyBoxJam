using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerStats : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject coinSpawnPoint;
    [SerializeField] private AudioSource coinPickup;
    [SerializeField] private AudioSource coinThrow;
    [SerializeField] private AudioSource lowHealth;
    [SerializeField] AudioSource playerHitAudioSource;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Only asign in boss level")]
    [SerializeField] private CoinSpawner coinSpawner;

    [Header("Preabs")]
    [SerializeField] private GameObject throwableCoin;

    [Header("Collectables")]
    [SerializeField] private int numCoins = 0;
    [SerializeField] private bool coinPickedUp = false;
    [SerializeField] private int maxCoins = 3;
    [SerializeField] private Image[] coins;

    [Header("Health")]
    [SerializeField] private float health = 3;
    [SerializeField] private bool invincible = false;
    [SerializeField] private float invulnerableTimer = 1f;
    [SerializeField] private float invulFrameTimer = 0f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float lowHealthAlertTimer = 5f;
    [SerializeField] private bool alerting = false;
    [SerializeField] private Image[] hearts;

    [Header("Cutscene Management")]
    [SerializeField] private bool inCutscene = false;
    [SerializeField] PlayerDeathManager deathManager;

    public bool InCutscene { get { return inCutscene; } set { inCutscene = value; } }

    private bool _canFire = true;

    public Action pickedUpCoin;

    private void OnEnable()
    {
        for(int i = 0; i < coins.Length; i++)
        {
            coins[i].color = Color.gray;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inCutscene) { return; }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            CoinPickedUp();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    //Keeps player from picking up multiple coins at once
    private void LateUpdate()
    {
        coinPickedUp = false;
    }

    public void SetCanFire(bool ableToFire)
    {
        _canFire = ableToFire;
    }

    //Fires a coin as long as the player has some available
    private void Fire()
    {
        if (!_canFire) { return; }
        
        if (numCoins <= 0) { 

            coins[0].color = Color.gray;
            return;
        }

        numCoins--;
        coins[numCoins].color = Color.gray;
        Instantiate(throwableCoin, coinSpawnPoint.transform.position, coinSpawnPoint.transform.rotation);
        SFX.instance.CoinThrown();
    }

    public void CoinPickedUp()
    {
        if(numCoins >= maxCoins) { return; }
        
        coins[numCoins].color = Color.white;
        numCoins++;
        SFX.instance.CoinPickedUp();
    }

    public void Flash()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    public void Hurt(float knockbackDir)
    {
        SFX.instance.Hit();
        health--;

        if (health >= 0)
        {
            hearts[(int)health].gameObject.SetActive(false);
        }

        if (health == 1)
        {
            SFX.instance.LowHealth();
        }

        if (health <= 0)
        {
            if (!SceneManager.GetActiveScene().name.Equals("MageBoss"))
            {
                playerMovement.IsDead = true;
                StopAllCoroutines();
                deathManager.PlayerDied();
                return;
            }
        }


        invulFrameTimer = 0f;

        if (knockbackDir > 0)
        {
            playerMovement.SetKnockbacked(true);
            playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
            playerRigidbody.AddForce(new Vector2(knockbackDir * knockbackForce, 5), ForceMode2D.Impulse);
        }
        
        StartCoroutine(InvulnTime());
    }

    public IEnumerator InvulnTime()
    {

        while(invulFrameTimer < invulnerableTimer)
        {
            Flash();

            if(invulFrameTimer == 0.2f)            {
                playerMovement.SetKnockbacked(false);
            }

            yield return new WaitForSeconds(0.1f);
            invulFrameTimer += 0.1f;
        }

        //Makes sure the sprite renderer is turned back on
        if(spriteRenderer.enabled == false)
        {
            spriteRenderer.enabled = true;  
        }
        invincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin" && numCoins < maxCoins)
        {
            Destroy(collision.gameObject);

            if (coinSpawner != null && !coinPickedUp)
            {
                CoinPickedUp();
                coinPickedUp = true;
                coinSpawner.RespawnCoin();
            }
            else if (!coinPickedUp)
            {
                CoinPickedUp();
                coinPickedUp = true;
                pickedUpCoin?.Invoke();
            }
        }
    }

    public void MakeInvincible()
    {
        invincible= true;
    }

    public void SetInvicible(bool value)
    {
        invincible= value;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {   
        //With Knockback
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Fireball" || collision.gameObject.tag == "SpinningFireball" || collision.tag.Equals("Wave"))
        {
            if (!invincible)
            {
                invincible = true;

                //Figures out which way to knock player 
                float knockbackDirection = 0f;

                if(collision.gameObject.transform.position.x > transform.position.x)
                {
                    knockbackDirection = -1;
                }
                else if (collision.gameObject.transform.position.x < transform.position.x)
                {
                    knockbackDirection = 1;
                }
                else if (collision.gameObject.transform.position.x == transform.position.x)
                {
                    int choice = UnityEngine.Random.Range(0, 2);

                    if (choice == 0)
                    {
                        knockbackDirection = -1;
                    }
                    else
                    {
                        knockbackDirection = 1;
                    }
                }

                Hurt(knockbackDirection);

                if(collision.tag == "Fireball" || collision.tag.Equals("Wave"))
                {
                    Destroy(collision.gameObject);
                }
                else if(collision.tag == "SpinningFireball")
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }

        //Without Knockback
        if (collision.CompareTag("Roller"))
        {
            if (!invincible)
            {
                invincible = true;

                float knockbackDirection = 0f;

                Hurt(knockbackDirection);

                
            }
        }
    }

    public void Respawned()
    {
        health = 3;
        invincible = false;
        alerting= false;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        for(int i = 0; i < health; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
    }
}
