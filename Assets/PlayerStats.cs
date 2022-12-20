using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject coinSpawnPoint;
    [SerializeField] private AudioSource coinPickup;
    [SerializeField] private AudioSource coinThrow;

    [Header("Preabs")]
    [SerializeField] private GameObject throwableCoin;

    [Header("Collectables")]
    [SerializeField] private int numCoins = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    //Fires a coin as long as the player has some available
    public void Fire()
    {
        Instantiate(throwableCoin, coinSpawnPoint.transform.position, coinSpawnPoint.transform.rotation);
        coinThrow.Play();
    }

    public void CoinPickedUp()
    {
        numCoins++;
        coinPickup.Play();
    }
}
