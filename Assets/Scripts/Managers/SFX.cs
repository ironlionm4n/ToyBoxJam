using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX instance { get; private set; }

    [Header("SFX Clips")]
    [Header("Movement")]
    [SerializeField] private AudioSource playerRun;
    [SerializeField] private AudioSource playerJump;
    [SerializeField] private AudioSource playerDash;
    [SerializeField] private AudioSource playerLand;

    [Header("Other")]
    [SerializeField] private AudioSource coinPickup;
    [SerializeField] private AudioSource coinThrow;
    [SerializeField] private AudioSource lowHealth;
    [SerializeField] private AudioSource playerHit;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one SFX Manager. Deleting " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public void Jump()
    {
        playerRun.Stop();
        playerJump.Play();
    }

    public void Run()
    {
        if (!playerRun.isPlaying)
        {
            playerRun.Play();
        }
    }

    public void StopRunning()
    {
        if (playerRun.isPlaying)
        {
            playerRun.Stop();
        }
    }

    public void Dash()
    {
        playerDash.Play();
    }

    public void Land()
    {
        playerLand.Play();
    }

    public void CoinPickedUp()
    {
        coinPickup.Play();
    }

    public void CoinThrown()
    {
        coinThrow.Play();
    }

    public void LowHealth()
    {
        lowHealth.Play();
    }

    public void Hit()
    {
        playerHit.Play();
    }

    public void StopSounds()
    {
        playerRun.Stop();
        playerJump.Stop();
        playerLand.Stop();
    }
}
