using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingAttack : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("OneWayPlatforms").GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.Find("Player").GetComponent<Collider2D>());

        BouncingAttack[] attacks = GameObject.FindObjectsOfType<BouncingAttack>();

        foreach (BouncingAttack b in attacks)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), b.GetComponent<Collider2D>());
        }

        rb = GetComponent<Rigidbody2D>();

        AddForce();
    }


    public void AddForce()
    {
        int[] direction = new int[] { -1, 1 };

        float[] xSpeed = new float[] { 5, 7, 10 };

        int currentDirection = UnityEngine.Random.Range(0, direction.Length);
        int currentSpeed = Random.Range(0, xSpeed.Length);

        rb.AddForce(new Vector2(direction[currentDirection] * xSpeed[currentSpeed], 3), ForceMode2D.Impulse);
    }

    public void DeleteProjectile()
    {
        Destroy(gameObject);
    }

}
