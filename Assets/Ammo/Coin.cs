using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [Header("Required Scripts")]
    [SerializeField] private Rigidbody2D ammoRigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Throwing")]
    [SerializeField] private float throwForce = 15;

    [Header("Disapearing")]
    [SerializeField] private float disapearTime = 3f;
    [SerializeField] private float flashTimer = 0f;

    [SerializeField] private float aliveTime = 5f;
    [SerializeField] private float aliveTimer = 0f;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;

    [SerializeField] Collider2D nonTriggerCollider;

    void OnEnable()
    {
        //Launches Coin Up
        ammoRigidbody.AddForce(transform.up * throwForce, ForceMode2D.Impulse);
        StartCoroutine(Disapear());

        if (GameObject.Find("Mage")) {
            Physics2D.IgnoreCollision(nonTriggerCollider, GameObject.Find("Mage").GetComponent<Collider2D>());
            return;
        }

        if (GameObject.Find("Boss"))
        {
            foreach(Collider2D col in GameObject.Find("Boss").GetComponents<Collider2D>())
            {
                Physics2D.IgnoreCollision(nonTriggerCollider, col);
            }
            return;
        }
    }

    private void Update()
    {
        aliveTimer += Time.deltaTime;
    }

    public IEnumerator Disapear()
    {
        yield return new WaitWhile(() => aliveTimer < aliveTime);

        while(flashTimer < disapearTime)
        {
            Flash();

            yield return new WaitForSeconds(0.1f);
            flashTimer += 0.1f;
        }

        Destroy(gameObject);
    }

    public void Flash()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().CoinPickedUp();
            Destroy(gameObject);
        }

        if(collision.tag == "Enemy")
        {
            if(collision.GetComponent<Enemy>() != null)
            {
                Debug.Log("Hit");
                collision.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
