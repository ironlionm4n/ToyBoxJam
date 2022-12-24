using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sparkleSprite;
    [SerializeField] private Color reachedColor;
    [SerializeField] PlayerDeathManager deathManager;
    [SerializeField] private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>() && !activated)
        {
            activated = true;
            sparkleSprite.color = reachedColor;
            deathManager.SetCurrentCheckpoint(gameObject);
        }
    }
}
