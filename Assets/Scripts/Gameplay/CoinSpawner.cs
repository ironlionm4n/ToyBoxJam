using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject player;

    [Header("Coin")]
    [SerializeField] private GameObject coin;

    [Header("Coin Spawns")]
    [SerializeField] private GameObject[] coinSpawns;
    [SerializeField] private GameObject middleSpawnPoint;
    [SerializeField] private int lastSpawnedPosition = -1;

    // Update is called once per frame
    void Update()
    {
        
    }

    //Respawns the coin on the opposite side of the arena so the player has to constantly traverse the arena
    public void RespawnCoin()
    {
        int spawnPoint = Random.Range(0, coinSpawns.Length);
        int rand = 0;

        while(spawnPoint == lastSpawnedPosition)
        {
            spawnPoint = Random.Range(0, coinSpawns.Length);
        }

        if(spawnPoint == 3)
        {
            lastSpawnedPosition= spawnPoint;
            Instantiate(coin, coinSpawns[spawnPoint].transform.position, Quaternion.identity);
        }
        else if(player.transform.position.x > middleSpawnPoint.transform.position.x)
        {
            rand = Random.Range(0, 3);
            lastSpawnedPosition = rand;
            Instantiate(coin, coinSpawns[rand].transform.position, Quaternion.identity);
        }
        else if (player.transform.position.x < middleSpawnPoint.transform.position.x)
        {
            rand = Random.Range(4, coinSpawns.Length);
            lastSpawnedPosition = rand;
            Instantiate(coin, coinSpawns[rand].transform.position, Quaternion.identity);
        }
        else
        {
            spawnPoint = Random.Range(0, coinSpawns.Length);
            lastSpawnedPosition = spawnPoint;
            Instantiate(coin, coinSpawns[spawnPoint].transform.position, Quaternion.identity);
        }
    }
}
