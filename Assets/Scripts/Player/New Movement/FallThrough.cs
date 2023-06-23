using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallThrough : MonoBehaviour
{
    private OneWay currentOneWay;

    [SerializeField] private BoxCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if(currentOneWay != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWay"))
        {
            currentOneWay = collision.gameObject.GetComponent<OneWay>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWay"))
        {
            currentOneWay = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        TilemapCollider2D platformCollider = currentOneWay.GetComponent<TilemapCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);

        yield return new WaitForSeconds(0.25f);

        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
    }
}
