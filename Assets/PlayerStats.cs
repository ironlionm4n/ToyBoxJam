using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject coinSpawnPoint;

    [Header("Preabs")]
    [SerializeField] private GameObject throwableCoin;

    [Header("Collectables")]
    [SerializeField] private int numCoins = 0;

    private bool _canFire = true;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    public void SetCanFire(bool ableToFire)
    {
        _canFire = ableToFire;
    }

    //Fires a coin as long as the player has some available
    private void Fire()
    {
        if (!_canFire) return;
        
        Instantiate(throwableCoin, coinSpawnPoint.transform.position, coinSpawnPoint.transform.rotation);
    }

    public void CoinPickedUp()
    {
        numCoins++;
    }
}
