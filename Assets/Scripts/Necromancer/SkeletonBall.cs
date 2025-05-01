using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBall : MonoBehaviour
{

    [SerializeField] private float KickForce;

    [SerializeField] private GameObject AttackPrefab;

    [SerializeField] private float SizeDecreaseAmount = 0.5f;

    // min speed ball needs to be moving to be considered kicked
    [SerializeField] private float kickedVelocityThreshold = 1.0f; 

    private bool kicked = false;

    private Rigidbody2D rigBody;
    // Start is called before the first frame update
    void Start()
    {
        rigBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(rigBody.velocity.x) > kickedVelocityThreshold)
        {
            kicked = true;
        }
        else
        {
            kicked = false;
        }
    }

    private void KickBall(Transform player)
    {
        Vector2 dir = transform.position - player.transform.position;

        dir += Vector2.up * 0.5f; // slight upwards momentum

        rigBody.AddForce(KickForce * dir, ForceMode2D.Impulse);
    }

    private void SpawnAttack(Vector2 position)
    {
        Instantiate(AttackPrefab, position, Quaternion.identity);
    }

    private void DecreaseSize()
    {
        Vector2 newScale = new Vector2(transform.localScale.x - SizeDecreaseAmount, transform.localScale.y - SizeDecreaseAmount);

        transform.localScale = newScale;

        if(newScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall")){

            rigBody.drag = 2;

            if (kicked)
            {
                // spawn bone attack
                SpawnAttack(collision.GetContact(0).point);

                // decrease size by x amount

                DecreaseSize();
            }
        }

        if (collision.gameObject.CompareTag("Player")) {
            // add kicking force
        
            KickBall(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            rigBody.drag = 0;
        }
    }
}
