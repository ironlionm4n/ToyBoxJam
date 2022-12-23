using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject coinSpawnPoint;
    [SerializeField] private AudioSource coinPickup;
    [SerializeField] private AudioSource coinThrow;
    [SerializeField] private AudioSource lowHealth;
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

    [Header("Health")]
    [SerializeField] private float health = 3;
    [SerializeField] private bool invincible = false;
    [SerializeField] private float invulnerableTimer = 1f;
    [SerializeField] private float invulFrameTimer = 0f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float lowHealthAlertTimer = 5f;
    [SerializeField] private bool alerting = false;

    [Header("Cutscene Management")]
    [SerializeField] private bool inCutscene = false;

    public bool InCutscene { get { return inCutscene; } set { inCutscene = value; } }

    private bool _canFire = true;

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inCutscene) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if(health == 1)
        {
            if (!alerting)
            {
                StartCoroutine(LowHealthAlert());
            }
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
        if (numCoins <= 0) return;

        numCoins--;
        Instantiate(throwableCoin, coinSpawnPoint.transform.position, coinSpawnPoint.transform.rotation);
        coinThrow.Play();
    }

    public void CoinPickedUp()
    {
        numCoins++;
        coinPickup.Play();
    }

    public void Flash()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    public void Hurt(float knockbackDir)
    {
        health--;
        
        if(health <= 0)
        {
            playerMovement.IsDead = true;
            
            return;
        }


        invulFrameTimer = 0f;

        playerMovement.SetKnockbacked(true);
        playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        playerRigidbody.AddForce(new Vector2(knockbackDir * knockbackForce, 5), ForceMode2D.Impulse);
        
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

    public IEnumerator LowHealthAlert()
    {
        alerting = true;
        
        if(health == 1)
        {
            lowHealth.Play();
        }
        yield return new WaitForSeconds(lowHealthAlertTimer);

        alerting = false;
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
        }
    }

    public void MakeInvincible()
    {
        invincible= true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {   
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Fireball" || collision.gameObject.tag == "SpinningFireball")
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
                    int choice = Random.Range(0, 1);

                    if (choice == 0)
                    {
                        knockbackDirection = -1;
                    }
                    else
                    {
                        knockbackDirection = 1;
                    }
                }

                Debug.Log("Hurt");
                Hurt(knockbackDirection);

                if(collision.tag == "Fireball")
                {
                    Destroy(collision.gameObject);
                }
                else if(collision.tag == "SpinningFireball")
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }
}
