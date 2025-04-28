using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private bool stunned = false;
    private bool walkingLeft = false;

    public float walkSpeed = 3f;

    private bool charging = false;

    public float chargeDetectionRange = 3f;
    public float chargeSpeedBoost = 1.5f;
    public float chargeTime = 2f;

    public float stunTimeAfterCharge = 1.5f;

    private float timer = 0f;

    private Rigidbody2D skeletonRigidbody;
    private bool playerInFront;

    private Transform player;

    public SpriteRenderer spriteRenderer;

    public GameObject CoinPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        skeletonRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (charging)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                charging = false;
                StartCoroutine(AfterChargePause());
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        CheckIfPlayerInFront();

        if(stunned) { return; }

        if(playerInFront && Vector2.Distance(transform.position, player.position ) < chargeDetectionRange){
            charging = true;
            timer = chargeTime;

        }

        if (walkingLeft)
        {
            if (charging)
            {
                skeletonRigidbody.velocity = new Vector2(-walkSpeed * chargeSpeedBoost, 0);
            }
            else
            {
                skeletonRigidbody.velocity = new Vector2(-walkSpeed, 0);
            }
            
            spriteRenderer.flipX = true;
        }
        else
        {
            if (charging)
            {
                skeletonRigidbody.velocity = new Vector2(walkSpeed * chargeSpeedBoost, 0);
            }
            else
            {
                skeletonRigidbody.velocity = new Vector2(walkSpeed, 0);
            }

            spriteRenderer.flipX = false;
        }

    }

    private void Die()
    {
        int rand = Random.Range(0, 10);

        if(rand >= 0)
        {
            SpawnCoin();
        }

        Destroy(gameObject);
    }

    private void SpawnCoin()
    {
        Quaternion spawnAngle = new Quaternion(0, 0, Random.Range(-5, 5), 0);

        Instantiate(CoinPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator AfterChargePause()
    {
        stunned = true;
        skeletonRigidbody.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(stunTimeAfterCharge);

        stunned = false;
    }
    private void CheckIfPlayerInFront()
    {
        if (walkingLeft)
        {
            if (player.position.x <= transform.position.x)
            {
                playerInFront = true;
            }
            else
            {
                playerInFront = false;
            }
        }
        else
        {
            if (player.position.x >= transform.position.x)
            {
                playerInFront = true;
            }
            else
            {
                playerInFront = false;
            }
        }

        //Debug.Log(playerInFront);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (charging)
        {
            if (collision.tag.Equals("Enemy"))
            {
                Die();
            }
        }

        if (collision.tag.Equals("Wall"))
        {
            walkingLeft = !walkingLeft;
        }
    }
}
