using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveFeeler : MonoBehaviour
{
    public static ShockwaveFeeler instance;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShockwaveFelt(float maxStrength, float upModifier, Vector2 origin, float maxRadius, bool disableMovement, float knockbackTime)
    {
        if(disableMovement)
        {
            movement.StartCoroutine(movement.TemporaryKnockback(knockbackTime));
        }

        Vector2 toPlayer = (Vector2)transform.position - origin;
        float distance = toPlayer.magnitude;

        // Prevent divide by zero
        if (distance < 0.01f)
            toPlayer = Vector2.right;
        else
            toPlayer.Normalize();

        // Linear falloff: closer = full strength, farther = reduced
        float t = Mathf.Clamp01(distance / maxRadius);          // 0 = close, 1 = far
        float falloff = 1f - t;                                 // full at 0, zero at maxRadius
        float scaledStrength = maxStrength * falloff;

        Vector2 force = toPlayer * scaledStrength + Vector2.up * upModifier;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
