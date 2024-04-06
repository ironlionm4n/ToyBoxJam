using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncySlime : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 10f;
    [SerializeField] private float consecutiveBounceBonus;
    [SerializeField] private float maxAdditionalForce = 50f; // Maximum additional force
    [SerializeField] private float consecutiveBounceTimeDetection = 2f; // Time window to detect consecutive bounces
    private Animator animator;
    private float lastBounceTime = -1;
    private int consecutiveBounceCount = 0;
    private static readonly int Bounce = Animator.StringToHash("Bounce");

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger(Bounce);
            float timeSinceLastBounce = Time.time - lastBounceTime;
            if (timeSinceLastBounce <= consecutiveBounceTimeDetection)
            {
                consecutiveBounceCount++;
            }
            else
            {
                consecutiveBounceCount = 1; // Reset to 1 for the first bounce in a new sequence
            }

            lastBounceTime = Time.time;

            float additionalForce = Mathf.Min(consecutiveBounceCount * consecutiveBounceBonus, maxAdditionalForce);
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * (_bounceForce + additionalForce), ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (Time.time > lastBounceTime + consecutiveBounceTimeDetection && consecutiveBounceCount > 0)
        {
            consecutiveBounceCount = 0;
        }
    }
}
